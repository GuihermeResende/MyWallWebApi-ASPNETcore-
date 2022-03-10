using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Models
{
    public class Like
    {

        public int id { get; set; }

        [ForeignKey("userId")]  
        public ApplicationUser user{ get; set; } //Traz a classe Post para o Like;
        public String userId { get; set; }  //UM LIKE PODE TER APENAS 1 USUÁRIO.

        [ForeignKey("postId")]
        public Post post { get; set; } //traz a classe Post para o Like;
        public int postId { get; set; } 


    }
}
