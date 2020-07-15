using System.Collections.Generic;
using System.Web.Http.Controllers;
using Skybrud.Umbraco.Elements.Models.ContentTypes;
using Umbraco.Core.Events;
using Umbraco.Web.Editors;

namespace Skybrud.Umbraco.Elements {
    
    public class ElementsEventManager {

        public static event TypedEventHandler<HttpActionContext, EditorModelEventArgs<IEnumerable<ElementsType>>> SendingContentTypesModel;

        private static void OnSendingContentTypesModel(HttpActionContext sender, EditorModelEventArgs<IEnumerable<ElementsType>> e) {
            var handler = SendingContentTypesModel;
            handler?.Invoke(sender, e);
        }

        internal static void Emit(HttpActionContext sender, EditorModelEventArgs e) {

            if (e.Model is IEnumerable<ElementsType>) {
                OnSendingContentTypesModel(sender, new EditorModelEventArgs<IEnumerable<ElementsType>>(e));
            }

        }

    }
 
}