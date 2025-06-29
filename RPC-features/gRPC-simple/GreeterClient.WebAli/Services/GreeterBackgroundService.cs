namespace GreeterClient.WebAli.Services;

public class GreeterBackgroundService : BackgroundService
{
	private readonly LangTranslator _translator;
	private readonly ILogger<GreeterBackgroundService> _logger;

	public GreeterBackgroundService(LangTranslator translator, ILogger<GreeterBackgroundService> logger)
	{
		_translator = translator;
		_logger = logger;
	}
	
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			// Используйте _translator - не нужно пересоздавать клиент или канал
			var res= await _translator.TranslateAsync("text");
			_logger.LogInformation(res);
			await Task.Delay(1000, stoppingToken);
		}
	}
}