using Newtonsoft.Json;
using Umbraco.Core.Models;

namespace Skybrud.Umbraco.Elements.Models.ContentTypes {

    public class ElementsValidation {

        [JsonProperty("mandatory")]
        public bool IsMandatory { get; set; }

        public ElementsValidation() { }

        public ElementsValidation(PropertyType propertyType) {
            IsMandatory = propertyType.Mandatory;
        }

    }

}