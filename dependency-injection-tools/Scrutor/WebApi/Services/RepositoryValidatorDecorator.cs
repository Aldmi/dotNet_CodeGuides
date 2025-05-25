namespace WebApi.Services;

public class RepositoryValidatorDecorator<T> : IRepository<T>
{
	private readonly IRepository<T> _decoratedRepository;
	public RepositoryValidatorDecorator(IRepository<T> decoratedRepository)
	{
		Console.WriteLine("RepositoryValidatorDecorator");
		_decoratedRepository = decoratedRepository;
	}
	public IEnumerable<T> Get()
	{
		return _decoratedRepository.Get();
	}
}