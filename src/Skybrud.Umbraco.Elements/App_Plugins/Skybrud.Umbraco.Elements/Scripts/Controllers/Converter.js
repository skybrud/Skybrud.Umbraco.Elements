angular.module("umbraco").controller("Skybrud.Umbraco.Elements.Converter.Controller", function ($scope, skyElements) {

    $scope.types = [
        {
            key: "",
            name: "No converter"
        }
    ];

    $scope.selected = $scope.types[0];

    $scope.error = null;

    skyElements.getConverters().then(function (r) {

        $scope.types = $scope.types.concat(r.data);

        var found = false;

        r.data.forEach(function (t) {

            if ($scope.model.value && t.key === $scope.model.value.key) {
                $scope.selected = t;
                found = true;
            }

        });

        if (!found && $scope.model.value && $scope.model.value.key) {
            $scope.error = `An item converter with the key <strong>${$scope.model.value.key}</strong> could not be found.`;
        }

    });

    $scope.updated = function () {

        $scope.error = null;

        if (!$scope.selected.key) {
            $scope.model.value = null;
            return;
        }

        $scope.model.value = {
            key: $scope.selected.key
        };

    };

});