using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Models.DTOs
{
    public class SignUpDTO
    {
        [Required(ErrorMessage = "User Name is required")]
        public string username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")] 
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string passwordConfirm { get; set; }
    
    }
}
