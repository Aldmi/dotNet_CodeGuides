using System.IO.Enumeration;

namespace Common;

public record Person(string Email, string Password, string City, string Company);

public class UsersDb
{
	public List<Person> Persons { get; set; } =
	[
		new Person("tom@gmail.com", "12345", "London", "Microsoft"),
		new Person("bob@gmail.com", "55555", "Лондон", "Google"),
		new Person("sam@gmail.com", "11111", "Berlin", "Microsoft")
	];


	public Person? GetAuthPerson(string email, string password)
	{
		return Persons.FirstOrDefault(p => p.Email == email && p.Password == password);
	}
}