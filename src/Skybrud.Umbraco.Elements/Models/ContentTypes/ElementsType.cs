using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Skybrud.Umbraco.Elements.Models.ContentTypes {

    public class ElementsType {

        #region Properties

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

        [JsonProperty("propertyGroups")]
        public IEnumerable<ElementsPropertyGroup> PropertyGroups { get; set; }

        #endregion

        #region Constructors
        
        public ElementsType() { }

        public ElementsType(IContentType ct, ServiceContext services) {

            Id = ct.Id;
            Key = ct.Key;
            Name = ct.Name;
            Icon = ct.Icon;
            Alias = ct.Alias;

            PropertyGroups = ct
                .CompositionPropertyGroups
                .OrderBy(x => x.SortOrder)
                .GroupBy(x => x.Name)
                .Select(x => new ElementsPropertyGroup(x, services.DataTypeService));

        }

        #endregion

    }

}