using System;
using System.Collections.Generic;
using System.Linq;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Umbraco.Elements.Converters;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Skybrud.Umbraco.Elements.PropertyEditors.Elements {

    public class ElementsValueConverter : PropertyValueConverterBase {

        private readonly ElementsConverterCollection _converters;

        //private readonly IPublishedElementHelper _helper;

        //public ImagePickerPropertyValueConverter(IPublishedElementHelper helper) {
        //    _helper = helper;
        //}

        #region Constructors

        public ElementsValueConverter(ElementsConverterCollection converters) {
            _converters = converters;
        }

        #endregion

        #region Member methods

        public override bool IsConverter(IPublishedPropertyType propertyType) {
            return propertyType.EditorAlias == "Skybrud.Umbraco.Elements";
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview) {
            return source;
        }
        
        public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview) {

            IEnumerable<IPublishedElement> value;

            if (inter is string) {
                
                // TODO: Get helper instance via DI
                PublishedElementHelper helper = new PublishedElementHelper();

                // Deserialize the JSON into instances of IPublishedElement
                value = helper.Deserialize(inter.ToString());

            } else {

                // Make sure to return an empty array if the property value is empty
                value = new IPublishedElement[0];

            }

            // Get the data type configuration
            ElementsConfiguration config = propertyType.DataType.ConfigurationAs<ElementsConfiguration>();

            // Get the key referencing the converter (if one is selected)
            string key = config.Converter?.GetString("key");

            // Get the value of the converter if one has been selected
            if (string.IsNullOrWhiteSpace(key) == false && _converters.TryGet(key, out IElementsConverter converter)) {
                return converter.Convert(owner, propertyType, value, config);
            }

            // Fallback
            return config.SinglePicker ? value.FirstOrDefault() : (object) value;

        }

        public override object ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview) {
            return null;
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
            return PropertyCacheLevel.Snapshot;
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

            // Get the data type configuration
            ElementsConfiguration config = propertyType.DataType.ConfigurationAs<ElementsConfiguration>();

            // Get the key referencing the converter (if one is selected)
            string key = config.Converter?.GetString("key");

            // Get the value of the converter if one has been selected
            if (string.IsNullOrWhiteSpace(key) == false && _converters.TryGet(key, out IElementsConverter converter)) {
                return converter.GetType(propertyType, config);
            }

            // Fallback to IPublishedElement or IEnumerable<IPublishedElement>
            return config.SinglePicker ? typeof(IPublishedElement) : typeof(IEnumerable<IPublishedElement>);

        }

        #endregion

    }

}