using System.ComponentModel.DataAnnotations;

namespace API_Lab2.DTOs
{
    public class RegisterUserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
