using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceGeApp.Core.Exceptions
{
    [Serializable]
    public class ValidationException : ApplicationException
    {
        public IEnumerable<KeyValuePair<string, string>> ErrorMessages { get; }

        public ValidationException(IEnumerable<KeyValuePair<string, string>> errorMessages)
        {
            this.ErrorMessages = errorMessages;
        }
    }
}
