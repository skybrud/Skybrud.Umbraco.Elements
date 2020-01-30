using System;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Skybrud.Umbraco.Elements.PropertyEditors.Elements {

    public class ElementsValueConverter : PropertyValueConverterBase {

        //private readonly IPublishedElementHelper _helper;

        //public ImagePickerPropertyValueConverter(IPublishedElementHelper helper) {
        //    _helper = helper;
        //}

        public override bool IsConverter(IPublishedPropertyType propertyType) {
            return propertyType.EditorAlias == "Skybrud.Umbraco.Elements";
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview) {

            if (!(source is string)) return null;

            string strValue = source.ToString();

            // TODO: Get helper instance via DI
            PublishedElementHelper helper = new PublishedElementHelper();

            var cfg = propertyType.DataType.ConfigurationAs<ElementsConfiguration>();

            var value = helper.Deserialize(strValue);

            return cfg.SinglePicker ? value.FirstOrDefault() : (object) value;

        }
        
        public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview) {
            return inter;
        }

        public override object ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview) {
            return null;
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
            return PropertyCacheLevel.Snapshot;
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {
            var cfg = propertyType.DataType.ConfigurationAs<ElementsConfiguration>();
            return cfg.SinglePicker ? typeof(IPublishedElement) : typeof(IPublishedElement[]);
        }

    }

}