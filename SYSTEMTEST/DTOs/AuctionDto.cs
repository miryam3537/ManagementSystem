namespace SYSTEMTEST.DTOs
{
    public class AuctionDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public decimal CurrentPrice { get; set; }

        public bool IsClosed { get; set; }

        public DateTime EndTime { get; set; }
    }
}
