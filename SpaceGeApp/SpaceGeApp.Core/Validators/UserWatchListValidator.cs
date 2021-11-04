using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SpaceGeApp.Core.Common.Models;

namespace SpaceGeApp.Core.Validators
{
   public class UserWatchListValidator : AbstractValidator<UserWatchlistModel>
    {
        public UserWatchListValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.MovieId).NotNull().NotEmpty();
        }
    }
}
