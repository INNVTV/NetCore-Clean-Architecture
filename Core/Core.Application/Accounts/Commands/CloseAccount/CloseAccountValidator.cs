using Core.Application.Accounts.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Commands
{
    public class CloseAccountValidator : AbstractValidator<CloseAccountCommand>
    {
        public CloseAccountValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Please specify an id");
        }
    }
}
