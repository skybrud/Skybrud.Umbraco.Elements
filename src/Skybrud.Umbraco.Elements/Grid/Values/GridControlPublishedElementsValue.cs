using Newtonsoft.Json;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Umbraco.Elements.Json.Converters;
using Skybrud.Umbraco.GridData;
using Skybrud.Umbraco.GridData.Interfaces;
using Umbraco.Core.Models.PublishedContent;

namespace Skybrud.Umbraco.Elements.Grid.Values {

    [JsonConverter(typeof(PublishedElementJsonConverter))]
    public class GridControlPublishedElementsValue : IGridControlValue {
        
        #region Properties

        public GridControl Control { get; }

        public bool IsValid { get; }

        public IPublishedElement[] Elements { get; }

        #endregion

        #region Constructors

        public GridControlPublishedElementsValue(GridControl control) {
            Control = control;
            Elements = new PublishedElementHelper().ParseElements(control.JObject.GetArray("value"));
            IsValid = control.IsValid;
        }

        #endregion

        #region Member methods

        public string GetSearchableText() {
            return string.Empty;
        }

        #endregion

    }

}