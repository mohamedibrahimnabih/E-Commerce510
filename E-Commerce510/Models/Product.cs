using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce510.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(100)]
        public string Name { get; set; } 
        public string? Description { get; set; }
        [Range(0, 1000000)]
        public double Price { get; set; }
        //[RegularExpression("1.png|2.png")]
        [ValidateNever]
        public string Img { get; set; }
        [Range(0, 100)]
        public int Quntity { get; set; }
        public double Rate { get; set; }
        [Range(0, 100)]
        public double Discount { get; set; }

        public int CategoryId { get; set; }
        public int? CompanyId { get; set; }

        [ValidateNever]

        public Category Category { get; set; }
        public Company Company { get; set; }
    }
}
