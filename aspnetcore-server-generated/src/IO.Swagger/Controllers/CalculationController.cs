using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Service.Interface;
using Common;

namespace IO.Swagger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Only users with Admin role can access this controller
    public class CalculationController : ControllerBase
    {
        private readonly ICalculationService _service;

        // Injects the calculation service dependency.
        public CalculationController(ICalculationService service)
        {
            _service = service;
        }

        // Performs a calculation using the provided operator and input values
        [HttpPost]
        public async Task<IActionResult> CalculateAsync(
            [FromBody] CalculationRequest request,
            [FromHeader(Name = "Operator")] string operatorHeader)
        {
            // Check if the Operator header was provided
            if (string.IsNullOrWhiteSpace(operatorHeader))
            {
                return BadRequest("Missing Operator in header");
            }

            try
            {
                // Perform the calculation using the service layer
                double result = await _service.Calculate(request, operatorHeader);
                return Ok(result);
            }
            catch (DivideByZeroException)
            {
                // Handle division by zero specifically
                return BadRequest("Cannot divide by zero");
            }
            catch (ArgumentException ex)
            {
                // Handle unsupported operations or validation errors
                return BadRequest(ex.Message);
            }
            catch
            {
                // Catch-all for unexpected exceptions
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        // Gets the list of operations supported by the API 
        [HttpGet("supported-operations")]
        public async Task<IActionResult> GetSupportedOperations()
        {
            // Fetch supported operations from the service
            var operations = await _service.GetSupportedOperations();
            return Ok(operations);
        }
    }
}
