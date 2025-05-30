using FribergHomeAPI.Data;
using FribergHomeAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.FribergHomeAPIFork.Helpers
{
    public class ConfigServiceProvider
    {
        public static ServiceProvider UseInMemoryDatabase()
        {
            var services = new ServiceCollection();
            AddDbContext(services);
            return services.BuildServiceProvider();
        }

        public static ServiceProvider UseIdentityWithRoles()
        {
            var services = new ServiceCollection();
            AddIdentity(services);
            return services.BuildServiceProvider();
        }

        public static ServiceProvider UseBothInMemoryDatabaseAndIdentity()
        {
            var services = new ServiceCollection();
            AddDbContext(services);
            AddIdentity(services);
            return services.BuildServiceProvider();
        }

        private static void AddDbContext(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
        }

        private static void AddIdentity(IServiceCollection services)
        {
            services.AddIdentity<ApiUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddLogging();
        }
    }
}
