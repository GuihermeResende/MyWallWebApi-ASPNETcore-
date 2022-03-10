using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Models
{
    //caracteristicas do usuário: nome, ID
    public class ApplicationUser : IdentityUser //Aqui só tem atributos..
    {
        [JsonIgnore] //Tira o loop , porque há um ApplicationUser no Post também.
        public List<Post> posts { get; set; }
    }
}
