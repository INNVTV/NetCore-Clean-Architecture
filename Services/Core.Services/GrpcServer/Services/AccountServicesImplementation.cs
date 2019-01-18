using AutoMapper;
using Core.Application.Accounts.Commands;
using Grpc.Core;
using MediatR;
using Serilog;
using Shared.GrpcClientLibrary;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Core.Common.Response;
using Core.Application.Accounts.Queries;

namespace Core.Services.RPC.Services
{
    class AccountServicesImplementation : Shared.GrpcClientLibrary.AccountServices.AccountServicesBase
    {
        readonly IMediator _mediator;
        readonly IMapper _mapper; //<-- Instance version of IMapper. Used only in the Service layer for ServiceModels

        public AccountServicesImplementation(IServiceProvider serviceProvider, IMapper mapper)
        {
            _mediator = serviceProvider.GetService<IMediator>();
            _mapper = mapper;
        }

        public override async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest createAccountRequest, ServerCallContext context)
        {
            Log.Information("CreateAccount Grpc Service Called {@createAccountRequest}", createAccountRequest);

            //Use AutoMapper instance to transform GrpcRequest into MediatR Request (Configured in Startup)
            var createAccountCommand = _mapper.Map<CreateAccountCommand>(createAccountRequest);

            var result = await _mediator.Send(createAccountCommand);

            //Use AutoMapper instance to transform CommandResponse into GrpcResponse (Configured in Startup)
            CreateAccountResponse createAccountResponse = _mapper.Map<CreateAccountResponse>(result);

            return createAccountResponse;
        }

        public override async Task<CloseAccountResponse> CloseAccount(CloseAccountRequest closeAccountRequest, ServerCallContext context)
        {
            Log.Information("CloseAccount Grpc Service Called {@closeAccountRequest}", closeAccountRequest);

            var closeAccountCommand = _mapper.Map<CreateAccountCommand>(closeAccountRequest);
            var result = await _mediator.Send(closeAccountCommand);

            return _mapper.Map<CloseAccountResponse>(result);

        }

        public override async Task<GetAccountListResponse> GetAccountList(GetAccountListRequest getAccountListRequest, ServerCallContext context)
        {
            Log.Information("CreateAccount Grpc Service Called {@getAccountListRequest}", getAccountListRequest);

            // We don't use the GetAccountListQuery in the controller method otherwise Swagger tries to use a POST on our GET call
            var accountListQuery = _mapper.Map<GetAccountListQuery>(getAccountListRequest);
            var result = await _mediator.Send(accountListQuery);
            return _mapper.Map<GetAccountListResponse>(result);

            //-----------------------------------------------------
            // TODO: DocumentDB will soon have skip/take
            // For now we use continuation token to get next batch from list
            // For even more robust query capabilities you should use the 'search' route
            //-----------------------------------------------------

        }
    }
}
