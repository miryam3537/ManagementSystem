using System.ComponentModel.DataAnnotations;

namespace SYSTEMTEST.Entities
{

    public class Auction
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public decimal CurrentPrice { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsClosed { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = [];

        public ICollection<Bid> Bids { get; set; }
            = new List<Bid>();
    }
}
