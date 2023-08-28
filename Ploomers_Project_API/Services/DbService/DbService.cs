using Microsoft.EntityFrameworkCore;
using Ploomers_Project_API.Models.Context;

namespace Ploomers_Project_API.Services.DbService
{
    public static class DbService
    {
        public static void MigrationInitialisation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                serviceScope.ServiceProvider.GetService<SqlServerContext>().Database.Migrate();
            }
        }
    }
}
