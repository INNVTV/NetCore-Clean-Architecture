using Core.Application.Account.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Account.Commands.CreateAccount
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.AccountName).NotEmpty().WithMessage("Please specify an account name");
            RuleFor(x => x.AccountName).Length(2, 40).WithMessage("Account name must be bewtween 2-40 characters in length");
            RuleFor(x => x.Count).LessThanOrEqualTo(0).WithMessage("Must have at least 1");
            RuleFor(x => x.Code).Must(BeAValidcode).WithMessage("Please enter a valid code");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Please enter a valid email address");
        }

        private bool BeAValidcode(string code)
        {
            // custom code validating logic goes here
            return true;
        }
    }
}
