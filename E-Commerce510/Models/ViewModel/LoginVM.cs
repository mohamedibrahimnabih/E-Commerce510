using System.ComponentModel.DataAnnotations;

namespace E_Commerce510.Models.ViewModel
{
    public class LoginVM
    {
        public int Id { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        public bool RemmeberMe { get; set; }
    }
}
