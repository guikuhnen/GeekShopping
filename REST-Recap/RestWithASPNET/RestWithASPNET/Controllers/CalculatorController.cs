using Microsoft.AspNetCore.Mvc;

namespace RestWithASPNET.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CalculatorController : ControllerBase
	{
		private readonly ILogger<CalculatorController> _logger;

		public CalculatorController(ILogger<CalculatorController> logger)
		{
			_logger = logger;
		}

		[HttpGet("sum/{firstNumber}/{secondNumber}")]
		public IActionResult Get(string firstNumber, string secondNumber)
		{
			if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
			{
				decimal sum = ConvertToDecimal(firstNumber) + ConvertToDecimal(secondNumber);

				return Ok(sum.ToString());
			}

			return BadRequest("Invalid Input.");
		}

		private decimal ConvertToDecimal(string number)
		{
			return Convert.ToDecimal(number);
		}

		private bool IsNumeric(string number)
		{
			return IsNumeric(number.ToLower());
		}
	}
}
