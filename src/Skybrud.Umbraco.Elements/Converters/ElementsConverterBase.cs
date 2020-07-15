using System;
using System.Collections.Generic;
using System.Linq;
using Skybrud.Umbraco.Elements.PropertyEditors.Elements;
using Umbraco.Core.Models.PublishedContent;

namespace Skybrud.Umbraco.Elements.Converters {
    
    public abstract class ElementsConverterBase : IElementsConverter {

        public virtual string Name => GetType().Name;

        public virtual string Desctiption => null;

        public virtual object Convert(IPublishedElement owner, IPublishedPropertyType propertyType, IEnumerable<IPublishedElement> elements, ElementsConfiguration config) {
            return elements;
        }

        public virtual Type GetType(IPublishedPropertyType propertyType, ElementsConfiguration config) {
            return config.SinglePicker ? typeof(IPublishedElement) : typeof(IEnumerable<IPublishedElement>);
        }

    }
    
    public abstract class ElementsConverterBase<T> : ElementsConverterBase {

        public override object Convert(IPublishedElement owner, IPublishedPropertyType propertyType, IEnumerable<IPublishedElement> elements, ElementsConfiguration config) {
            return elements.OfType<T>();
        }

        public override Type GetType(IPublishedPropertyType propertyType, ElementsConfiguration config) {
            return config.SinglePicker ? typeof(T) : typeof(IEnumerable<T>);
        }

    }

}