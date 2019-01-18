# gRPC Service Definition (Shared Client Library)
The shared client library includes the .proto files that define the gRPC services and request/response messages.

## Generating the Protocol Buffer
Protobuffer generation is built into the Visual Studio when you use the following libraries:

    Google.Protobuf
    Grpc
    Grpc.Tools

Set your .proto file properties to use the following Build Action:

![grpc-1](https://github.com/INNVTV/NetCore-Clean-Architecture/blob/master/_docs/imgs/grpc-1.png)

Once you add ths project to either the client or server solutions as a dependancy you will be able to use the service definition like you would any C# class.

**Note:** On a production project you will want to version control this library and include it as a package via Nuget, a compiled DLL or other such mechanism