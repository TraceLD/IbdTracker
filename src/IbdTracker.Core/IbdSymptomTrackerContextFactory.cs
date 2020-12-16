using System;
using System.IO;
using IbdTracker.Core.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IbdTracker.Core
{
    public class IbdSymptomTrackerContextFactory : IDesignTimeDbContextFactory<IbdSymptomTrackerContext>
    {
        public IbdSymptomTrackerContext CreateDbContext(string[] args)
        {
            // build config;
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../IbdTracker"))
                .AddJsonFile("appsettings.json")
                .Build();
            
            // get connection string;
            var optionsBuilder = new DbContextOptionsBuilder<IbdSymptomTrackerContext>();
            var boundConfig = config.GetSection("Database").Get<DbConfig>();
            optionsBuilder
                .UseNpgsql(boundConfig.ConnectionString, b => b.MigrationsAssembly("IbdTracker.Core"));
            return new IbdSymptomTrackerContext(optionsBuilder.Options);
        }
    }
}