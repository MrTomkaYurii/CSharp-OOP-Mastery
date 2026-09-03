using System;
using CodeFirst.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CodeFirst.DataAccess.Factories
{
    public class SqlServerDbContextFactory : IDesignTimeDbContextFactory<SqlServerDbContext>
    {
        public SqlServerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqlServerDbContext>();

            // Хардкодимо connection string
            var connectionString =
                "Data Source=localhost,1434;Integrated Security=False;Database=MoviesEntityFrameworkDatabase;User ID=SA;Password=yourStrong(!)Password;TrustServerCertificate=True";

            optionsBuilder.UseSqlServer(connectionString);

            return new SqlServerDbContext(optionsBuilder.Options);
        }
    }
}
