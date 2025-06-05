namespace DomainEventDispatcher.Data
{
    using DomainEventDispatcher.Domain.PersonAggregate;
    using DomainEventDispatcher.Domain.UserAggregate;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Person> Persons => Set<Person>();
    }
}
