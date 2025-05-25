using WebApi.Domain;

namespace WebApi.Services;

public class UserRepository : IRepository<User>
{
	public UserRepository()
	{
		Console.WriteLine("UserRepository");
	}
	
	private readonly List<User> _users = new()
	{
		new User
		{
			Id = 1,
			FirstName = "John",
			LastName = "Doe"
		},
		new User
		{
			Id = 2,
			FirstName = "Janine",
			LastName = "Doe"
		}
	};
	public IEnumerable<User> Get()
	{
		return _users;
	}
}