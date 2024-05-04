using System.Globalization;
using OneOf_Review.Application.ReturnVariants;
using OneOf_Review.Domain;
using OneOf;
using OneOf_Review.Application.Features.CreateUserFeature;
using OneOf.Types;

//https://www.youtube.com/watch?v=7z-xjijYfcI
Console.WriteLine("discriminated unions for C# using OneOf nuget package");

//------------------------------------------------------------------------------------------------
// OneOf - возвращает результат с ограниченным набором возвращаемых значений. только одно значение может быть за один раз
var username = "Petya";
OneOf<User, InvalidName, NameTaken> createUserResult = CreateUser.Creat(username);

//Можем сопоставить полученный результат и вернуть новое значение.
//отлично работает с IActionResult, можно возвращать Http ответы
var res=createUserResult.Match(
    user => { return "111";},
    name => { return "222";},
    taken => { return "333";}
);


//Использование Готовых типов (структур) для маркировки возвращаемого значения-------------------
//Yes, No, Maybe, Unknown, True, False, All, Some, None.
OneOf<User, None> GetValue()
{
    return new None();
    //return new User("");
}


var res2 = GetValue();
var isT0= res2.IsT0;
var isT1= res2.IsT1;


//MapTests -------------------------------------------------------------------------------------
string ResolveString(OneOf<double, int, string> input)
    => input
        .MapT0(d => d.ToString(CultureInfo.InvariantCulture))
        .MapT1(i => i.ToString(CultureInfo.InvariantCulture))
        .Match(t1 => t1, t2 => t2, t3 => t3);

var resolvedDouble = ResolveString(2.0);
var resolveInt = ResolveString(4);
var resolveString = ResolveString("6");

Console.ReadKey();
