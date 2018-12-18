using Core.Common.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.Common.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Core.Application.Account.Models;

namespace Core.Application.Account.Commands
{
    public class CreateAccountCommand : IRequest<AccountViewModel>
    {
        public string AccountName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
