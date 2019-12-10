using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;
using Umbraco.Web.PropertyEditors;

namespace Skybrud.Umbraco.Elements.Models {

    public class SkybrudElementType {

        [JsonProperty("id")]
        public int Id { get; }

        [JsonProperty("key")]
        public Guid Key { get; }

        [JsonProperty("alias")]
        public string Alias { get; }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("icon")]
        public string Icon { get; }

        //[JsonProperty("geometry")]
        //public MapsContentTypeGeometryProperty Geometry { get; }

        [JsonProperty("propertyTypes")]
        public object PropertyTypes { get; }

        public SkybrudElementType(IContentType ct, ServiceContext services) {

            Id = ct.Id;
            Key = ct.Key;
            Name = ct.Name;
            Icon = ct.Icon;
            Alias = ct.Alias;

            //foreach (PropertyType propertyType in ct.PropertyTypes) {
            //    if (propertyType.PropertyEditorAlias.StartsWith("Skybrud.Umbraco.Maps.Geometry.")) {
            //        Geometry = new MapsContentTypeGeometryProperty(propertyType);
            //        break;
            //    }
            //}

            List<object> temp = new List<object>();
            
            foreach (PropertyType propertyType in ct.PropertyTypes) {

                IDataType dataType = services.DataTypeService.GetDataType(propertyType.DataTypeId);

                IDataEditor propertyEditor = dataType.Editor;

                temp.Add(new {
                    alias = propertyType.Alias,
                    name = propertyType.Name,
                    description = propertyType.Description,
                    dataType = new {
                        alias = propertyEditor.Alias,
                        view = propertyEditor.GetValueEditor().View,
                        config = GetConfig(dataType.Configuration)
                    },
                    validation = new {
                        mandatory = propertyType.Mandatory
                    }
                });

            }

            PropertyTypes = temp;

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