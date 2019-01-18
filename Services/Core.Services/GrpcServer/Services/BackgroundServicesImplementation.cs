using Grpc.Core;
using Serilog;
using Shared.GrpcClientLibrary;
using System.Threading.Tasks;

namespace Core.Services.RPC.Services
{
    class BackgroundServicesImplementation : Shared.GrpcClientLibrary.BackgroundServices.BackgroundServicesBase
    {

        public override Task<WorkerTaskResponse> WorkerTask(WorkerTaskRequest request, ServerCallContext context)
        {
            Log.Information("Worker Task called via gRPC remote service {@request}", request);

            return Task.FromResult(new WorkerTaskResponse { IsSuccess = true, Message = $"The worker task with id '{ request.Id }' has been run!" });

        }

        public override Task<CustodialTaskResponse> CustodialTask(CustodialTaskRequest request, ServerCallContext context)
        {
            Log.Information("Custodial Task called via gRPC remote service {@request}", request);

            return Task.FromResult(new CustodialTaskResponse { IsSuccess = true, Message = $"The custodial task with id '{ request.Id }' has been run!" });
        }


    }
}
