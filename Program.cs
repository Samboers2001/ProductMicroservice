using ProductMicroservice.AsyncDataServices.Implementations;
using ProductMicroservice.AsyncDataServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProductMicroservice.Data;
using ProductMicroservice.Data.Interfaces;
using ProductMicroservice.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;


// Add services to the container.
Console.WriteLine("Using MariaDB");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(configuration.GetConnectionString("ProductConn"), new MySqlServerVersion(new Version(11, 1, 2))));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
