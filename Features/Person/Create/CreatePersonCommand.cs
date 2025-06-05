namespace DomainEventDispatcher.Features.Person.Create
{
    using DomainEventDispatcher.Data;
    using DomainEventDispatcher.Domain.PersonAggregate;

    public class CreatePersonCommand
    {
        private readonly AppDbContext _db;

        public CreatePersonCommand(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Person> Handle(string name, string lastname)
        {
            var person = Person.Create(name, lastname);

            _db.Persons.Add(person);

            await _db.SaveChangesAsync();

            return new Person
            {
                Id = person.Id,
                Name = person.Name,
                LastName = person.LastName,
            };
        }
    }
}
