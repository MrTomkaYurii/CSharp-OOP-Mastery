using System;
using CodeFirst.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CodeFirst.DataAccess.Factories
{
    public class PostgresDbContextFactory : IDesignTimeDbContextFactory<PostgresDbContext>
    {
        public PostgresDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgresDbContext>();

            // Хардкодимо connection string
            var connectionString = "Host=localhost;Port=5432;Database=MoviesEntityFrameworkDatabase;Username=postgres;Password=yourStrong(!)Password";

            optionsBuilder.UseNpgsql(connectionString);

            return new PostgresDbContext(optionsBuilder.Options);
        }
    }
}
