using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StudentRegistry.DataAccess
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StudenDbContext>
    {
        public StudenDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../StudentRegistry/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<StudenDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new StudenDbContext(builder.Options);
        }
    }
}
