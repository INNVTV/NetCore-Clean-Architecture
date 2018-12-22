using MediatR;

namespace Core.Application.Account.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest
    {
        public string AccountName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
