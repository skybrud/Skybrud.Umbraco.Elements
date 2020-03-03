angular.module("umbraco").directive("skybrudElements", function ($interpolate, localizationService, editorService, overlayService, skyElements) {

    // https://stackoverflow.com/a/2117523
    function uuidv4() {
        return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c === "x" ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    return {
        scope: {
            value: "=",
            config: "=",
            view: "=",
            row: "=",
            control: "="
        },
        transclude: true,
        restrict: "E",
        replace: true,
        template: "<div class=\"skybrud-elements\"><div ng-if=\"loaded\" ng-include=\"view\"></div></div>",
        link: function (scope) {

            if (!scope.view) scope.view = "/App_Plugins/Skybrud.Umbraco.Elements/Views/Partials/Default.html";

            // Init value
            if (scope.value && scope.value.contentType) scope.value = [scope.value];
            if (!Array.isArray(scope.value)) scope.value = [];

            // Init config
            if (!scope.config) scope.config = {};
            if (!scope.config.minItems) scope.config.minItems = 0;
            if (scope.config.maxItems === 0 || scope.config.maxItems === undefined) scope.config.maxItems = 999;
            scope.config.singlePicker = scope.config.singlePicker === true;
            if (scope.config.singlePicker) scope.config.maxItems = 1;
            scope.config.confirmDeletes = scope.config.confirmDeletes !== false;

            // Init scope variables
            scope.items = [];
            scope.contentTypes = [];
            scope.contentTypesLookup = {};

            scope.loading = true;

            // Stop further execution if there are no selected content type
            if (scope.config.allowedTypes.length === 0) return;

            // Get data about the allowed element types
            skyElements.getContentTypes(scope.config.allowedTypes).then(function (r) {

                scope.contentTypes = r.data;
                scope.contentTypes.forEach(function (ct) {
                    ct.config = {};
                    scope.contentTypesLookup[ct.key] = ct;

                    // Gets whether a property type with "alias" exists on "ct"
                    ct.hasPropertyType = function (alias) {
                        for (var i = 0; i < ct.propertyGroups.length; i++) {
                            for (var j = 0; j < ct.propertyGroups[i].propertyTypes.length; j++) {
                                if (ct.propertyGroups[i].propertyTypes[j].alias === alias) return true;
                            }
                        }
                        return false;
                    };

                });

                scope.value.forEach(function (v) {
                    scope.items.push({
                        contentType: scope.contentTypesLookup[v.contentType],
                        value: v
                    });
                });

                // Set the name expression if a name template is configured
                scope.config.allowedTypes.forEach(function (item) {
                    if (typeof item === "string") return;
                    var ct = scope.contentTypesLookup[item.key];
                    if (ct) {
                        ct.config = item.config || {};
                        if (ct.config.nameTemplate) ct.nameExp = $interpolate(ct.config.nameTemplate);
                    }
                });

                scope.loading = false;
                scope.loaded = true;

            });

            // Ensures changes in "scope.items" are pushed back to "scope.value"
            scope.sync = function () {
                var temp = [];
                scope.items.forEach(function (item) {
                    temp.push(item.value);
                });
                scope.value = temp;
            };

            // Swaps the items as indexes "index1" and "index2"
            scope.swap = function (index1, index2) {
                var first = scope.items[index1];
                scope.items[index1] = scope.items[index2];
                scope.items[index2] = first;
                scope.sync();
            };

            // Initializes the model for a new item
            scope.initNewItem = function (contentType, properties) {
                return {
                    contentType: contentType,
                    value: {
                        key: uuidv4(),
                        contentType: contentType.key,
                        properties: properties || {}
                    }
                };
            };

            // Opens the editor for a new item, and adds the item when the user submits
            scope.addItemFromContentType = function (contentType, properties, callback) {
                var item = scope.initNewItem(contentType, properties);
                scope.editItem(item, function () {
                    scope.items.push(item);
                    scope.sync();
                    if (callback) callback(item);
                });
            };

            // Opens an editor for adding a new item. If more than one content type is allowed, the user is prompted to
            // select the type in order to continue
            scope.addItem = function () {

                if (scope.contentTypes.length === 1) {
                    scope.addItemFromContentType(scope.contentTypes[0]);
                    return;
                }

                editorService.itemPicker({
                    title: "Select type",
                    filter: scope.contentTypes.length > 12,
                    availableItems: scope.contentTypes,
                    size: scope.contentTypes.length > 6 ? "medium" : "small",
                    submit: function (model) {
                        editorService.close();
                        scope.addItemFromContentType(model.selectedItem);
                    },
                    close: function () {
                        editorService.close();
                    }
                });

            };

            // Removes the item at "index"
            scope.deleteItem = function (index) {
                scope.items.splice(index, 1);
                scope.sync();
            };

            // Prompts the user to confirm the deletion
            scope.requestDeleteItem = function (index) {

                if (scope.items.length <= scope.config.minItems) return;

                if (scope.config.confirmDeletes === true) {
                    localizationService.localizeMany([
                        "content_nestedContentDeleteItem",
                        "general_delete",
                        "general_cancel",
                        "contentTypeEditor_yesDelete"
                    ]).then(function (data) {
                        var overlay = {
                            title: data[1],
                            content: data[0],
                            closeButtonLabel: data[2],
                            submitButtonLabel: data[3],
                            submitButtonStyle: "danger",
                            submit: function () {
                                scope.deleteItem(index);
                                overlayService.close();
                            },
                            close: function () {
                                overlayService.close();
                            }
                        };
                        overlayService.open(overlay);
                    });
                } else {
                    scope.deleteItem(index);
                }

            };

            // Opens item editor in overlay
            scope.editItem = function (item, callback) {

                var groups = [];
                var properties = [];

                item.contentType.propertyGroups.forEach(function (pg) {

                    var propertyGroup = {
                        name: pg.name,
                        properties: []
                    };

                    pg.propertyTypes.forEach(function (pt) {

                        // Map the property
                        var p = {
                            alias: pt.alias,
                            label: pt.name,
                            description: pt.description,
                            view: pt.dataType.view,
                            value: item.value.properties[pt.alias] ? item.value.properties[pt.alias] : null,
                            config: pt.dataType.config,
                            validation: pt.validation
                        };

                        // Push the property to the properties collections
                        properties.push(p);
                        propertyGroup.properties.push(p);

                    });

                    groups.push(propertyGroup);

                });

                editorService.open({
                    title: "Edit element",
                    view: "/App_Plugins/Skybrud.Umbraco.Elements/Views/Properties.html",
                    size: "medium",
                    groups: groups,
                    submit: function (model) {
                        model.groups.forEach(function (g) {
                            g.properties.forEach(function (p) {
                                item.value.properties[p.alias] = p.value;
                            });
                        });
                        editorService.close();
                        if (callback) callback(item);
                    },
                    close: function () {
                        editorService.close();
                    }
                });

            };

            // Helper method used for getting the visual name of an item 
            scope.getName = function (item, index) {

                var name = "";

                // first try getting a name using the configured name template
                if (item.contentType.nameExp) {

                    item.value.properties["$index"] = index + 1;
                    var newName = item.contentType.nameExp(item.value.properties);
                    if (newName && (newName = $.trim(newName))) {
                        name = newName;
                    }

                    // Delete the index property as we don't want to persist it
                    delete item.value.properties["$index"];

                }

                // if we still do not have a name and we have multiple content types to choose from, use the content type name (same as is shown in the content type picker)
                if (!name && scope.contentTypes.length > 1) {
                    name = item.contentType.name;
                }

                if (!name) {
                    name = "Item " + (index + 1);
                }

                return name;

            };

        }
    };

});