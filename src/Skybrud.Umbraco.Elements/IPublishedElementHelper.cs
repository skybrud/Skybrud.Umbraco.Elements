using Newtonsoft.Json.Linq;
using Umbraco.Core.Models.PublishedContent;

namespace Skybrud.Umbraco.Elements {

    public interface IPublishedElementHelper {

        IPublishedElement[] Deserialize(string str);

        IPublishedElement[] Deserialize(IPublishedElement parent, string str);

        IPublishedElement ParseElement(JObject obj);

        IPublishedElement ParseElement(IPublishedElement parent, JObject obj);

        IPublishedElement[] ParseElements(JArray array);

        IPublishedElement[] ParseElements(IPublishedElement parent, JArray array);

    }

}