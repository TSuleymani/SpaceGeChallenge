using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation.Results;

namespace SpaceGeApp.Core.Extensions
{
   public static class ValidationExtensions
    {
        public static IEnumerable<KeyValuePair<string,string>> ToPairedErrors(this ValidationResult validationResult)
        {
            var errors = new List<KeyValuePair<string, string>>();
            foreach (var error in validationResult.Errors)
            {
                errors.Add(new KeyValuePair<string, string>(error.PropertyName, error.ErrorMessage));
            }
            return errors;
        }
    }
}
