using System;
using System.Collections.Generic;
using Skybrud.Essentials.Strings;
using Skybrud.Umbraco.Elements.Models;
using Skybrud.WebApi.Json;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Skybrud.Umbraco.Elements.Controllers.Api {

    [JsonOnlyConfiguration]
    [PluginController("Skybrud")]
    public class ElementsController : UmbracoAuthorizedApiController {

        public object GetContentTypes(string ids) {

            List<object> temp = new List<object>();

            foreach (string id in StringUtils.ParseStringArray(ids)) {

                if (Guid.TryParse(id, out Guid guid)) {

                    var ct = Services.ContentTypeService.Get(guid);

                    if (ct == null) continue;

                    temp.Add(new SkybrudElementType(ct, Services));

                } else if (int.TryParse(id, out int numeric)) {

                    var ct = Services.ContentTypeService.Get(numeric);

                    if (ct == null) continue;

                    temp.Add(new SkybrudElementType(ct, Services));

                }

            }

            return temp;

        }

        public object GetImage(string id, int width = 350, int height = 250) {

            IPublishedContent media = null;

            if (Udi.TryParse(id, out Udi udi)) {
                media = Umbraco.Media(udi);
            } else if (int.TryParse(id, out int numericId)) {
                media = Umbraco.Media(numericId);
            }

            if (media == null) return null;

            string cropUrl = media.GetCropUrl(width, height, preferFocalPoint: true);

            return new {
                id = media.Id,
                key = media.Key,
                udi = Udi.Create("media", media.Key),
                name = media.Name,
                cropUrl
            };

        }

        public object GetImages(string ids, int width = 350, int height = 250) {

            List<object> results = new List<object>();

            foreach (string id in (ids ?? string.Empty).Split(',')) {

                IPublishedContent media = null;

                if (Udi.TryParse(id, out Udi udi)) {
                    media = Umbraco.Media(udi);
                } else if (int.TryParse(id, out int numericId)) {
                    media = Umbraco.Media(numericId);
                }

                if (media == null) return null;

                string cropUrl = media.GetCropUrl(width, height, preferFocalPoint: true);

                results.Add(new {
                    id = media.Id,
                    key = media.Key,
                    udi = Udi.Create("media", media.Key),
                    name = media.Name,
                    cropUrl
                });

            }

            return results;

        }

    }

}