using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Example.Mobile.EntityFrameworkCore.DbContexts;

namespace Example.Mobile.Migrations
{
    public class ExampleMobileDbContextFactory : IDesignTimeDbContextFactory<ExampleMobileDbContext>
    {
        public ExampleMobileDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ExampleMobileDbContext>();
            optionsBuilder.UseSqlite("Data Source=Example-migrations.db");

            return new ExampleMobileDbContext(optionsBuilder.Options);
        }
    }
}
