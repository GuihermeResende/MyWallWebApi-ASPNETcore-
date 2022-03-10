using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Models
{
    //Dominio traz do mundo real ao contexto do sistema
    //pasta models (blog) 
    public class Post
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public DateTime Data { get; set; }

        [ForeignKey("ApplicationUserId")]
        public String ApplicationUserId { get; set; } //FK
        public ApplicationUser ApplicationUser { get; set; } //Referencia

        public int likeCount { get; set; }

        public int dislikeCount { get; set; }

        [JsonIgnore]
        public List<Like> Likes { get; set; }
        ///public List<Dislikes> Dislikes { get; set; }

    }
}
