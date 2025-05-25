namespace WebApi.Services;

public interface IRepository<T>
{
	IEnumerable<T> Get();
}