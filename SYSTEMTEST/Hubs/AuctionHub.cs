using Microsoft.AspNetCore.SignalR;

namespace SYSTEMTEST.Hubs
{
    public class AuctionHub : Hub
    {
        public async Task JoinAuction(int auctionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"auction-{auctionId}");
        }
    }
}
