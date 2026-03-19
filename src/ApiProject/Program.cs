using Demo.Api.Endpoints;
using Demo.Api.Startup;
using Demo.Infrastructure.RabbitMQ;
using Demo.Infrastructure.SqlStorage;
using Demo.Domain.Interfaces;
using Demo.UnitTests.Mocks;
using Demo.Api.Exceptions;

var useMocks = false;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(TimeProvider.System);

if (useMocks) {
    builder.Services.AddSingleton<IUserRepository>(new UserRepositoryMock(new MessageBrokerMock(), TimeProvider.System));
    builder.Services.AddSingleton<IMessageBroker>(new MessageBrokerMock());
}
else {
    builder.Services.AddDatabase(builder.Configuration);
    builder.Services.AddRabbit(builder.Configuration);
}

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<DomainExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

if (!useMocks)
{
    app.RunDatabaseMigrations();
}

// not needed for this demo, but in production you should use https and authorization
// app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

// UseStatusCodePages enables middleware that provides default responses for HTTP status codes
// (like 404, 400, 500) when your API does not return a body
app.UseStatusCodePages();
app.UseExceptionHandler();

app.MapUserEndpoints();

// not needed for this demo, but in production you should use https and authorization
// app.UseAuthorization(); 

app.Run();