using Aplication;
using Infrastructure;
using Persentation.Hubs;
using Scalar.AspNetCore;
using Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddSignalR();

builder.Services.AddRazorPages();
builder.Services.AddUtilsService(builder.Configuration);
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationService();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.WithTitle("JWT + Refresh Token Auth API");
    });
}


app.UseRouting();
app.MapStaticAssets();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages()
   .WithStaticAssets();

app.UseStaticFiles();

app.MapHub<ChatHub>("/chathub");

app.UseAuthApiEndpoints();
app.Run();
