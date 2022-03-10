using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Models
{
    public class Dislike
    {
        public int id { get; set; }

        [ForeignKey("userId")] //
        public String userId { get; set; }
        public ApplicationUser user { get; set; } //

        [ForeignKey("postId")] //--
        public int postId { get; set; } 
        public Post post { get; set; } //-





    }
}
