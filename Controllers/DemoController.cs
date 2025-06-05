namespace DomainEventDispatcher.Controllers
{
    using DomainEventDispatcher.Data;
    using DomainEventDispatcher.Domain.PersonAggregate;
    using DomainEventDispatcher.Features.Person.Create;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Demonstrates how domain events can automatically trigger actions 
    /// when aggregate roots are created (e.g., creating a User when a Person is added).
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DomainEventDemoController : ControllerBase
    {
        private readonly CreatePersonCommand _createUserCommand;
        private readonly AppDbContext _db;

        public DomainEventDemoController(AppDbContext dbContext)
        {
            _createUserCommand = createUserCommand;
            _db = dbContext;
        }

        /// <summary>
        /// Returns a list of all persons and their associated users.
        /// This helps visualize the result of domain events in action.
        /// </summary>
        [HttpGet("people-with-users")]
        public async Task<IActionResult> GetPeopleWithUsers()
        {
            var people = await _db.Persons
                .Include(p => p.User)
                .AsNoTracking()
                .Select(p => new
                {
                    personId = p.Id,
                    firstName = p.Name,
                    lastName = p.LastName,
                    userId = p.User!.Id,
                    username = p.User.Username
                })
                .ToListAsync();

            return Ok(people);
        }

        /// <summary>
        /// Creates a new Person.
        /// Behind the scenes, a User is automatically created using a domain event.
        /// </summary>
        [HttpPost("create-person")]
        public async Task<IActionResult> CreatePerson([FromBody] CreatePersonRequest request)
        {
            var person = Person.Create(request.Name, request.LastName);

            _db.Persons.Add(person);

            await _db.SaveChangesAsync();

            return Ok(new
            {
                person.Id,
                person.Name,
                person.LastName,
                userId = person.User!.Id,
                username = person.User.Username
            });
        }
    }

    /// <summary>
    /// Represents the payload for creating a new person.
    /// </summary>
    public record CreatePersonRequest(string Name, string LastName);
}
