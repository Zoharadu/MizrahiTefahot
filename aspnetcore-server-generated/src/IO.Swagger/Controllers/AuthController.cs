using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common;
using System;

namespace IO.Swagger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Authenticates the user and returns a JWT token if credentials are valid
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Simple hardcoded credentials check
            if (request.Username == "zohara" && request.Password == "zd")
            {
                // Define user claims 
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username),
                    new Claim(ClaimTypes.Role, "Admin"),
                };

                // Create signing key using symmetric security key
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-very-strong-secret-key-128-bits-long!"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Generate JWT token with claims and expiration time
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: creds
                );

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                // Return the generated token
                return Ok(new { token = jwt });
            }

            // Return 401 Unauthorized if credentials are invalid
            return Unauthorized("Invalid credentials");
        }
    }
}
