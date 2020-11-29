using System.ComponentModel.DataAnnotations;

namespace e_commerce.DTOs
{
  public class LoginDto
  {
    [Required] public string email { get; set; }
    [Required] public string password { get; set; }
  }
}