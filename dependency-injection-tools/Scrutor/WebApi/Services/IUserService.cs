using WebApi.Domain;

namespace WebApi.Services;

public interface IUserService
{
	User GetUser(int id);
}