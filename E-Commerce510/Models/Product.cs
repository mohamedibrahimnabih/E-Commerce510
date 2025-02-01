namespace E_Commerce510.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Img { get; set; }
        public int Quntity { get; set; }
        public double Rate { get; set; }
        public double Discount { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
