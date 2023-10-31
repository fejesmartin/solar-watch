using SolarWatchApp.DataServices;
using Microsoft.EntityFrameworkCore;

namespace SolarWatchIntegrationTest.DBContextTest
{
    public sealed class DBTest : SolarWatchContext
    {
        public DBTest(DbContextOptions<SolarWatchContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase(databaseName: "DBTest");
        }
    }
}