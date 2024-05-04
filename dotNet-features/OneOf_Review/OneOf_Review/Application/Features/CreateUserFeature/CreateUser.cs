using OneOf_Review.Application.ReturnVariants;
using OneOf_Review.Domain;
using OneOf;

namespace OneOf_Review.Application.Features.CreateUserFeature;

public static class CreateUser
{
    public static OneOf<User, InvalidName, NameTaken> Creat(string username)
    {
        if (string.IsNullOrEmpty(username))
            return new InvalidName();
        User user = null;//_repo.FindByUsername(username);
        if(user != null)
            return new NameTaken();
        
        user = new User(username);
        //_repo.Save(user);
        return user;
    }
}