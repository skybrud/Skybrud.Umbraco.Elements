using System;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Skybrud.Umbraco.Elements {

    public class PublishedElementValueConverter : PropertyValueConverterBase {

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
                    if (string.IsNullOrWhiteSpace(strValue)) return new IPublishedElement[0];
                    JArray array = JsonUtils.ParseJsonArray(strValue);
                    return helper.ParseElements(array);

                case "Skybrud.Umbraco.Elements.Element":
                    if (string.IsNullOrWhiteSpace(strValue)) return null;
                    JObject obj = JsonUtils.ParseJsonObject(strValue);
                    return helper.ParseElement(obj);

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