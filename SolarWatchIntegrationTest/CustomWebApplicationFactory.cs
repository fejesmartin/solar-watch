using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarWatchApp.DataServices;
using SolarWatchApp.DataServices.Repositories;
using SolarWatchIntegrationTest.DBContextTest;

namespace SolarWatchIntegrationTest
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public ICityRepository CityRepository { get; }
        public DBTest DbTest { get; }

        public CustomWebApplicationFactory()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<SolarWatchContext>()
                .UseInMemoryDatabase("InMemoryDbForTesting")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            DbTest = new DBTest(options);
            CityRepository = new CityRepository(DbTest);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(DbTest);
                services.AddSingleton(CityRepository);
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
            });

            builder.UseEnvironment("Testing");
        }
    }
}