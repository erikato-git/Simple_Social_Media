using System.ComponentModel.DataAnnotations;

namespace webapi.DTOs
{
    // If I create a constructor it will give an error in LonInOk in UsersControllerTests.cs for some reason ... 
    public class UserCreateDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Full_Name { get; set; }
        [Required]
        public string Password { get; set; }

    }
}

