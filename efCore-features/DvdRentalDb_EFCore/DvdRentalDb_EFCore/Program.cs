using Microsoft.EntityFrameworkCore;
using Persistance.Ef;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddPersistence(builder.Configuration);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var context= scope.ServiceProvider.GetService<DvdRentalDbContext>();
    var isAvalaible= await context!.Database.CanConnectAsync();
    Console.WriteLine(isAvalaible ? "База данных доступна" : "База данных не доступна");
}

// using (var scope = app.Services.CreateScope())
// {
//     var context= scope.ServiceProvider.GetService<DvdRentalDbContext>();
//     var actors= context.Actors.Take(10).ToList();
//     var categories= context.Categories.ToList();
// }


// using (var scope = app.Services.CreateScope())
// {
//     var context= scope.ServiceProvider.GetService<DvdRentalDbContext>();
//     //var citys= context.Citys.Take(10).Include(c=>c.Country).ToList();
//     var contries= context.Countries.Take(10).Include(c=>c.Cities).ToList();
// }


using (var scope = app.Services.CreateScope())
{
    var context= scope.ServiceProvider.GetService<DvdRentalDbContext>();
    // var films = context.Films
    //     .Include(f=>f.Actors)
    //     .Include(f=>f.Language)
    //     .Include(f=>f.Categories)
    //     .Take(10)
    //     .ToList();
}


using (var scope = app.Services.CreateScope())
{
    var context= scope.ServiceProvider.GetService<DvdRentalDbContext>();
    // var addresses = context.Addresses
    //     .Include(f=>f.City)
    //     .Take(10)
    //     .ToList();
}


using (var scope = app.Services.CreateScope())
{
    var context= scope.ServiceProvider.GetService<DvdRentalDbContext>();
    // var customers = context.Customers
    //     .Include(f=>f.Address)
    //     .Take(10)
    //     .ToList();
    //
    // var stuffs = context.Staffs
    //     .Include(f=>f.Address).ThenInclude(a=>a.City)
    //     .Take(10)
    //     .ToList();
}


using (var scope = app.Services.CreateScope())
{
    var context= scope.ServiceProvider.GetService<DvdRentalDbContext>();
    // var inventories = context.Inventories
    //     .Include(f=>f.Film)
    //     .Take(10)
    //     .ToList();
    //
    // var stores = context.Stores
    //     .Include(f=>f.Address)
    //     .Include(f=>f.ManagerStaff)
    //     .Take(10)
    //     .ToList();
    
    var rentals = context.Rentals
        .Include(f=>f.Customer)
        .Include(f=>f.Staff)
        .Include(f=>f.Inventory).ThenInclude(inventory=>inventory.Film)
        .Take(10)
        .ToList();
    
    var payments = context.Payments
        .Include(f=>f.Customer)
        .Include(f=>f.Staff)
        .Include(f=>f.Rental).ThenInclude(rental=>rental.Customer)
        .ToList();
}

app.Run();