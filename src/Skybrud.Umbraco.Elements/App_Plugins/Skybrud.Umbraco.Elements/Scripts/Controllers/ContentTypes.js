angular.module("umbraco").controller("Skybrud.Umbraco.Elements.ContentTypePicker.Controller", function ($scope, contentTypeResource, editorService) {

    // Make sure we have an array
    if (!Array.isArray($scope.model.value)) $scope.model.value = [];

    // Initialize empty arrays
    $scope.selected = [];
    $scope.contentTypes = [];

    // Get the content types
    contentTypeResource.getAll().then(function (r) {
        $scope.contentTypes = r;
        r.forEach(function (t) {
            if ($scope.model.value.indexOf(t.key) >= 0) {
                $scope.selected.push(t);
            }
        });

    });

    $scope.addItem = function () {
        editorService.itemPicker({
            availableItems: $scope.contentTypes,
            selectedItems: $scope.selected,
            submit: function (model) {
                $scope.selected.push(model.selectedItem);
                $scope.model.value.push(model.selectedItem.key);
                editorService.close();
            },
            close: function () {
                editorService.close();
            }
        });
    };

    $scope.removeItem = function (index) {
        $scope.selected.splice(index, 1);
        $scope.model.value.splice(index, 1);
    };

});