using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Model;
using RestWithASPNET.Services;
using RestWithASPNET.Services.Implementations;

namespace RestWithASPNET.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PersonController : ControllerBase
	{
		private readonly ILogger<PersonController> _logger;
		private IPersonService _personService;

		public PersonController(ILogger<PersonController> logger, IPersonService personService)
		{
			_logger = logger;
			_personService = personService;
		}

		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_personService.FindAll());
		}

		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			Person person = _personService.FindById(id);

			if (person == null)
				return NotFound();

			return Ok(person);
		}

		[HttpPost]
		public IActionResult Create([FromBody] Person person)
		{
			Person newPerson = _personService.Create(person);

			if (newPerson == null)
				return BadRequest();

			return Ok(newPerson);
		}

		[HttpPut]
		public IActionResult Update([FromBody] Person person)
		{
			Person updatePerson = _personService.Update(person);

			if (updatePerson == null)
				return BadRequest();

			return Ok(updatePerson);
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			Person person = _personService.FindById(id);
			if (person == null)
				return NotFound();

			_personService.Delete(id);

			return NoContent();
		}
	}
}
