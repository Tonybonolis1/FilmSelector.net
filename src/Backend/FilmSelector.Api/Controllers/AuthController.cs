using Microsoft.AspNetCore.Mvc;
using FilmSelector.Application.DTOs.Requests;
using FilmSelector.Application.Interfaces;

namespace FilmSelector.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        _logger.LogInformation("Intento de login para usuario: {Username}", request.Username);

        var result = await _authService.LoginAsync(request);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Login fallido para usuario: {Username}", request.Username);
            return Unauthorized(new { message = result.ErrorMessage });
        }

        _logger.LogInformation("Login exitoso para usuario: {Username}", request.Username);
        return Ok(result.Data);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        _logger.LogInformation("Intento de registro para usuario: {Username}", request.Username);

        var result = await _authService.RegisterAsync(request);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Registro fallido: {Error}", result.ErrorMessage);
            return BadRequest(new { message = result.ErrorMessage });
        }

        _logger.LogInformation("Registro exitoso para usuario: {Username}", request.Username);
        return CreatedAtAction(nameof(Register), result.Data);
    }
}

