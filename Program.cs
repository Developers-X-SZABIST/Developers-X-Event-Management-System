using Microsoft.EntityFrameworkCore;
using Event_Management_System.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["jwt_token"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF32.GetBytes("class-work-5E")
            )
        };
    });

//Connecting to our SQL Server and adding our DatabaseContext as a service
//And getting the connection string from appsettings.json file (Event_Management_System is defined in appsettings.json)

builder.Services.AddSqlServer<DatabaseContext>(builder.Configuration.GetConnectionString("Event_Management_System") ?? throw new InvalidOperationException("Connection string 'Event_Management_System' not found."));

//if you don't understand the ?? its just a null-coalescing operator to throw an exception if the connection string is not found
//it just works by checking if the left side of the "??" is null, if it is not then run it, if it is then run the right side of the "??"



var app = builder.Build();


//Note: DO NOT put this code in the builder.Services section above, it will not work there
//also: DO NOT put this code after app.Run(), it will never be reached there
//also: DO add-migration when changing schema, this WILL NOT create migrations for you, it only applies them
//auto update database 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        //Get DbContext service
        var context = services.GetRequiredService<DatabaseContext>();

        //Apply pending migrations (create database if it doesn't exist)
        context.Database.Migrate();

        //seed data here
        //example DbInitializer.SeedData(context); make the function there too
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "WHOOPS! An error occurred while migrating or seeding the database.");
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");


app.Run();
