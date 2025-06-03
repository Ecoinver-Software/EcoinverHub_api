﻿
using EcoinverHub_api.Models.Dto;
using EcoinverHub_api.Services;
using Microsoft.AspNetCore.Mvc;
using EcoinverHub_api.Utils;



[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var response = await _authService.AuthenticateAsync(loginDto.Username, loginDto.Password);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            var msg = "Credenciales Inválidas";
            _logger.LogError(ex, msg);
            return BadRequest(new { message = msg });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in api/auth/login");
            return StatusCode(500, new { message = ex.Message });
        }
    }
}

