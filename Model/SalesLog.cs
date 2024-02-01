namespace Medical.Models
{
    public class SalesLog
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public Items? Items { get; set; }
        public int ItemId { get; set; }
        public int QuantitySold { get; set; }
        public bool IsFullBox { get; set; }
    }
}
