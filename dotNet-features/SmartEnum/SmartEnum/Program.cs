
using SmartEnum;

//var creditCard = CreditCard.Platinum;

var creditCard = CreditCard.FromValue(1);
var creditCard2 = CreditCard.FromValue(2);

var creditCard_Platinum = CreditCard.FromName("Platinum");

var creditCard_Unnown = CreditCard.FromName("Unnown");

Console.WriteLine($"Discount for {creditCard} is {creditCard.Discount:P}");




return 0;