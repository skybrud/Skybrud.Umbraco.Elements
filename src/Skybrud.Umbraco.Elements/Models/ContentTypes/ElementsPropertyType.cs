using System;
using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;

namespace Skybrud.Umbraco.Elements.Models.ContentTypes {

    public class ElementsPropertyType {

        #region Properties

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("view")]
        public string View { get; set; }

        [JsonProperty("config")]
        public object Config { get; set; }

        [JsonProperty("hideLabel")]
        public bool HideLabel { get; set; }

        [JsonProperty("validation")]
        public ElementsValidation Validation { get; set; }

        [JsonProperty("readonly")]
        public bool IsReadOnly { get; set; }

        [JsonProperty("dataTypeKey")]
        public Guid DataTypeKey { get; set; }

        [JsonProperty("editor")]
        public string Editor { get; set; }

        [JsonIgnore]
        public ElementsDataType DataType { get; set; }

        #endregion

        #region Constructors

        public ElementsPropertyType() { }

        public ElementsPropertyType(PropertyType propertyType, IDataTypeService dataTypeService) : this(propertyType, dataTypeService.GetDataType(propertyType.DataTypeId)) { }

        public ElementsPropertyType(PropertyType propertyType, IDataType dataType) {

            IDataValueEditor valueEditor = dataType.Editor.GetValueEditor(dataType.Configuration);

            Alias = propertyType.Alias;
            Label = propertyType.Name;
            Description = propertyType.Description;
            View = valueEditor.View;
            Config = dataType.Configuration;
            HideLabel = valueEditor.HideLabel;
            Validation = new ElementsValidation(propertyType);
            IsReadOnly = valueEditor.IsReadOnly;
            DataTypeKey = propertyType.DataTypeKey;
            Editor = dataType.EditorAlias;

            DataType = new ElementsDataType(dataType);

        }

        #endregion

    }

}