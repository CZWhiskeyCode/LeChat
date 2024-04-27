using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebApplication6.Model;
using Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSignalR().AddHubOptions<ChatHub>(hubOptions =>
{
    hubOptions.ClientTimeoutInterval = TimeSpan.FromSeconds(5);
    hubOptions.HandshakeTimeout = TimeSpan.FromSeconds(5);
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(5);
    hubOptions.MaximumReceiveMessageSize = 1024;
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chat");
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello, world!");
    });
});

app.Run();