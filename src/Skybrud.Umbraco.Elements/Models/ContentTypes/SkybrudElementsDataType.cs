using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

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
            View = dataType.Editor.GetValueEditor().View;
            Config = GetConfig(dataType.Configuration);
        }

        private object GetConfig(object obj) {

            if (obj == null) return null;

            if (obj is Dictionary<string, object> dictionary) return dictionary;

            Dictionary<string, object> temp = new Dictionary<string, object>();

            // Umbraco apparently sets the "idType" config option on-the-fly
            if (obj is MediaPickerConfiguration) {
                temp.Add("idType", "udi");
            }

            foreach (var property in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)) {

                var cf = property.GetCustomAttribute<ConfigurationFieldAttribute>();

                if (cf != null) {

                    temp.Add(cf.Key, property.GetValue(obj, null));

                } else {

                    temp.Add(property.Name, property.GetValue(obj, null));

                }

            }

            return temp;

        }

    }

}