using AutoMapper;
using Core.Services.RPC.Services;
using Serilog;
using Shared.GrpcClientLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services.GrpcServer
{
    public static class ServerInitializer
    {
        public static void Initialize(int port, IServiceProvider serviceProvider, IMapper mapper)
        {
            var server = new Grpc.Core.Server
            {
                Services = {
                    AccountServices.BindService(new AccountServicesImplementation(serviceProvider, mapper)),
                    BackgroundServices.BindService(new BackgroundServicesImplementation())
                },
                Ports = { new Grpc.Core.ServerPort("localhost", port, Grpc.Core.ServerCredentials.Insecure) }
            };

            server.Start();

            Log.Information($"Account gRPC service listening on port { port }");

            //server.ShutdownAsync().Wait();
        }
    }
}
