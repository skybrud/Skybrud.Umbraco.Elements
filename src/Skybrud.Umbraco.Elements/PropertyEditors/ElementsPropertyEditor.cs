using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;

namespace Skybrud.Umbraco.Elements.PropertyEditors {

    [DataEditor("Skybrud.Umbraco.Elements", EditorType.PropertyValue, "Skybrud Elements", "/App_Plugins/Skybrud.Umbraco.Elements/Views/Editors/Multiple.html", ValueType = ValueTypes.Json, Group = "Skybrud.dk", Icon = Constants.Icons.ContentType)]
    public class ElementsPropertyEditor : DataEditor {

        public ElementsPropertyEditor(ILogger logger) : base(logger) { }

        /// <inheritdoc />
        protected override IConfigurationEditor CreateConfigurationEditor() => new ElementsConfigurationEditor();

    }

    public class ElementsConfigurationEditor : ConfigurationEditor<ElementsConfiguration> {

        public ElementsConfigurationEditor() {
            Field(nameof(ElementsConfiguration.View)).Config = new Dictionary<string, object> {
                { "view", "/App_Plugins/Skybrud.Umbraco.Elements/Views/Partials/Multiple/Default.html" }
            };
        }

    }

    public class ElementsConfiguration {

        [ConfigurationField("view", "View", "textstring")]
        public string View { get; set; }

        [ConfigurationField("allowedTypes", "Allowed element types", "/App_Plugins/Skybrud.Umbraco.Elements/Views/ContentTypes.html", Description = "Select the element types should be allowed.")]
        public string[] AllowedTypes { get; set; }

    }

}