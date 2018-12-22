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
