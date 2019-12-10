using System;
using Newtonsoft.Json;
using Skybrud.Umbraco.Elements.Grid.Values;

namespace Skybrud.Umbraco.Elements.Json.Converters {

    public class PublishedElementJsonConverter : JsonConverter {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {

            if (value is GridControlPublishedElementValue element && element.Element != null) {
                serializer.Serialize(writer, element.Element);
                return;
            }

            if (value is GridControlPublishedElementsValue elements && elements.Elements != null && elements.Elements.Length > 0) {
                serializer.Serialize(writer, elements.Elements);
                return;
            }

            writer.WriteNull();

        }

        public override object ReadJson(JsonReader reader, Type type, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type type) {
            return false;
        }

    }

}