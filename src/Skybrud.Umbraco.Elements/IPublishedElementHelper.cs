using Newtonsoft.Json.Linq;
using Umbraco.Core.Models.PublishedContent;

namespace Skybrud.Umbraco.Elements {

    public interface IPublishedElementHelper {

        IPublishedElement ParseElement(JObject obj);

        IPublishedElement[] ParseElements(JArray array);

    }

}