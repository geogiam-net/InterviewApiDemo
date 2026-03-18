using InterviewApiDemo.Endpoints;
using InterviewApiDemo.Startup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.RunDatabaseMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();
// app.UseAuthorization();

app.MapGet("/", () => "Hello InterviewApiDemo!");

UserEndpoints.MapUserEndpoints(app);

app.Run();
