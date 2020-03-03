angular.module("umbraco").factory("skyElements", function ($http) {

    return {

        // Returns a list of the content types specified in "allowedTypes"
        getContentTypes: function (allowedTypes) {

            // Construct an array with all content type keys
            var array = [];
            allowedTypes.forEach(function (ct) {
                array.push(typeof ct === "string" ? ct : ct.key);
            });

            return $http.get("/umbraco/backoffice/Skybrud/Elements/GetContentTypes?ids=" + array.join(","));

        },

        // Gets information about the image with "id"
        getImage: function (id, width, height) {

            if (!width) width = 250;
            if (!height) height = 175;

            return $http({
                method: "GET",
                url: "/umbraco/backoffice/Skybrud/Elements/GetImage?id=" + id + "&width=" + width + "&height=" + height,
                umbIgnoreStatus: [404]
            });

        },

        // Gets information about the image with "id"
        getImages: function (ids, width, height) {
            if (!width) width = 250;
            if (!height) height = 175;
            return $http.get("/umbraco/backoffice/Skybrud/Elements/GetImages?ids=" + ids.join(",") + "&width=" + width + "&height=" + height);
        }

    };

});