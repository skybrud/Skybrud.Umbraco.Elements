using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Umbraco.Elements.Exceptions;
using Skybrud.Umbraco.Elements.Models;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Editors;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;
// ReSharper disable PossibleInvalidCastExceptionInForeachLoop

namespace Skybrud.Umbraco.Elements {

    public class PublishedElementHelper : IPublishedElementHelper {

        private readonly IContentTypeService _contentTypeService;
        private readonly IPublishedContentTypeFactory _publishedContentTypeFactory;
        private readonly ILogger _logger;
        private readonly IDataTypeService _dataTypeService;
        private readonly IPublishedModelFactory _publishedModelFactory;
        private readonly PropertyEditorCollection _propertyEditors;

        public PublishedElementHelper() {

            _contentTypeService = Current.Services.ContentTypeService;
            _publishedContentTypeFactory = Current.PublishedContentTypeFactory;
            _logger = Current.Logger;
            _dataTypeService = Current.Services.DataTypeService;
            _publishedModelFactory = Current.Factory.GetInstance<IPublishedModelFactory>();
            _propertyEditors = Current.PropertyEditors;

        }

        //public PublishedElementHelper(IContentTypeService contentTypeService,
        //    IPublishedContentTypeFactory publishedContentTypeFactory, ILogger logger, IDataTypeService dataTypeService,
        //    IPublishedModelFactory publishedModelFactory, PropertyEditorCollection propertyEditors)
        //{

        //    _contentTypeService = contentTypeService;
        //    _publishedContentTypeFactory = publishedContentTypeFactory;
        //    _logger = logger;
        //    _dataTypeService = dataTypeService;
        //    _publishedModelFactory = publishedModelFactory;
        //    _propertyEditors = propertyEditors;

        //}

        public virtual IPublishedElement[] Deserialize(string str) {

            if (string.IsNullOrWhiteSpace(str)) return new IPublishedElement[0];

            JToken token = JsonUtils.LoadJsonToken(str);

            switch (token) {

                case JArray array:
                    return ParseElements(array);

                case JObject obj:
                    return ParseElements(new JArray(obj));

                default:
                    throw new ElementsException("Unsupported token type: " + token.Type);

            }

        }

        public virtual IPublishedElement ParseElement(JObject obj) {
            if (obj == null) return null;
            return ParseElements(new JArray(obj))[0];
        }

        public virtual IPublishedElement[] ParseElements(JArray array) {

            if (array == null) return new IPublishedElement[0];

            List<IPublishedElement> items = new List<IPublishedElement>();

            foreach (JObject item in array) {

                // Get basic information from the item
                Guid key = item.GetGuid("key");
                string name = item.GetString("name");
                Guid contentTypeKey = item.GetGuid("contentType");

                // Get a reference to the content type
                IContentType contentType = _contentTypeService.Get(contentTypeKey);
                if (contentType == null) {
                    _logger.Error(typeof(PublishedElementHelper), "Content type with key " + contentTypeKey + " not found.");
                    continue;
                }

                // Convert the content type to it's published counterpart
                IPublishedContentType pct = _publishedContentTypeFactory.CreateContentType(contentType);

                List<IPublishedProperty> properties = new List<IPublishedProperty>();

                foreach (JProperty prop in item.GetObject("properties").Properties()) {

                    // Get a reference to the property type
                    IPublishedPropertyType type = pct.GetPropertyType(prop.Name);
                    if (type == null) {
                        _logger.Error<PublishedElementHelper>("Property type for property with alias {Alias} not found.", prop.Name);
                        continue;
                    }

                    // Get a reference to the property editor
                    if (_propertyEditors.TryGet(type.EditorAlias, out IDataEditor propEditor) == false) {
                        _logger.Error<PublishedElementHelper>("Property editor with alias {Alias} not found.", type.EditorAlias);
                        continue;
                    }

                   
                    #region Borrowed from Doc Type Grid Editor

                    ContentPropertyData contentPropData = new ContentPropertyData(prop.Value, type.DataType.Configuration);

                    object newValue = prop.Value == null ? null : propEditor.GetValueEditor().FromEditor(contentPropData, prop.Value);

                    PropertyType propType2;
                    try {
                        propType2 = contentType.CompositionPropertyTypes.First(x => x.PropertyEditorAlias.InvariantEquals(type.DataType.EditorAlias));
                    } catch (Exception ex) {
                        throw new ElementsException($"Unable to find property editor with alias: {type.DataType.EditorAlias} (" + type.DataType.Id + ")", ex);
                    }


                    Property prop2 = null;
                    try {
                        /* HACK: [LK:2016-04-01] When using the "Umbraco.Tags" property-editor, the converted DB value does
                             * not match the datatypes underlying db-column type. So it throws a "Type validation failed" exception.
                             * We feel that the Umbraco core isn't handling the Tags value correctly, as it should be the responsiblity
                             * of the "Umbraco.Tags" property-editor to handle the value conversion into the correct type.
                             * See: http://issues.umbraco.org/issue/U4-8279
                             */
                        prop2 = new Property(propType2);
                        prop2.SetValue(newValue);
                    } catch (Exception ex) {
                        _logger.Error(typeof(PublishedElementHelper), ex, "Error creating Property object.");
                    }

                    if (prop2 != null) {
                        string newValue2 = propEditor.GetValueEditor().ConvertDbToString(propType2, newValue, _dataTypeService);
                        properties.Add(new SkybrudPublishedProperty(type, prop.Name, newValue2));
                    }

                    #endregion

                }

                // Create the model based on our implementation of IPublishedElement
                IPublishedElement content = new SkybrudPublishedElement(key, name, pct, properties.ToArray());

                // Let the current model factory create a typed model to wrap our model
                if (_publishedModelFactory != null) content = _publishedModelFactory.CreateModel(content);

                items.Add(content);

            }

            return items.ToArray();

        }

    }

}