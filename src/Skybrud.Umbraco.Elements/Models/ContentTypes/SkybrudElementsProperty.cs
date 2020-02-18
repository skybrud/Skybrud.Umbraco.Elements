using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Skybrud.Umbraco.Elements.Models.ContentTypes {

    public class SkybrudElementsProperty {

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("dataType")]
        public SkybrudElementsDataType DataType { get; set; }

        [JsonProperty("validation")]
        public SkybrudElementsValidation Validation { get; set; }

        public SkybrudElementsProperty(PropertyType propertyType, IDataTypeService dataTypeService) {

            IDataType dataType = dataTypeService.GetDataType(propertyType.DataTypeId);

            Alias = propertyType.Alias;
            Name = propertyType.Name;
            Description = propertyType.Description;
            DataType = new SkybrudElementsDataType(dataType);
            Validation = new SkybrudElementsValidation(propertyType);

        }

    }

}