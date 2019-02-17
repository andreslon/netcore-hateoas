using Microsoft.AspNetCore.Mvc;
using NetCore.Hateoas.Api.Models;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCore.Hateoas.Api.Controllers
{
    [Produces("application/json+hateoas")]
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        private readonly IEnumerable<PersonDto> people;
        public PeopleController()
        {
            people = new List<PersonDto>() {
                new PersonDto { Id = 1, Name = "Fanie", Email = "fanie@reynders.co" },
                new PersonDto { Id = 2, Name = "Maarten", Email = "maarten@example.com" },
                new PersonDto { Id = 3, Name = "Marcel", Email = "marcel@example.com" }
            };
        }

        private PersonDto GetPerson(int id)
        {
            return people.Single(p => p.Id == id);
        }
        [HttpGet(Name = "get-people")]
        public IActionResult Get()
        {
            return Ok(people);
        }

        [HttpGet("{id}", Name = "get-person")]
        public IActionResult Get(int id)
        {
            var person = GetPerson(id);
            return Ok(person);
        }
        /// <summary>
        /// Creates a person.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /People
        ///     {
        ///        "id": 1,
        ///        "name": "andres",
        ///        "email": "andres@gmail.com"
        ///     }
        ///
        /// </remarks>
        /// <param name="person"></param>
        /// <returns>A newly created person</returns>
        /// <response code="200">Returns the newly created person</response>
        /// <response code="400">If the item is null</response>
        [ProducesResponseType(typeof(PersonDto), 200)]
        [ProducesResponseType(400)]
        [HttpPost(Name = "create-person")]
        public IActionResult Post([FromBody]PersonDto person)
        {
            person.Id = people.Count() + 1;
            ((List<PersonDto>)people).Add(person);
            return Ok(person);
        }

        [HttpPut("{id}", Name = "update-person")]
        public IActionResult Put(int id, [FromBody]PersonDto person)
        {
            var oldPerson = GetPerson(id);

            oldPerson.Name = person.Name;
            oldPerson.Email = person.Email;
            return NoContent();
        }
        /// <summary>
        /// Deletes a specific person.
        /// </summary>
        /// <param name="id"></param> 
        [HttpDelete("{id}", Name = "delete-person")]
        public IActionResult Delete(int id)
        {
            var person = GetPerson(id);

            ((List<PersonDto>)people).Remove(person);
            return Ok();
        }
    }
}
