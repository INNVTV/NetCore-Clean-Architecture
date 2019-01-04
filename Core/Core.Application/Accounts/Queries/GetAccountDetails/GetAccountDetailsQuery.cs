using Core.Application.Accounts.Models.Views;
using MediatR;

namespace Core.Application.Accounts.Queries
{
    public class GetAccountDetailsQuery : IRequest<AccountDetailsViewModel>
    {
        public string NameKey { get; set; }
    }
}
