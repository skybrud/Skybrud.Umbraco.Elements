using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Skybrud.Umbraco.Elements.Models.ContentTypes {

    public class SkybrudElementsType {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("key")]
        public Guid Key { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("propertyTypes")]
        public object PropertyTypes { get; set; }

        [JsonProperty("propertyGroups")]
        public IEnumerable<SkybrudElementsPropertyGroup> PropertyGroups { get; set; }

        public SkybrudElementsType(IContentType ct, ServiceContext services) {

            Id = ct.Id;
            Key = ct.Key;
            Name = ct.Name;
            Icon = ct.Icon;
            Alias = ct.Alias;

            PropertyGroups = ct
                .CompositionPropertyGroups
                .OrderBy(x => x.SortOrder)
                .GroupBy(x => x.Name)
                .Select(x => new SkybrudElementsPropertyGroup(x, services.DataTypeService));

            PropertyTypes = PropertyGroups.SelectMany(x => x.PropertyTypes);

        }

    }

}