using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Example.Mobile.EntityFrameworkCore.DbContexts;
using Example.Mobile.Infrastructure;

namespace Example.Mobile.EntityFrameworkCore.Sqlite
{
    public static class ExampleMobileBuilderExtensions
    {
        public static IExampleMobileBuilder AddEntityFrameworkCoreSqlite(this IExampleMobileBuilder builder, Action<SqliteDbContextOptionsBuilder> optionsAction = null)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.AddEntityFrameworkCore();

            builder.Services.AddDbContext<ExampleMobileDbContext>((sp, options) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var connectionString = config.GetConnectionString("Default");
                string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Example.Mobile");
                System.IO.Directory.CreateDirectory(dbPath);
                var db = System.IO.Path.Combine(dbPath, connectionString);
                options.UseSqlite($"Filename={db}", optionsAction);
            });

            return builder;
        }
    }
}
