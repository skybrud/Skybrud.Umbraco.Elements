using System;

namespace Skybrud.Umbraco.Elements.Exceptions {

    public class ElementsException : Exception {

        public ElementsException(string message) : base(message) { }

        public ElementsException(string message, Exception innerException) : base(message, innerException) { }

    }

}