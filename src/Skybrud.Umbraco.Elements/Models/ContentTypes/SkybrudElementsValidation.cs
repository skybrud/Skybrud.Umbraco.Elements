using Newtonsoft.Json;
using Umbraco.Core.Models;

namespace Skybrud.Umbraco.Elements.Models.ContentTypes {

    public class SkybrudElementsValidation {

        [JsonProperty("mandatory")]
        public bool IsMandatory { get; set; }

        public SkybrudElementsValidation(PropertyType propertyType) {
            IsMandatory = propertyType.Mandatory;
        }

    }

}