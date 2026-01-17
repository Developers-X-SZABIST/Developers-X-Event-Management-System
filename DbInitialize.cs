using Event_Management_System.Models;
using Event_Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Event_Management_System
{
    public static class DbInitialize
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DatabaseContext>();

                    //Automatically apply any pending migrations
                    context.Database.Migrate();

                    
                    SeedData(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during database migration/seeding.");
                }
            }
        }

        private static void SeedData(DatabaseContext context)
        {
            
            var adminEmail = "admin@system.com";
            if (!context.Users.Any(u => u.Email == adminEmail))
            {
                context.Users.Add(new User
                {
                    Username = "System Admin",
                    Email = adminEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = Roles.Admin
                });
            }

            
            if (context.Users.Count() < 3)
            {
                context.Users.AddRange(new List<User>
                {
                    new User {
                        Username = "Sarah",
                        Email = "sarah@events.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("org123"),
                        Role = Roles.Organizer
                    },
                    new User {
                        Username = "Mike",
                        Email = "mike@festivals.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("org123"),
                        Role = Roles.Organizer
                    },
                    new User {
                        Username = "Alex",
                        Email = "alex@gmail.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                        Role = Roles.Public
                    },
                    new User {
                        Username = "Jordan",
                        Email = "jordan@yahoo.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                        Role = Roles.Public
                    }
                });
            }
            context.SaveChanges();

            // --- STEP 2: SEED EVENTS ---
            if (!context.Events.Any())
            {
                context.Events.AddRange(new List<Event>
                {
                    new Event
                    {
                        Title = "Global Tech Summit 2026",
                        Description = "Join industry leaders for a 3-day deep dive into AI, Quantum Computing, and the future of the Web.",
                        EventDate = DateTime.Now.AddMonths(2),
                        Location = "Grand Convention Center, NY",
                        MaxCapacity = "500",
                        RegistrationDeadline = DateTime.Now.AddMonths(1)
                    },
                    new Event
                    {
                        Title = "AI & Machine Learning Summit",
                        Description = "A deep dive into neural networks, LLMs, and the future of generative AI. Keynote speeches by industry leaders.",
                        EventDate = DateTime.Now.AddMonths(2),
                        Location = "Silicon Innovation Hub",
                        MaxCapacity = "300",
                        RegistrationDeadline = DateTime.Now.AddMonths(1).AddDays(15)
                    },
                    new Event
                    {
                        Title = "Full-Stack Web Boot Camp",
                        Description = "Accelerated workshop on .NET Core, React, and cloud deployment strategies for modern developers.",
                        EventDate = DateTime.Now.AddDays(12),
                        Location = "Tech Park Plaza",
                        MaxCapacity = "40",
                        RegistrationDeadline = DateTime.Now.AddDays(5)
                    },
                    new Event
                    {
                        Title = "Cyber Security Capture The Flag",
                        Description = "A competitive hacking event where participants solve security puzzles to win prizes and recognition.",
                        EventDate = DateTime.Now.AddDays(30),
                        Location = "Virtual / Global Link",
                        MaxCapacity = "1000",
                        RegistrationDeadline = DateTime.Now.AddDays(28)
                    },
                    new Event
                    {
                        Title = "Cloud Infrastructure Expo",
                        Description = "Explore the latest in Azure, AWS, and GCP. Networking opportunities with certified cloud architects.",
                        EventDate = DateTime.Now.AddMonths(3),
                        Location = "Grand Metro Convention Center",
                        MaxCapacity = "450",
                        RegistrationDeadline = DateTime.Now.AddMonths(2)
                    },
                    new Event
                    {
                        Title = "Game Dev Jam 2026",
                        Description = "48-hour event where teams build a game from scratch based on a surprise theme using Unity or Unreal Engine.",
                        EventDate = DateTime.Now.AddDays(60),
                        Location = "The Gaming Loft",
                        MaxCapacity = "100",
                        RegistrationDeadline = DateTime.Now.AddDays(50)
                    },
                    new Event
                    {
                        Title = "UI/UX Design Masterclass",
                        Description = "Focusing on accessibility, user psychology, and prototyping high-conversion interfaces in Figma.",
                        EventDate = DateTime.Now.AddDays(22),
                        Location = "Design District Studio",
                        MaxCapacity = "25",
                        RegistrationDeadline = DateTime.Now.AddDays(18)
                    }
                });
                context.SaveChanges();
            }

            // --- STEP 3: SEED REGISTRATIONS ---
            if (!context.Registrations.Any())
            {
                var users = context.Users.Where(u => u.Role == Roles.Public).ToList();
                var events = context.Events.Take(2).ToList();

                if (users.Any() && events.Any())
                {
                    context.Registrations.AddRange(new List<Registration>
                    {
                        new Registration {
                            UserId = users[0].UserId,
                            EventId = events[0].EventId,
                            RegistrationDate = DateTime.Now.AddDays(-2),
                            Status = "Confirmed"
                        },
                        new Registration {
                            UserId = users[1].UserId,
                            EventId = events[0].EventId,
                            RegistrationDate = DateTime.Now.AddDays(-1),
                            Status = "Confirmed"
                        },
                        new Registration {
                            UserId = users[0].UserId,
                            EventId = events[1].EventId,
                            RegistrationDate = DateTime.Now,
                            Status = "Confirmed"
                        }
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}