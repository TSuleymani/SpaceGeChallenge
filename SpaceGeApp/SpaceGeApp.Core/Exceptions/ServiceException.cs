using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceGeApp.Core.Exceptions
{
    public enum ExceptionType
    {
        Unknown = 1000,
        Internal,
        External
    }

    [Serializable]
    public class ServiceException : ApplicationException
    {
        public ExceptionType ExceptionType { get; }
        public IEnumerable<string> ErrorMessages { get; }

        public ServiceException(ExceptionType exceptionType, IEnumerable<string> errorMessages)
        {
            this.ExceptionType = exceptionType;
            this.ErrorMessages = errorMessages;
        }
    }
}
