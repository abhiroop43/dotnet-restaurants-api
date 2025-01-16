using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Application.Users.Commands.DeleteUserRole;
using Restaurants.Application.Users.Commands.UpdateUserDetails;
using Restaurants.Application.Users.Dtos;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(
    IMediator mediator,
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    TokenProvider tokenProvider) : ControllerBase
{
    [HttpPatch("user")]
    [Authorize]
    public async Task<IActionResult> UpdateUserDetails([FromBody] UpdateUserDetailsCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> AssignUserRole([FromBody] AssignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> DeleteUserRole([FromBody] DeleteUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null) return Unauthorized();

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded) return Unauthorized();

        var loginResponse = tokenProvider.Create(user);
        return Ok(loginResponse);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
            return BadRequest("Password and Confirm Password does not match.");

        var user = new User { UserName = request.Email, Email = request.Email };
        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        var loginResponse = tokenProvider.Create(user);
        return Ok(loginResponse);
    }
}