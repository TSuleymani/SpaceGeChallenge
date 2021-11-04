using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SpaceGeApp.Core.Common.Models;

namespace SpaceGeApp.Core.Validators
{
   public class WatchlistItemsValidator : AbstractValidator<WatchlistItemsRequestModel>
    {
        public WatchlistItemsValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
