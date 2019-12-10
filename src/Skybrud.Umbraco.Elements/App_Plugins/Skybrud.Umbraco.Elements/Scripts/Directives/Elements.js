angular.module("umbraco").directive("skybrudElements", function ($http, editorService) {

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
            view: "="
        },
        transclude: true,
        restrict: "E",
        replace: true,
        template: "<div><div ng-include=\"view\"></div></div>",
        link: function (scope) {

            if (!scope.view) scope.view = "/App_Plugins/Skybrud.Umbraco.Elements/Views/Partials/Multiple/Default.html";

            scope.contentTypes = [];
            scope.contentTypesLookup = {};

            if (!Array.isArray(scope.value)) scope.value = [];

            scope.hest = {
                value: scope.value,
                config: scope.config
            };
            
            // Get data about the allowed element types
            $http.get("/umbraco/backoffice/Skybrud/Elements/GetContentTypes?ids=" + scope.config.allowedTypes.join(",")).then(function (r) {
                scope.contentTypes = r.data;
                scope.contentTypes.forEach(function (e) {
                    scope.contentTypesLookup[e.key] = e;
                });
            });

            scope.getContentType = function (item) {
                if (!item) return null;
                var id = typeof item === "object" ? item.contentType : item;
                return scope.contentTypesLookup[id];
            };

            scope.addItem = function (ct, properties, callback) {

                var item = {
                    key: uuidv4(),
                    contentType: ct.key,
                    properties: properties || {}
                };
                
                scope.value.push(item);

                if (typeof callback === "function") {
                    callback(item, ct);
                } else if (callback === true) {
                    scope.editItem(item);
                }

            };

            scope.editItem = function (item, callback) {

                var ct = scope.contentTypesLookup[item.contentType];

                var properties = [];

                ct.propertyTypes.forEach(function(propertyType) {
                    properties.push({
                        alias: propertyType.alias,
                        label: propertyType.name,
                        description: propertyType.description,
                        view: propertyType.dataType.view,
                        value: item.properties[propertyType.alias] ? item.properties[propertyType.alias] : null,
                        config: propertyType.dataType.config,
                        validation: propertyType.validation
                    });
                });

                editorService.open({
                    title: "Edit element",
                    view: "/App_Plugins/Skybrud.Umbraco.Elements/Views/Properties.html",
                    //size: "small",
                    properties: properties,
                    submit: function (model) {
                        model.properties.forEach(function(p) {
                            item.properties[p.alias] = p.value;
                        });
                        editorService.close();
                        if (callback) callback(item);
                    },
                    close: function () {
                        editorService.close();
                    }
                });

            };

            scope.removeItem = function(index) {
	            scope.value.splice(index, 1);
            };

            scope.swap = function(a, b) {

                var c = scope.value[a];
                var d = scope.value[b];

                scope.value[a] = d;
                scope.value[b] = c;

            };

        }
    };
});