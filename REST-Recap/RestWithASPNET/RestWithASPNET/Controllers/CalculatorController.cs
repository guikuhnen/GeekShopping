using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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

		private decimal ConvertToDecimal(string strNumber)
		{
			if (decimal.TryParse(strNumber, out decimal number))
				return number;

			return 0;
		}

		private bool IsNumeric(string strNumber)
		{
			return decimal.TryParse(strNumber, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out decimal number);
		}
	}
}
