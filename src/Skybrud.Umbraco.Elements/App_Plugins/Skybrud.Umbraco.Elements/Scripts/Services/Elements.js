angular.module("umbraco").factory("skyElements", function ($http) {

    return {
        getContentTypes: function (allowedTypes) {

            // Construct an array with all content type keys
            var array = [];
            allowedTypes.forEach(function (ct) {
                array.push(typeof ct === "string" ? ct : ct.key);
            });

            return $http.get("/umbraco/backoffice/Skybrud/Elements/GetContentTypes?ids=" + array.join(","));

        },
        getImage: function(id) {
            return $http.get("/umbraco/backoffice/Skybrud/Elements/GetImage?id=" + id + "&width=250&height=175");
        },
        getImages: function(ids) {
            return $http.get("/umbraco/backoffice/Skybrud/Elements/GetImages?ids=" + ids.join(",") + "&width=250&height=175");
        }
    };

});