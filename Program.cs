using Jwt.Services.Password;
using Jwt.Services.RabbitMq;
using Jwt.Services.Token;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPasswordManager, PasswordManager>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();

builder.Services.AddSingleton<IMessagePublisher, RabbitMQService>();

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

app.Run();
