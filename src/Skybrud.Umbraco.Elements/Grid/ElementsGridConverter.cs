using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.Elements.Grid.Config;
using Skybrud.Umbraco.Elements.Grid.Values;
using Skybrud.Umbraco.GridData;
using Skybrud.Umbraco.GridData.Converters;
using Skybrud.Umbraco.GridData.Interfaces;

namespace Skybrud.Umbraco.Elements.Grid {

    public class ElementsGridConverter : GridConverterBase {

        public override bool ConvertControlValue(GridControl control, JToken token, out IGridControlValue value) {

            value = null;

            switch (control.Editor.View?.Split('?')[0]) {

                case "/App_Plugins/Skybrud.Umbraco.Elements/Views/Grid.html":

                    var cfg = control.Editor.GetConfig<GridEditorElementsConfig>();

                    if (cfg == null || cfg.IsSinglePicker == false) {
                        value = new GridControlElementsValue(control);
                    } else {
                        value = new GridControlElementValue(control);
                    }

                    break;

            }

            return value != null;

        }

        public override bool ConvertEditorConfig(GridEditor editor, JToken token, out IGridEditorConfig config) {

            config = null;

            switch (editor.View?.Split('?')[0]) {

                case "/App_Plugins/Skybrud.Umbraco.Elements/Views/Grid.html":
                    config = GridEditorElementsConfig.Parse(editor, token as JObject);
                    return true;

            }

            return false;

        }

    }

}