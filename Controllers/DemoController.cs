namespace DomainEventDispatcher.Controllers
{
    using DomainEventDispatcher.Data;
    using DomainEventDispatcher.Domain.PersonAggregate;
    using DomainEventDispatcher.Features.Person.Create;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {
        private readonly CreatePersonCommand _createUserCommand;
        private readonly AppDbContext _db;

        public DemoController(CreatePersonCommand createUserCommand, AppDbContext dbContext)
        {
            _createUserCommand = createUserCommand;
            _db = dbContext;
        }

        public record CreatePersonRequest(string Name, string LastName);

        [HttpGet("result")]
        public async Task<IActionResult> GetUsersAndPersons()
        {
            var users = await _db.Users.AsNoTracking().ToListAsync();
            var persons = await _db.Users.AsNoTracking().ToListAsync();

            return Ok(new { persons, users });
        }

        [HttpPost("create-person")]
        public async Task CreatePerson([FromBody] CreatePersonRequest request)
        {
            var person = Person.Create(request.Name, request.LastName);

            _db.Persons.Add(person);

            await _db.SaveChangesAsync();
        }
    }
}
