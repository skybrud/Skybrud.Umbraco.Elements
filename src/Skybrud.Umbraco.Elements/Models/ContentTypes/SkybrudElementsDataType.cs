using Newtonsoft.Json;
using Umbraco.Core.Models;

namespace Skybrud.Umbraco.Elements.Models.ContentTypes {

    public class SkybrudElementsDataType {

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("view")]
        public string View { get; set; }

        [JsonProperty("config")]
        public object Config { get; set; }

        public SkybrudElementsDataType() { }

        public SkybrudElementsDataType(IDataType dataType) {
            Alias = dataType.Editor.Alias;
            View = dataType.Editor.GetValueEditor(dataType.Configuration).View;
            Config = dataType.Editor.GetConfigurationEditor().ToValueEditor(dataType.Configuration);
        }

    }

}