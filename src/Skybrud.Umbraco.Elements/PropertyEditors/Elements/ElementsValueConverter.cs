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
            return propertyType.EditorAlias == "Skybrud.Umbraco.Elements" || propertyType.EditorAlias == "Skybrud.Umbraco.Elements.List" || propertyType.EditorAlias == "Skybrud.Umbraco.Elements.Element" || propertyType.EditorAlias == "Skybrud.Umbraco.Elements.Multiple";
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview) {

            if (!(source is string)) return null;

            string strValue = source.ToString();

            // TODO: Get helper instance via DI
            PublishedElementHelper helper = new PublishedElementHelper();

            switch (propertyType.EditorAlias) {

                case "Skybrud.Umbraco.Elements":
                case "Skybrud.Umbraco.Elements.List":
                case "Skybrud.Umbraco.Elements.Multiple":
                    return helper.Deserialize(strValue);

                case "Skybrud.Umbraco.Elements.Element":
                    return helper.Deserialize(strValue)?.FirstOrDefault();

                default:
                    return typeof(object);

            }

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

            switch (propertyType.EditorAlias) {

                case "Skybrud.Umbraco.Elements":
                case "Skybrud.Umbraco.Elements.List":
                case "Skybrud.Umbraco.Elements.Multiple":
                    return typeof(IPublishedElement[]);

                case "Skybrud.Umbraco.Elements.Element":
                    return typeof(IPublishedElement);

                default:
                    return typeof(object);

            }

        }

    }

}