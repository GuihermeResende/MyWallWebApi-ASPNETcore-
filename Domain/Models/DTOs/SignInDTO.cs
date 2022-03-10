using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Models.DTOs
{
    public class SignInDTO //os dados não vão para o BD
    {
      
            [Required(ErrorMessage = "User Name is required")] //Decorador.. Se não for preenchido vai aparecer
                                                               //essa mensagem de erro
            public string username { get; set; }

            [Required(ErrorMessage = "Password is required")]
            public string password { get; set; }
        }
    }

