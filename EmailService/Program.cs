using EmailService.Data;
using EmailService.Extensions;
using EmailService.Messaging;
using EmailService.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//connect to database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var dbContextBuilder = new DbContextOptionsBuilder<AppDbContext>();
dbContextBuilder.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
builder.Services.AddSingleton(new Emailservice(dbContextBuilder.Options));

//register email service
builder.Services.AddSingleton<IAzureMessageBusConsumer, AzureMessageBusConsumer>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.useAzure();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
