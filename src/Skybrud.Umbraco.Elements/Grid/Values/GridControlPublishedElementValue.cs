﻿using Newtonsoft.Json;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Umbraco.Elements.Json.Converters;
using Skybrud.Umbraco.GridData;
using Skybrud.Umbraco.GridData.Interfaces;
using Umbraco.Core.Models.PublishedContent;

namespace Skybrud.Umbraco.Elements.Grid.Values {

    [JsonConverter(typeof(PublishedElementJsonConverter))]
    public class GridControlPublishedElementValue : IGridControlValue {

        #region Properties

        public GridControl Control { get; }

        public bool IsValid { get; }

        public IPublishedElement Element { get; }

        #endregion

        #region Constructors

        public GridControlPublishedElementValue(GridControl control) {
            Control = control;
            Element = new PublishedElementHelper().ParseElement(control.JObject.GetObject("value"));
            IsValid = Element != null;
        }

        #endregion

        #region Member methods

        public string GetSearchableText() {
            return string.Empty;
        }

        #endregion

    }

}