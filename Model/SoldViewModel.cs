namespace Medical.Models
{
    public class SoldViewModel
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public string? ItemName { get; set; }
        public int QuantitySold { get; set; }
        public bool IsFullBox { get; set; }
    }
}
