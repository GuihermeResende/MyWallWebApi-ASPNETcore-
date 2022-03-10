using MyWallWebAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Services.Interfaces
{
    public interface ILikeService 
    {
      
        Task<List<Like>> ListLikes();
        Task<List<Like>> ListMeusLikes();
        Task<Like> GetLike(int likeId);
        Task<Like> CreateLike(int postId);
        Task<bool> DeleteLike(int likeId);







        }
}
