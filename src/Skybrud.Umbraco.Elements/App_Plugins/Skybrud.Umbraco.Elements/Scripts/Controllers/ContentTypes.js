angular.module("umbraco").controller("Skybrud.Umbraco.Elements.ContentTypePicker.Controller", function ($scope, contentTypeResource, editorService) {

    // Make sure we have an array
    if (!Array.isArray($scope.model.value)) $scope.model.value = [];

    // Initialize empty arrays
    $scope.selected = [];
    $scope.contentTypes = [];

    // Get the content types
    contentTypeResource.getAll().then(function (r) {

        var lookup = {};

        // Limit the content types to only element types
        $scope.contentTypes = _.filter(r, (x) => x.isElement);

        r.forEach(function (t) {
            lookup[t.key] = t;
        });

        $scope.model.value.forEach(function (item) {

            // Legacy values may be saved as a string array
            if (typeof item === "string") item = { key: item };

            // Look up the content
            var ct = lookup[item.key];

            // Append to selected items if found
            if (ct) {
                ct.elementsSettings = item.config || {};
                $scope.selected.push(ct);
            }

        });

    });

    // Ensures changes in "$scope.selected" are pushed back to "$scope.model.value"
    function sync() {

        var temp = [];

        $scope.selected.forEach(function (ct) {
            temp.push({
                key: ct.key,
                config: ct.elementsSettings
            });
        });

        $scope.model.value = temp;

    }

    // Opens up a new picker for adding an additional content type
    $scope.addItem = function () {
        editorService.itemPicker({
            title: "Select element type",
            availableItems: $scope.contentTypes,
            selectedItems: $scope.selected,
            submit: function (model) {
                $scope.selected.push(model.selectedItem);
                sync();
                editorService.close();
            },
            close: function () {
                editorService.close();
            }
        });
    };

    // Removes the content type at the specified index
    $scope.removeItem = function (index) {
        $scope.selected.splice(index, 1);
        sync();
    };

    // Opens up a new editor for managing the settings of "item"
    $scope.editSettings = function (item) {

        item.elementsSettings = item.elementsSettings || {};

        var properties = [
            {
                alias: "nameTemplate",
                label: "Template",
                description: "Specify an Angular expression to be used as the naming template.",
                view: "textbox",
                value: item.elementsSettings.nameTemplate
            },
            {
                alias: "view",
                label: "View",
                description: "Specify the URL for a partial view to be used for items of this type.",
                view: "textbox",
                value: item.elementsSettings.view
            }
        ];

        editorService.open({
            title: "Configuration",
            view: "/App_Plugins/Skybrud.Umbraco.Elements/Views/Properties.html",
            size: "small",
            properties: properties,
            submit: function (model) {
                model.properties.forEach(function (p) {
                    item.elementsSettings[p.alias] = p.value;
                });
                sync();
                editorService.close();
            },
            close: function () {
                editorService.close();
            }
        });

    };

});