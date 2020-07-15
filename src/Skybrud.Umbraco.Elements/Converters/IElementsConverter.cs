using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Skybrud.Umbraco.Elements.PropertyEditors.Elements;
using Umbraco.Core.Models.PublishedContent;

namespace Skybrud.Umbraco.Elements.Converters {
    
    public interface IElementsConverter {

        [JsonProperty("name")]
        string Name { get; }

        object Convert(IPublishedElement owner, IPublishedPropertyType propertyType, IEnumerable<IPublishedElement> elements, ElementsConfiguration config);

        Type GetType(IPublishedPropertyType propertyType, ElementsConfiguration config);

    }

}