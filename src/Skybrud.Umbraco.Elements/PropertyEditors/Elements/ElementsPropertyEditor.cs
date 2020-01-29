using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;

namespace Skybrud.Umbraco.Elements.PropertyEditors.Elements {

    [DataEditor("Skybrud.Umbraco.Elements", EditorType.PropertyValue, "Skybrud Elements", "/App_Plugins/Skybrud.Umbraco.Elements/Views/Editor.html", ValueType = ValueTypes.Json, Group = "Skybrud.dk", Icon = Constants.Icons.ContentType)]
    public class ElementsPropertyEditor : DataEditor {

        public ElementsPropertyEditor(ILogger logger) : base(logger) { }

        /// <inheritdoc />
        protected override IConfigurationEditor CreateConfigurationEditor() => new ElementsConfigurationEditor();

    }

}