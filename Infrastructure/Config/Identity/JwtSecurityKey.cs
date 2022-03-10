using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


       
namespace MyWallWebAPI.Infrastructure.Config.Identity
    {
        public class JwtSecurityKey
        {
            public static SymmetricSecurityKey Create(string secret)
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            }
        }
    }

