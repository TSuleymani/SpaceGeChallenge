using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceGeApp.Core.Exceptions
{
    [Serializable]
    public class ProviderException : ApplicationException
    {
        public ProviderException(string message) : base(message) { }
    }
}
