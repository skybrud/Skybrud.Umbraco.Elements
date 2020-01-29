using Umbraco.Core.PropertyEditors;

namespace Skybrud.Umbraco.Elements.PropertyEditors.Elements {

    public class ElementsConfiguration {

        [ConfigurationField("view", "View", "textstring")]
        public string View { get; set; }

        [ConfigurationField("allowedTypes", "Allowed element types", "/App_Plugins/Skybrud.Umbraco.Elements/Views/ContentTypes.html", Description = "Select the element types that should be allowed.")]
        public object AllowedTypes { get; set; }

        [ConfigurationField("singlePicker", "Pick a single item", "boolean")]
        public bool SinglePicker { get; set; }

        [ConfigurationField("minItems", "Min Items", "number", Description = "Minimum number of items allowed.")]
        public int MinItems { get; set; }

        [ConfigurationField("maxItems", "Max Items", "number", Description = "Maximum number of items allowed.")]
        public int MaxItems { get; set; }

        [ConfigurationField("confirmDeletes", "Confirm Deletes", "boolean", Description = "Requires editor confirmation for delete actions.")]
        public bool ConfirmDeletes { get; set; } = true;

        [ConfigurationField("hideLabel", "Hide Label", "boolean", Description = "Hide the property label and let the item list span the full width of the editor window.")]
        public bool HideLabel { get; set; }

    }

}