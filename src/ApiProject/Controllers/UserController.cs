using InterviewApiDemo.Models;
using InterviewApiDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterviewApiDemo.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly UsersService _usersService;
    private readonly RabbitService _rabbitService;

    public UserController(UsersService usersService, RabbitService rabbitService)
    {
        _usersService = usersService;
        _rabbitService = rabbitService;
    }

    // dateOfBirth must be in ISO 8601 date format (e.g. "2000-12-31")
    [HttpPost(ApiRoutes.CreateUser)]
    public async Task<ActionResult> Create(User user)
    {
        // validation errors are thrown before entering the method and have Microsoft Error format

        try
        {
            await _usersService.AddUserAsync(user);

            await _rabbitService.SendUserCreatedAsync(user);

            // return 201 with link to created resource, because there is nothing new to return
            // https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-10.0&tabs=visual-studio#update-the-posttodoitem-create-method
            return CreatedAtAction(nameof(Get), new { username = user.Username }, user);
        }
        catch (DbUpdateException)
        {
            // return error in Microsoft Error format
            // https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.validationproblemdetails?view=aspnetcore-10.0

            var error = new ValidationProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.10",
                Title = $"{user.Username} already exists.",
                Status = 409
            };
            error.Errors.Add("Username", [error.Title]);

            return StatusCode(409, error);
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpGet(ApiRoutes.GetUser)]
    public async Task<ActionResult<User?>> Get(string username)
    {
        try
        {
            var user = await _usersService.GetUserAsync(username);
            if (user is null)
            {
                return NotFound();
            }

            return user;
        }
        catch
        {
            return StatusCode(500);
        }
    }
}