namespace SYSTEMTEST.DTOs
{
    public class CreateBidDto
    {
        public decimal Amount { get; set; }

        public byte[] RowVersion { get; set; } = [];
    }
}
