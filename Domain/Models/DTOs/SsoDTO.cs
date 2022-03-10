using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Models.DTOs
{
    public class SsoDTO //classe que retorna os dados do usuário
    {
        public string access_token { get; set; }
        public DateTime expiration { get; set; }

        public SsoDTO(string access_token, DateTime expiration) //construtor com o nome da clase
        {
            this.access_token = access_token;
            this.expiration = expiration;
        }
    }
}

