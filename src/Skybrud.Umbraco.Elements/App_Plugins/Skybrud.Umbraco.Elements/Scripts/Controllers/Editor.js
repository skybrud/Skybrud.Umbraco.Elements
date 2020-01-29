angular.module("umbraco").controller("Skybrud.Umbraco.Elements.Editor.Controller", function ($scope) {

    // Make sure the "hideLabel" prevalue is applied to the UI
    if ($scope.model && $scope.model.config && $scope.model.config.hideLabel) {
        $scope.model.hideLabel = true;
    }

});