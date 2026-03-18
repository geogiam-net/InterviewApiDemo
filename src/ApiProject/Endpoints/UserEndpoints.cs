using InterviewApiDemo.Models;
using InterviewApiDemo.Services;
using Microsoft.EntityFrameworkCore;

namespace InterviewApiDemo.Endpoints;

public static class UserEndpoints
{
    internal static void MapUserEndpoints(WebApplication app)
    {
        app.MapPost(pattern: "/api/users", handler:
          async Task<IResult> (User user, UsersService usersService, RabbitService rabbitService) =>
          {
              try
              {






                  await usersService.AddUserAsync(user);

                  await rabbitService.SendUserCreatedAsync(user);

                  // return 201 with link to created resource, because there is nothing new to return
                  return TypedResults.Created(
                    uri: $"/api/users/{user.Username}",
                    value: user);
              }
              catch (DbUpdateException)
              {
                  //// return error in Microsoft Error format
                  //// https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.validationproblemdetails?view=aspnetcore-10.0

                  //var error = new ValidationProblemDetails
                  //{
                  //    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.10",
                  //    Title = $"{user.Username} already exists.",
                  //    Status = 409
                  //};
                  //error.Errors.Add("Username", [error.Title]);

                  //// return StatusCode(409, error);

                  // error 409 would be better
                  return TypedResults.BadRequest($"{user.Username} already exists.");
              }
              catch
              {
                  return TypedResults.InternalServerError();
              }
          });

        app.MapGet(pattern: "/api/users/{username}", handler:
          async Task<IResult> (string username, UsersService usersService) =>
          {
              try
              {
                  var user = await usersService.GetUserAsync(username);
                  if (user is null)
                  {
                      return TypedResults.NotFound();
                  }

                  return TypedResults.Ok(user);
              }
              catch
              {
                  return TypedResults.InternalServerError();
              }
          });
    }

}