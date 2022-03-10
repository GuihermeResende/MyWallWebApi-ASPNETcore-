using MyWallWebAPI.Domain.Models;
using MyWallWebAPI.Domain.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Services.Interfaces
{
    public interface IAuthService   //contrato. Possui as funções da AuthService. Quebra a dependência das outras classes
                             //com a AuthService
    {
        Task<List<ApplicationUser>> ListUsers();
        Task<ApplicationUser> GetUserById(string userId);
        Task<int> UpdateUser(ApplicationUser user);
        Task<bool> DeleteUser(string userId);
        Task<bool> SignUp(SignUpDTO signUpDTO);
        Task<SsoDTO> SignIn(SignInDTO signInDTO);
        Task<ApplicationUser> GetCurrentUser();





















    }
}
