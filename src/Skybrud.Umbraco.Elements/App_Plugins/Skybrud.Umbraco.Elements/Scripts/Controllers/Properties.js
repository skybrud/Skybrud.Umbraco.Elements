angular.module("umbraco").controller("Skybrud.Umbraco.Elements.Properties.Controller", function ($scope, formHelper) {

    if (Array.isArray($scope.model.groups)) {

        $scope.model.groups.forEach(function(g) {
            g.open = true;
            g.collapsible = true;
        });

    }

    $scope.save = function () {

        if (formHelper.submitForm({ scope: $scope })) {

            formHelper.resetForm({ scope: $scope });

            $scope.model.submit($scope.model);

        }

    };

});