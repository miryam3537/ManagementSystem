using Microsoft.EntityFrameworkCore;
using SYSTEMTEST.Data;
using SYSTEMTEST.Hubs;
using SYSTEMTEST.Services;
using SYSTEMTEST.Middleware;
using SYSTEMTEST.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddDbContext<AuctionDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddHostedService<AuctionBackgroundService>();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapHub<AuctionHub>("/auctionHub");

app.Run();