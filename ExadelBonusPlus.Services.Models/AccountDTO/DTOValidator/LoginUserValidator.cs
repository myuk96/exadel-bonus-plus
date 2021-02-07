﻿using ExadelBonusPlus.WebApi.ViewModel;
using FluentValidation;

namespace ExadelBonusPlus.Services.Models.ViewModel
{
    class LoginUserValidator : AbstractValidator<LoginUserDTO>
    {
        public LoginUserValidator()
        {
            RuleFor(model => model.Email).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Please enter your email")
                .EmailAddress().WithMessage("Check your email");
        }

    }
}
