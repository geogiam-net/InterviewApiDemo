using Demo.Api.Dtos;
using Demo.Domain.Interfaces;

namespace Demo.Api.Endpoints;

internal static class UserEndpoints
{
    internal static void MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/api/users", 
            async Task<IResult> (UserDto user, IUserRepository userRepository) =>
          {
              await userRepository.AddUserAsync(user.Username, user.Name, user.DateOfBirth);

              // return 201 with link to created resource, because there is nothing new to return
              return TypedResults.Created(
                uri: $"/api/users/{user.Username}",
                value: user);
          });

        builder.MapGet("/api/users/{username}",
            async Task<IResult> (string username, IUserRepository userRepository) =>
          {
              var user = await userRepository.GetUserAsync(username);
              if (user is null)
              {
                  return TypedResults.NotFound();
              }

              return TypedResults.Ok(new UserDto(user));
          });
    }
}