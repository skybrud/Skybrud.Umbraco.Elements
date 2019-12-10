angular.module("umbraco").factory("skyElements", function ($http) {

    return {
        getImage: function(id) {
            return $http.get("/umbraco/backoffice/Skybrud/Elements/GetImage?id=" + id + "&width=250&height=175");
        },
        getImages: function(ids) {
            return $http.get("/umbraco/backoffice/Skybrud/Elements/GetImages?ids=" + ids.join(",") + "&width=250&height=175");
        }
    };

});