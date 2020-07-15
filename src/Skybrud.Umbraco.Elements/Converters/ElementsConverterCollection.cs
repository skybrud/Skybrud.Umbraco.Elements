using System;
using System.Collections.Generic;
using Umbraco.Core.Composing;

// ReSharper disable AssignNullToNotNullAttribute

namespace Skybrud.Umbraco.Elements.Converters {
    
    public sealed class ElementsConverterCollection : BuilderCollectionBase<IElementsConverter> {
        
        private readonly Dictionary<string, IElementsConverter> _lookup;

        public ElementsConverterCollection(IEnumerable<IElementsConverter> items) : base(items) {
            
            _lookup = new Dictionary<string, IElementsConverter>(StringComparer.OrdinalIgnoreCase);

            foreach (IElementsConverter item in this) {
                string typeName = item.GetType().AssemblyQualifiedName;
                if (_lookup.ContainsKey(typeName) == false)  {
                    _lookup.Add(typeName, item);
                }
            }

        }

        public bool TryGet(string typeName, out IElementsConverter item) {
            return _lookup.TryGetValue(typeName, out item);
        }

    }

}