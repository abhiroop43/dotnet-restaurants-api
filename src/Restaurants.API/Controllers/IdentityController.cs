using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    IMapper mapper,
    TokenProvider tokenProvider) : ControllerBase
{
  [HttpPatch("user")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<IActionResult> UpdateUserDetails([FromBody] UpdateUserDetailsCommand command)
  {
    await mediator.Send(command);
    return NoContent();
  }

  [HttpPost("userRole")]
  [Authorize(Roles = UserRoles.Admin, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

    var roles = await userManager.GetRolesAsync(user);
    var loginResponse = await tokenProvider.Create(user, roles);
    return Ok(loginResponse);
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterRequest request)
  {
    if (request.Password != request.ConfirmPassword)
      return BadRequest("Password and Confirm Password does not match.");

    var user = mapper.Map<User>(request);
    var result = await userManager.CreateAsync(user, request.Password);

    if (!result.Succeeded) return BadRequest(result.Errors);

    var roles = await userManager.GetRolesAsync(user);
    var loginResponse = await tokenProvider.Create(user, roles);
    return Ok(loginResponse);
  }

  [HttpPost("refresh")]
  public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
  {
    var user = await userManager.FindByEmailAsync(request.Email);
    if (user == null) return Unauthorized();

    var isValid = tokenProvider.ValidateRefreshToken(user.Id, request.RefreshToken);

    if (!isValid) return Unauthorized();

    var roles = await userManager.GetRolesAsync(user);
    var loginResponse = await tokenProvider.Create(user, roles);
    return Ok(loginResponse);
  }
}
