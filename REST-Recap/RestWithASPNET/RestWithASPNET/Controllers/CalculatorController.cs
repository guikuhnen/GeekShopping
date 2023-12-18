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
		public IActionResult Sum(string firstNumber, string secondNumber)
		{
			if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
			{
				decimal sum = ConvertToDecimal(firstNumber) + ConvertToDecimal(secondNumber);

				return Ok(sum.ToString());
			}

			return BadRequest("Invalid Input.");
		}

		[HttpGet("subtraction/{firstNumber}/{secondNumber}")]
		public IActionResult Subtraction(string firstNumber, string secondNumber)
		{
			if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
			{
				decimal subtraction = ConvertToDecimal(firstNumber) - ConvertToDecimal(secondNumber);

				return Ok(subtraction.ToString());
			}

			return BadRequest("Invalid Input.");
		}

		[HttpGet("multiplication/{firstNumber}/{secondNumber}")]
		public IActionResult Multiplication(string firstNumber, string secondNumber)
		{
			if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
			{
				decimal multiplication = ConvertToDecimal(firstNumber) * ConvertToDecimal(secondNumber);

				return Ok(multiplication.ToString());
			}

			return BadRequest("Invalid Input.");
		}

		[HttpGet("division/{firstNumber}/{secondNumber}")]
		public IActionResult Division(string firstNumber, string secondNumber)
		{
			if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
			{
				decimal division = ConvertToDecimal(firstNumber) / ConvertToDecimal(secondNumber);

				return Ok(division.ToString());
			}

			return BadRequest("Invalid Input.");
		}

		[HttpGet("mean/{firstNumber}/{secondNumber}")]
		public IActionResult Mean(string firstNumber, string secondNumber)
		{
			if (IsNumeric(firstNumber) && IsNumeric(secondNumber))
			{
				decimal mean = (ConvertToDecimal(firstNumber) + ConvertToDecimal(secondNumber)) / 2;

				return Ok(mean.ToString());
			}

			return BadRequest("Invalid Input.");
		}

		[HttpGet("square-root/{number}")]
		public IActionResult SquareRoot(string number)
		{
			if (IsNumeric(number))
			{
				double sqrt = Math.Sqrt((double)ConvertToDecimal(number));

				return Ok(sqrt.ToString());
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
