namespace Medical.Models
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        public int? MgId { get; set; }

        public string? Name { get; set; }
        public string MgName { get; set; }

        public int? SinglePrice { get; set; }

        public int? BoxPrice { get; set; }

        public int? Quantity { get; set; }

        public bool IsBox { get; set;}
    }
}
