using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginRequest = Common.LoginRequest;

namespace MizrahiTefahot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        private readonly ICalculationService _service;

        public CalculationController(ICalculationService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "zohara" && request.password == "zd") 
            {
                var claims = new[]
                {
                  new Claim(ClaimTypes.Name, request.Username),
                  new Claim(ClaimTypes.Role, "Admin"), 
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-very-strong-secret-key-128-bits-long!"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: creds
                );

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                Console.WriteLine(jwt);
                Console.WriteLine(key);
                Console.WriteLine(creds);

                return Ok(new { token = jwt });
            }

            return Unauthorized();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CalculateAsync([FromBody] CalculationRequest request,[FromHeader(Name = "Operator")] string operatorHeader)
        {
            if (string.IsNullOrWhiteSpace(operatorHeader))
            {
                return BadRequest("Missing Operator in header");
            }

            try
            {
                double result = await _service.Calculate(request, operatorHeader);
                return Ok(result);
            }

            catch (DivideByZeroException)
            {
                return BadRequest("Cannot divide by zero");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("supported-operations")]
        public async Task<IActionResult> GetSupportedOperations()
        {
            var operations = await _service.GetSupportedOperations();
            return Ok(operations);
        }
    }
}
