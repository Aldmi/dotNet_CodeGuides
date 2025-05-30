using Infrastructure.Persistence.Pg;

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
   var context= scope.ServiceProvider.GetService<ApplicationDbContext>();
   var isAvalaible= await context!.Database.CanConnectAsync();
   Console.WriteLine(isAvalaible ? "База данных доступна" : "База данных не доступна");
}


app.Run();