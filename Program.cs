using Microsoft.AspNetCore.Builder;
using System.Diagnostics;

var options = new WebApplicationOptions
{
    Args = args,
    WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp", "dist")
};

var builder = WebApplication.CreateBuilder(options);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

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

app.MapFallbackToFile("/index.html");
app.Run();
