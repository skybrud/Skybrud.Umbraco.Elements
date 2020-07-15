using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Skybrud.Essentials.Strings;
using Skybrud.Umbraco.Elements.Converters;
using Skybrud.Umbraco.Elements.Models.ContentTypes;
using Skybrud.Umbraco.Elements.Models.Images;
using Skybrud.WebApi.Json;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Skybrud.Umbraco.Elements.Controllers.Api {

    [JsonOnlyConfiguration]
    [PluginController("Skybrud")]
    public class ElementsController : UmbracoAuthorizedApiController {

        private readonly ElementsConverterCollection _converters;

        #region Constructors

        public ElementsController(ElementsConverterCollection converters) {
            _converters = converters;
        }

        #endregion

        #region Public API methods

        [HttpGet]
        public object GetContentTypes(string ids) {

            List<ElementsType> temp = new List<ElementsType>();

            foreach (string id in StringUtils.ParseStringArray(ids)) {

                if (Guid.TryParse(id, out Guid guid)) {

                    var ct = Services.ContentTypeService.Get(guid);

                    if (ct == null) continue;

                    temp.Add(new ElementsType(ct, Services));

                } else if (int.TryParse(id, out int numeric)) {

                    var ct = Services.ContentTypeService.Get(numeric);

                    if (ct == null) continue;

                    temp.Add(new ElementsType(ct, Services));

                }

            }

            ElementsEventManager.Emit(ActionContext, new EditorModelEventArgs(temp, UmbracoContext));

            return temp;

        }


        [HttpGet]
        public object GetConverters() {

            return _converters.ToArray().Select(x => new {
                assembly = x.GetType().Assembly.FullName,
                key = x.GetType().AssemblyQualifiedName,
                name = x.Name
            });

        }

        [HttpGet]
        public object GetImage(string id, int width = 350, int height = 250) {

            ElementsImage image = GetImageFromCache(id, width, height) ?? GetImageFromService(id, width, height);

            return Request.CreateResponse(image == null ? HttpStatusCode.NotFound : HttpStatusCode.OK, image);

        }

        [HttpGet]
        public object GetImages(string ids, int width = 350, int height = 250) {

            List<object> results = new List<object>();

            foreach (string id in (ids ?? string.Empty).Split(',')) {

                ElementsImage media = GetImageFromCache(id, width, height) ?? GetImageFromService(id, width, height);
                if (media == null) return null;

                results.Add(media);

            }

            return results;

        }

        #endregion

        #region Private helper methods

        private ElementsImage GetImageFromCache(string id, int width, int height) {

            IPublishedContent media = null;

            if (Udi.TryParse(id, out Udi udi)) {
                media = Umbraco.Media(udi);
            } else if (int.TryParse(id, out int numericId)) {
                media = Umbraco.Media(numericId);
            }

            return media == null ? null : new ElementsImage(media, width, height);

        }

        private ElementsImage GetImageFromService(string id, int width, int height) {

            IMedia media = null;

            if (Udi.TryParse(id, out Udi udi) && udi is GuidUdi guidUdi) {
                media = Services.MediaService.GetById(guidUdi.Guid);
            } else if (int.TryParse(id, out int numericId)) {
                media = Services.MediaService.GetById(numericId);
            }

            return media == null ? null : new ElementsImage(media, width, height);

        }

        #endregion

    }

}