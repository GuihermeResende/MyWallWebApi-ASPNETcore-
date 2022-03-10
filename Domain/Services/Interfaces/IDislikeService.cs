using MyWallWebAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Services.Interfaces
{
    public interface IDislikeService
    {
        Task<List<Dislike>> ListDislikes();

        Task<List<Dislike>> ListDislikesByApplicationUserId();

        Task<Dislike> GetDislike(int deslikeId);

        Task<Dislike> CreateDislike(int postId);

        Task<bool> DeleteDislike(int dislikeId);
    }
}
