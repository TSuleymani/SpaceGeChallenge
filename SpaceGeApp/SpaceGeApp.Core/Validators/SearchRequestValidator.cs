using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SpaceGeApp.Core.Common.Models;

namespace SpaceGeApp.Core.Validators
{
   public class SearchRequestValidator : AbstractValidator<SearchRequestModel>
    {
        public SearchRequestValidator()
        {
            RuleFor(x => x.Expression)
                 .NotNull()
                 .NotEmpty();
        }
    }
}
