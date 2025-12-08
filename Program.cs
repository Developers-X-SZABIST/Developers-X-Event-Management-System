using Microsoft.EntityFrameworkCore;
using Event_Management_System.Models;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



//Connecting to our SQL Server and adding our DatabaseContext as a service
//And getting the connection string from appsettings.json file (Event_Management_System is defined in appsettings.json)

builder.Services.AddSqlServer<DatabaseContext>(builder.Configuration.GetConnectionString("Event_Management_System") ?? throw new InvalidOperationException("Connection string 'Event_Management_System' not found."));

//if you don't understand the ?? its just a null-coalescing operator to throw an exception if the connection string is not found
//it just works by checking if the left side of the "??" is null, if it is not then run it, if it is then run the right side of the "??"



var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
