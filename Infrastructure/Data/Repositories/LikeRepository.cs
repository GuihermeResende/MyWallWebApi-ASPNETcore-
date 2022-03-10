using Microsoft.EntityFrameworkCore;
using MyWallWebAPI.Domain.Models;
using MyWallWebAPI.Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Infrastructure.Data.Repositories
{
    public class LikeRepository
    {
        private readonly MySQLContext _context;


        public LikeRepository(MySQLContext context)
        {
            _context = context;
        }

        public async Task<List<Like>> ListLikes() //listar todos os likes
        {
            List<Like> list = await _context.Like.Include(p => p.user).ToListAsync();

            return list;
        }
                                                         //Criei essa variável para comparar.
        public async Task<List<Like>> ListLikesByApplicationUserId(String UserId) //Post específico do ID do usuário
        {
            List<Like> list = await _context.Like.Where(p => p.userId.Equals(UserId)).ToListAsync(); //posts ordenados por data
                                                                                                                                                //Retorna um boolean se for igual
            return list;
        }

        public async Task<Like> GetLikeByUserIdAndPostId(String userId, int postId) //saber se já deu like;
        {
            Like like = await _context.Like.Where(p => p.userId.Equals(userId) && p.postId.Equals(postId)).FirstOrDefaultAsync();
            
            return like;
        }


        public async Task<Like> GetLikeById(int likeId) //Um like específico pelo ID
        {
            Like like = await _context.Like.Include(p => p.user).FirstOrDefaultAsync((p => p.id == likeId));

            return like;
        }

        public async Task<Like> CreateLike(Like like)
        {
            var ret = await _context.Like.AddAsync(like);

            await _context.SaveChangesAsync();

            ret.State = EntityState.Detached;

            return ret.Entity;
        }

        public async Task<bool> DeleteLikeAsync(int LikeId)
        {
            var item = await _context.Like.FindAsync(LikeId);
            _context.Like.Remove(item);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
