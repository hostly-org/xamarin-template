using Microsoft.EntityFrameworkCore;
using Example.Mobile.EntityFrameworkCore.Entities;

namespace Example.Mobile.EntityFrameworkCore.DbContexts
{
    public class ExampleMobileDbContext : DbContext
    {
        public DbSet<Command> Commands { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Query> Queries { get; set; }

        public ExampleMobileDbContext(DbContextOptions<ExampleMobileDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Command>().ToTable(name: nameof(Command), schema: "log");
            modelBuilder.Entity<Event>().ToTable(name: nameof(Event), schema: "log");
            modelBuilder.Entity<Query>().ToTable(name: nameof(Query), schema: "log");

            base.OnModelCreating(modelBuilder);
        }
    }
}
