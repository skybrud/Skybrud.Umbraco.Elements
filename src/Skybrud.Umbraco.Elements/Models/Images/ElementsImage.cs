using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Extensions;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Skybrud.Umbraco.Elements.Models.Images {

    public class ElementsImage {

        [JsonProperty("id")]
        public int Id { get; }

        [JsonProperty("key")]
        public Guid Key { get; }

        [JsonProperty("udi")]
        public Udi Udi { get; }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; }

        [JsonProperty("cropUrl")]
        public string CropUrl => Thumbnail;

        [JsonProperty("trashed")]
        public bool IsTrashed { get; }

        public ElementsImage(IPublishedContent media, int width, int height) {
            Id = media.Id;
            Key = media.Key;
            Udi = Udi.Create("media", media.Key);
            Name = media.Name;
            Thumbnail = media.GetCropUrl(width, height, preferFocalPoint: true);
        }

        public ElementsImage(IMedia media, int width, int height) {

            Id = media.Id;
            Key = media.Key;
            Udi = Udi.Create("media", media.Key);
            Name = media.Name;
            IsTrashed = media.Path.StartsWith("-1,-21,");

            string umbracoFile = media.GetValue<string>("umbracoFile");
            
            double left = 0;
            double top = 0;

            if (umbracoFile != null && umbracoFile[0] == '{') {
                JObject json = JObject.Parse(umbracoFile);
                left = json.GetDouble("focalPoint.left");
                top = json.GetDouble("focalPoint.top");
                umbracoFile = json.GetString("src");
            }

            Thumbnail = GetCropUrl(umbracoFile, left, top, width, height, media.UpdateDate);

        }

        private string GetCropUrl(string url, double left, double top, int width, int height, DateTime updateDate) {

            List<string> temp = new List<string>();

            if (Math.Abs(left) < double.Epsilon && Math.Abs(top) < double.Epsilon) {
                temp.Add("anchor=center");
            } else {
                temp.Add(string.Format(CultureInfo.InvariantCulture, "center={0},{1}", left, top));
            }

            temp.Add("mode=crop");
            temp.Add("width=" + width);
            temp.Add("height=" + height);
            temp.Add("rnd=" + updateDate.ToFileTimeUtc().ToString(CultureInfo.InvariantCulture));

            return url + "?" + string.Join("&", temp);

        }

    }

}