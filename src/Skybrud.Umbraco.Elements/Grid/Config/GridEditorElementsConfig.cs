using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Umbraco.GridData;
using Skybrud.Umbraco.GridData.Config;

namespace Skybrud.Umbraco.Elements.Grid.Config  {

    public class GridEditorElementsConfig : GridEditorConfigBase {

        #region Properties

        public bool IsSinglePicker { get; }

        #endregion

        #region Constructors

        private GridEditorElementsConfig(GridEditor editor, JObject obj) : base(editor, obj) {
            IsSinglePicker = obj.GetBoolean("singlePicker");
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Gets an instance of <see cref="GridEditorElementsConfig"/> from the specified <paramref name="obj"/>.
        /// </summary>
        /// <param name="editor">The parent editor.</param>
        /// <param name="obj">The instance of <see cref="JObject"/> to be parsed.</param>
        public static GridEditorElementsConfig Parse(GridEditor editor, JObject obj) {
            return obj == null ? null : new GridEditorElementsConfig(editor, obj);
        }

        #endregion

    }

}