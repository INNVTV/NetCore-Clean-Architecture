using Core.Application.Accounts.Models;
using MediatR;

namespace Core.Application.Accounts.Queries
{
    public class GetAccountDetailsQuery : IRequest<AccountViewModel>
    {
        public string NameKey { get; set; }
    }
}
