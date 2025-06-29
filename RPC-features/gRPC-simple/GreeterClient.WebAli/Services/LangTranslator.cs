using GreeterClientApp;
using Grpc.Core;

namespace GreeterClient.WebAli.Services;

public class LangTranslator
{
	private readonly Greeter.GreeterClient _client;
    
	public LangTranslator(Greeter.GreeterClient client)
	{
		_client = client;
	}
    
	public async Task<string> TranslateAsync(string text)
	{
		try
		{
			var reply = await _client.SayHelloAsync(new HelloRequest { Name = text });
			return reply.Message;
		}
		catch (RpcException ex)
		{
			return "RpcException";
		}
		catch (Exception e)
		{
			return "Exception";
		}
	}
}