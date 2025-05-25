using WebApi.Domain;

namespace WebApi.Services;

public class UserService : IUserService
{
	public User GetUser(int id)
	{
		return new User
		{
			Id = id,
			FirstName = "John",
			LastName = "Doe"
		};
	}
}