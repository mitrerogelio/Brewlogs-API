using brewlogsMinimalApi.Helpers;
using brewlogsMinimalApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace brewlogsMinimalApi.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly JwtService _jwtService;

    public IdentityController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        JwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] EmailRegistrationParams registrationParams)
    {
        if (!ModelState.IsValid) return BadRequest("Invalid registration data.");

        var user = new IdentityUser { UserName = registrationParams.Email, Email = registrationParams.Email };

        var result = await _userManager.CreateAsync(user, registrationParams.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);
        var token = _jwtService.GenerateJwtToken(user.Id);
        return Ok(new { Token = token });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Dictionary<string, string> model)
    {
        if (!model.TryGetValue("Email", out var email) || !model.TryGetValue("Password", out var password))
            return BadRequest("Invalid login attempt.");
        var result =
            await _signInManager.PasswordSignInAsync(email, password, isPersistent: false,
                lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return Ok("User logged in successfully.");
        }

        return BadRequest("Invalid login attempt.");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok("User logged out successfully.");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] Dictionary<string, string> model)
    {
        if (!model.TryGetValue("Email", out var email) || !model.TryGetValue("NewPassword", out var newPassword))
            return BadRequest("Password reset failed.");
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return BadRequest("Password reset failed.");
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (result.Succeeded)
        {
            return Ok("Password reset successful.");
        }

        return BadRequest("Password reset failed.");
    }
}