using Umbraco.Core.Composing;

namespace Skybrud.Umbraco.Elements.Converters
{
    public sealed class ElementsConverterCollectionBuilder : LazyCollectionBuilderBase<ElementsConverterCollectionBuilder, ElementsConverterCollection, IElementsConverter> {
        protected override ElementsConverterCollectionBuilder This => this;
    }
}