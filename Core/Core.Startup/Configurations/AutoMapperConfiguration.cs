using Core.Application.Accounts.Models;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Startup
{
    internal static class AutoMapperConfiguration
    {
        internal static void Configure()
        {
            AutoMapper.Mapper.Initialize(cfg => {
                cfg.CreateMap<AccountDocumentModel, Account>();
                cfg.CreateMap<AccountDocumentModel, AccountListItem>();
                //cfg.CreateMap<EntityDocumentModel, Entity>();
                //cfg.CreateMap<EntityDocumentModel, Entity>();
                //cfg.CreateMap<EntityDocumentModel, Entity>();
            });
        }
    }
}
