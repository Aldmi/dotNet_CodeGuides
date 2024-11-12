using Grpc.Core;
using GreeterServiceApp;

namespace GreeterServiceApp.Services;

public class GreeterService(ILogger<GreeterService> _logger) : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }

    public override Task<SumReply> CalcSum(SumRequest request, ServerCallContext context)
    {
        if (request is {Val1: 0, Val2: 0})
        {
            throw new ArithmeticException("Sum Error");
        }
        var sumResult=request.Val1 + request.Val2;
        return Task.FromResult(new SumReply{Sum=sumResult});
    }
}