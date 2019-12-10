using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;

namespace Skybrud.Umbraco.Elements.Models {

    public class SkybrudPublishedElement : IPublishedElement {

        public IPublishedContentType ContentType { get; }

        public Guid Key { get; }

        public string Name { get; }

        public IEnumerable<IPublishedProperty> Properties { get; }

        public SkybrudPublishedElement(Guid key, string name, IPublishedContentType contentType, IEnumerable<IPublishedProperty> properties) {
            Key = key;
            Name = name;
            ContentType = contentType;
            Properties = properties;
        }

        public IPublishedProperty GetProperty(string alias) {
            return Properties.FirstOrDefault(x => x.PropertyType.Alias.InvariantEquals(alias));
        }

    }

}
