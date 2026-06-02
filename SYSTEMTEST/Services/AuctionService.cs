using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SYSTEMTEST.Data;
using SYSTEMTEST.DTOs;
using SYSTEMTEST.Entities;
using SYSTEMTEST.Hubs;
using SYSTEMTEST.Services.Interfaces;

namespace SYSTEMTEST.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly AuctionDbContext _context;

        private readonly IHubContext<AuctionHub> _hubContext;

        public AuctionService(
            AuctionDbContext context,
            IHubContext<AuctionHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<List<AuctionDto>> GetAllAsync()
        {
            return await _context.Auctions
                .Select(a => new AuctionDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    CurrentPrice = a.CurrentPrice,
                    EndTime = a.EndTime,
                    IsClosed = a.IsClosed
                })
                .ToListAsync();
        }

        public async Task<AuctionDetailsDto?> GetByIdAsync(int id)
        {
            return await _context.Auctions
                .Where(a => a.Id == id)
                .Select(a => new AuctionDetailsDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    CurrentPrice = a.CurrentPrice,
                    EndTime = a.EndTime,
                    IsClosed = a.IsClosed,

                    Bids = a.Bids
                        .OrderByDescending(b => b.CreatedAt)
                        .Select(b => new BidDto
                        {
                            Amount = b.Amount,
                            CreatedAt = b.CreatedAt
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task PlaceBidAsync(
            int auctionId,
            decimal amount,
            byte[] rowVersion)
        {
            var auction = await _context.Auctions
                .FirstOrDefaultAsync(a => a.Id == auctionId);

            if (auction == null)
                throw new Exception("Auction not found");

            // Concurrency check
            if (!auction.RowVersion.SequenceEqual(rowVersion))
                throw new Exception("Auction was updated. Please refresh.");

            if (auction.IsClosed)
                throw new Exception("Auction is closed");

            if (auction.EndTime <= DateTime.UtcNow)
                throw new Exception("Auction has ended");

            if (amount <= auction.CurrentPrice)
                throw new Exception("Bid must be higher than current price");

            auction.CurrentPrice = amount;

            _context.Bids.Add(new Bid
            {
                Amount = amount,
                CreatedAt = DateTime.UtcNow,
                AuctionId = auction.Id
            });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception(
                    "Someone else updated this auction. Try again.");
            }

            await _hubContext.Clients
                .Group($"auction-{auction.Id}")
                .SendAsync("BidUpdated", new
                {
                    AuctionId = auction.Id,
                    CurrentPrice = auction.CurrentPrice
                });
        }
    }
}