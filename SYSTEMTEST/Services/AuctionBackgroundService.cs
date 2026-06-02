using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using SYSTEMTEST.Data;
using SYSTEMTEST.Hubs;

public class AuctionBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHubContext<AuctionHub> _hubContext;

    public AuctionBackgroundService(
        IServiceScopeFactory scopeFactory,
        IHubContext<AuctionHub> hubContext)
    {
        _scopeFactory = scopeFactory;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();

            var now = DateTime.UtcNow;

            var expiredAuctions = await context.Auctions
                .Where(a => !a.IsClosed && a.EndTime <= now)
                .ToListAsync();

            foreach (var auction in expiredAuctions)
            {
                auction.IsClosed = true;
            }

            await context.SaveChangesAsync();

            foreach (var auction in expiredAuctions)
            {
                await _hubContext.Clients.Group($"auction-{auction.Id}")
                    .SendAsync("AuctionClosed", new
                    {
                        AuctionId = auction.Id
                    });
            }

            await Task.Delay(10000, stoppingToken); // כל 10 שניות
        }
    }
}