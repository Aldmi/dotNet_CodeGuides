namespace WebApi.Services;

public class RepositoryLoggerDecorator<T> : IRepository<T>
{
	private readonly IRepository<T> _decoratedRepository;
	public RepositoryLoggerDecorator(IRepository<T> decoratedRepository)
	{
		Console.WriteLine("RepositoryLoggerDecorator");
		_decoratedRepository = decoratedRepository;
	}
	public IEnumerable<T> Get()
	{
		return _decoratedRepository.Get();
	}
}