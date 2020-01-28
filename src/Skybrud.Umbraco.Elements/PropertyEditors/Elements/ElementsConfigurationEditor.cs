using System.Collections.Generic;
using Umbraco.Core.PropertyEditors;

namespace Skybrud.Umbraco.Elements.PropertyEditors.Elements {

    public class ElementsConfigurationEditor : ConfigurationEditor<ElementsConfiguration> {

        public ElementsConfigurationEditor() {
            Field(nameof(ElementsConfiguration.View)).Config = new Dictionary<string, object> {
                { "view", "/App_Plugins/Skybrud.Umbraco.Elements/Views/Partials/Multiple/Default.html" }
            };
        }

    }

}