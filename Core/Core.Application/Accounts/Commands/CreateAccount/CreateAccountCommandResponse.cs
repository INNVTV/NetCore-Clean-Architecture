using Core.Common.Response;
using Core.Common.Responses;
using Core.Domain.Entities;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandResponse : BaseResponse
    {
        public CreateAccountCommandResponse()
            : base()
        {

        }

        public CreateAccountCommandResponse(IList<ValidationFailure> failures)
            : base(failures)
        {

        }

        public Account Account { get; set; }
    }
}
