namespace Medical.Models
{
    public class Items
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public Mg? Mgs { get; set; } // navigation property
        public int? Mg{ get; set; } // foreign key

        public int? SinglePrice { get; set;}

        public int? BoxPrice { get; set;}

        public int? Quantity { get; set;}

        public bool Isbox { get; set;}
        //public int? AvalaibleItems { get; set;}


    }
}
