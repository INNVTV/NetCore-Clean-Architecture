using Core.Application.Accounts.Models.Documents;
using Core.Application.Accounts.Models.Views;
using Core.Domain.Entities;

namespace Core.Startup
{
    internal static class AutoMapperConfiguration
    {
        internal static void Configure()
        {
            AutoMapper.Mapper.Initialize(cfg => {
                cfg.CreateMap<AccountDocumentModel, Account>();
                cfg.CreateMap<AccountDocumentModel, AccountListViewItem>();
                //cfg.CreateMap<EntityDocumentModel, Entity>();
                //cfg.CreateMap<EntityDocumentModel, Entity>();
                //cfg.CreateMap<EntityDocumentModel, Entity>();
            });
        }
    }
}
