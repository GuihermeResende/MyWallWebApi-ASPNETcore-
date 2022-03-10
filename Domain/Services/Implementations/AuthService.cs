using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyWallWebAPI.Domain.Models;
using MyWallWebAPI.Domain.Models.DTOs;
using MyWallWebAPI.Domain.Services.Interfaces;
using MyWallWebAPI.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Services.Implementations

{             //Uma implementa a outra.
    public class AuthService : IAuthService //É a classe que tem a regra de negócio, coisas para verificar, atualizar....
    {
        private readonly UserRepository _userRepository;

        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private object _userRepostory;

        public AuthService(UserRepository userRepository, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;

            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager; //Como se fosse uma service do identity.
            _signInManager = signInManager;
        }

        public async Task<List<ApplicationUser>> ListUsers()
        {
            List<ApplicationUser> listUsers = await _userRepository.ListUsers();

            return listUsers;
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            ApplicationUser user = await _userRepository.GetUser(userId);

            if (user == null)
                throw new ArgumentException("Usuário não existe!");

            return user;
        }

        public async Task<int> UpdateUser(ApplicationUser user)
        {
            ApplicationUser findUser = await _userRepository.GetUser(user.Id);
            if (findUser == null)
                throw new ArgumentException("Usuário não encontrado");
            findUser.Email = user.Email;
            findUser.UserName = user.UserName;

            return await _userRepository.UpdateUser(findUser); //Salvar o atributo escolhido em específico
        }

        public async Task<bool> DeleteUser(string userId)
        {
            ApplicationUser findUser = await _userRepository.GetUser(userId);
            if (findUser == null)
                throw new ArgumentException("Usuário não encontrado");

            await _userRepository.DeleteUser(userId);

            return true;
        }

        public async Task<bool> SignUp(SignUpDTO signUpDTO)
        {


            if (!signUpDTO.password.Equals(signUpDTO.passwordConfirm))
            {
                throw new ArgumentException("Password Confirmmed is different!");
            }

            var userExists = await _userManager.FindByNameAsync(signUpDTO.username);
            if (userExists != null)
                throw new ArgumentException("Username already exists!");

            userExists = await _userManager.FindByEmailAsync(signUpDTO.email);
            if (userExists != null)
                throw new ArgumentException("Email already exists!");


            ApplicationUser user;

            user = new ApplicationUser()
            {
                Email = signUpDTO.email,
                SecurityStamp = Guid.NewGuid().ToString(),  //conjunto de caracteres aleatórios
                UserName = signUpDTO.username
            };

            var result = await _userManager.CreateAsync(user, signUpDTO.password); //Criar o usuário

            if (!result.Succeeded)
            {
                if (result.Errors.Count() > 0)      //////MOSTRA OS ERROS QUE ACONTECERAM
                {
                    throw new ArgumentException(result.Errors.ElementAt(0).Description);
                }


            }

            // Faz Login automatico
            // var userExists = await _userManager.FindByNameAsync(registerDto.Username);

            return true;
        }
        //<Dado que retorna>
        public async Task<SsoDTO> SignIn(SignInDTO signInDTO)
        {
            var user = await _userManager.FindByNameAsync(signInDTO.username); //Tenta encontrar o Nome do Usuário
            if (user == null)
                throw new ArgumentException("Usuário não encontrado.");

            if (!await _userManager.CheckPasswordAsync(user, signInDTO.password)) //Tenta encontrar a senha do Usuário
                throw new ArgumentException("Senha inválida.");

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),  //Claim = atributo.
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3), //3 horas de acesso para o Token
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new SsoDTO(access_token: new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
        }

        public async Task<ApplicationUser> GetCurrentUser()
        {
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User); // Get user id:

            ApplicationUser user = await _userRepository.GetUser(userId);

            return user;
        }
    }
}
