using Microsoft.EntityFrameworkCore;
using MyWallWebAPI.Domain.Models;
using MyWallWebAPI.Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Infrastructure.Data.Repositories
{
    public class DislikeRepository
    {
        private readonly MySQLContext _context;

        public DislikeRepository(MySQLContext context)
        {
            _context = context;
        }

        public async Task<List<Dislike>> ListDislikes()
        {
            List<Dislike> list = await _context.Dislike.Include(p => p.user).ToListAsync();

            return list;

        }

        public async Task<List<Dislike>> ListDeslikesByApplicationUserId(String userId)
        {
            List<Dislike> list = await _context.Dislike.Where(p => p.userId.Equals(userId)).ToListAsync();

            return list;

        }

        public async Task<Dislike> GetDislikeByUserIdAndPostId(String userId, int postId)
        {
            Dislike dislike = await _context.Dislike.Where(p => p.userId.Equals(userId) && p.postId.Equals(postId)).FirstOrDefaultAsync();
           
            return dislike;
        }

        public async Task<Dislike> GetDislikesById(int dislikeId)
        {
            Dislike dislike = await _context.Dislike.Where(p => p.id.Equals(dislikeId)).Include(p => p.user).FirstOrDefaultAsync();

            return dislike;
        }

        public async Task<Dislike> CreateDislike(Dislike dislike)
        {
            var ret = await _context.Dislike.AddAsync(dislike);

            await _context.SaveChangesAsync();

            ret.State = EntityState.Detached;

            return ret.Entity;

        }

        public async Task<bool> DeleteDislike(int deslikeId)
        {
            var item = await _context.Dislike.FindAsync(deslikeId);
            _context.Dislike.Remove(item);

            await _context.SaveChangesAsync();

            return true;

        }

    }
}
