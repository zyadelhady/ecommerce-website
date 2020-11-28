using System.ComponentModel.DataAnnotations;

namespace e_commerce.DTOs
{
  public class RegisterDto
  {
    [Required]
    public string password { get; set; }
    [Required]
    public string username { get; set; }
    [Required]
    [EmailAddress]
    public string email { get; set; }
  }
}