using Core.Domain.Entities;
using MediatR;

namespace Core.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<Account>
    {
        public string Name { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
