using Microsoft.AspNetCore.Builder;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Endpoints;

var options = new WebApplicationOptions
{
    Args = args,
    WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp", "dist")
};

var builder = WebApplication.CreateBuilder(options);

builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

var dataSource = app.Services.GetRequiredService<Microsoft.AspNetCore.Routing.EndpointDataSource>();
foreach (var e in dataSource.Endpoints)
    Console.WriteLine("UGA BUGA " + e.DisplayName);

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
    db.Database.Migrate(); // Apply any pending migrations
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

app.UseDefaultFiles(); // Enables default file mapping (e.g., index.html)
app.UseStaticFiles();

app.MapTasksEndpoints();

app.Lifetime.ApplicationStarted.Register(() =>
{
    try
    {
        var url = app.Urls.FirstOrDefault() ?? "http://localhost:5272";
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
    catch { /* ignore */ }
});

app.MapFallbackToFile("index.html");
app.Run();
