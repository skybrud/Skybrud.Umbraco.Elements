using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Skybrud.Umbraco.Elements.Models.ContentTypes {

    public class SkybrudElementsPropertyGroup {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("propertyTypes")]
        public IEnumerable<SkybrudElementsProperty> PropertyTypes { get; set; }

        public SkybrudElementsPropertyGroup(PropertyGroup group, IDataTypeService dataTypeService) {
            Name = group.Name;
            PropertyTypes = group.PropertyTypes.Select(x => new SkybrudElementsProperty(x, dataTypeService));
        }

    }

}