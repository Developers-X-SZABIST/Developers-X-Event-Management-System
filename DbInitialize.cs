using Event_Management_System.Models;
using Microsoft.EntityFrameworkCore;
namespace Event_Management_System
{
    public class DbInitialize
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                //apply pending migrations
                context.Database.Migrate();
            }
        }
    }
}

//trying to initialize the database and apply migrations at startup without needing to use package manager console