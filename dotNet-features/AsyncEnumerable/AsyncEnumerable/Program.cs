// See https://aka.ms/new-console-template for more information

using AsyncEnumerable;

Console.WriteLine("Hello, World!");

var customerService= new GetCustomerService();
//await customerService.HandleCustomers_Once();

//await customerService.HandleCustomers_ByOne();

await customerService.HandleCustomers_Parallel();


Console.ReadLine();
