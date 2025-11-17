using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MarineTraffic.Application.DTOs.Requests;
using MarineTraffic.Application.Interfaces;

namespace MarineTraffic.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FavoritesController : ControllerBase
{
    private readonly IFavoriteService _favoriteService;
    private readonly ILogger<FavoritesController> _logger;

    public FavoritesController(IFavoriteService favoriteService, ILogger<FavoritesController> logger)
    {
        _favoriteService = favoriteService;
        _logger = logger;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim ?? "0");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();
        _logger.LogInformation("Obteniendo favoritos para usuario: {UserId}", userId);

        var result = await _favoriteService.GetAllAsync(userId);

        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetUserId();
        _logger.LogInformation("Obteniendo favorito {Id} para usuario: {UserId}", id, userId);

        var result = await _favoriteService.GetByIdAsync(id, userId);

        if (!result.IsSuccess)
        {
            return NotFound(new { message = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFavoriteRequestDto request)
    {
        var userId = GetUserId();
        _logger.LogInformation("Creando favorito para usuario: {UserId}", userId);

        var result = await _favoriteService.CreateAsync(request, userId);

        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.ErrorMessage });
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFavoriteRequestDto request)
    {
        var userId = GetUserId();
        _logger.LogInformation("Actualizando favorito {Id} para usuario: {UserId}", id, userId);

        var result = await _favoriteService.UpdateAsync(id, request, userId);

        if (!result.IsSuccess)
        {
            return NotFound(new { message = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        _logger.LogInformation("Eliminando favorito {Id} para usuario: {UserId}", id, userId);

        var result = await _favoriteService.DeleteAsync(id, userId);

        if (!result.IsSuccess)
        {
            return NotFound(new { message = result.ErrorMessage });
        }

        return NoContent();
    }
}
