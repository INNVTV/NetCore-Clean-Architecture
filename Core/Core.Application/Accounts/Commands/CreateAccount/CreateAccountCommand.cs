using Core.Application.Accounts.Models;
using MediatR;

namespace Core.Application.Accounts.Commands
{
    public class CreateAccountCommand : IRequest<AccountViewModel>
    {
        public string Name { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
