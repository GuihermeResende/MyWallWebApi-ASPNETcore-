using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Models.DTOs
{
    public class Chat
    {
        public int id { get; set; }

        [ForeignKey("userId")]

        public string userId { get; set; }

        public ApplicationUser user { get; set; }
        public DateTime lastDateTimeMessage { get; set; }

        [JsonIgnore]
        public List<Message> Messages { get; set; }

    }
}
