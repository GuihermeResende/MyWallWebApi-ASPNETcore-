using MyWallWebAPI.Domain.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Models
{
    public class Message
    {
        public int id { get; set; }
        public String conteudo { get; set; }
        public DateTime data { get; set; }

        [ForeignKey("applicationUserId")]

        public String applicationUserId;
        public ApplicationUser user { get; set; }

        public String userReciever { get; set; }

        [ForeignKey("chatId")]
        public int chatId { get; set; }

        public Chat chat;




    }
}
