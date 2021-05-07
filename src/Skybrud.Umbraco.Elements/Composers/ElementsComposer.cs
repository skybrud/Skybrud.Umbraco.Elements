using Skybrud.Umbraco.Elements.Converters;
using Skybrud.Umbraco.Elements.Grid;
using Skybrud.Umbraco.GridData;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Skybrud.Umbraco.Elements.Composers {
    
    [RuntimeLevel(MinLevel = RuntimeLevel.Upgrade)]
    public class ElementsComposer : IUserComposer {

        public void Compose(Composition composition) {
            
            //composition.Register<IPublishedElementHelper, PublishedElementHelper>();
            GridContext.Current.Converters.Add(new ElementsGridConverter());

            // Register elements converters
            composition
                .RegisterUnique<ElementsConverterCollection>();
            composition
                .WithCollectionBuilder<ElementsConverterCollectionBuilder>()
                .Add(() => composition.TypeLoader.GetTypes<IElementsConverter>());

        }

    }

}