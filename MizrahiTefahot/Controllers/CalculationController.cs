using Common;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

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

        [HttpGet("supported-operations")]
        public async Task<IActionResult> GetSupportedOperations()
        {
            var operations = await _service.GetSupportedOperations();
            return Ok(operations);
        }
    }
}
