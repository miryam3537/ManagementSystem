using SYSTEMTEST.DTOs;


namespace SYSTEMTEST.Services.Interfaces
{
    public interface IAuctionService
    {
        Task<List<AuctionDto>> GetAllAsync();

        Task<AuctionDetailsDto?> GetByIdAsync(int id);

        Task PlaceBidAsync(
      int auctionId,
      decimal amount,
      byte[] rowVersion);
    }
}