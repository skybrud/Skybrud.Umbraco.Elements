using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Skybrud.Umbraco.Elements.Models.ContentTypes {

    public class ElementsPropertyGroup {

        #region Properties

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("propertyTypes")]
        public IEnumerable<ElementsPropertyType> PropertyTypes { get; set; }

        #endregion

        #region Constructors

        public ElementsPropertyGroup() {
            PropertyTypes = new List<ElementsPropertyType>();
        }

        public ElementsPropertyGroup(PropertyGroup group, IDataTypeService dataTypeService) {
            Name = group.Name;
            PropertyTypes = group.PropertyTypes.Select(x => new ElementsPropertyType(x, dataTypeService));
        }

        public ElementsPropertyGroup(IEnumerable<PropertyGroup> groups, IDataTypeService dataTypeService) {
            Id = groups.First().Id;
            Name = groups.First().Name;
            PropertyTypes = groups
                .OrderBy(x => x.SortOrder)
                .SelectMany(x => x.PropertyTypes.Select(y => new ElementsPropertyType(y, dataTypeService)));
        }

        #endregion

    }

}