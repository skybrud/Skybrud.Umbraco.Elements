using Skybrud.Umbraco.Elements.Grid;
using Skybrud.Umbraco.GridData;
using Umbraco.Core.Composing;

namespace Skybrud.Umbraco.Elements.Composers {

    public class ElementsComposer : IUserComposer {

        public void Compose(Composition composition) {
            //composition.Register<IPublishedElementHelper, PublishedElementHelper>();
            GridContext.Current.Converters.Add(new ElementsGridConverter());
        }

    }

}