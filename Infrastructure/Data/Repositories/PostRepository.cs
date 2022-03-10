using Microsoft.EntityFrameworkCore;
using MyWallWebAPI.Domain.Models;
using MyWallWebAPI.Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq; //(p => p.data)
using System.Threading.Tasks;

namespace MyWallWebAPI.Infrastructure.Data.Repositories
{
    public class PostRepository 
    {
        private readonly MySQLContext _context;

        public PostRepository(MySQLContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> ListPosts() //Todos os posts
        {
            List<Post> list = await _context.Post.OrderBy(p => p.Data).Include(p => p.ApplicationUser).ToListAsync(); //posts ordenados por data e traz o nome do Usuário
             
            return list;
        }

        public async Task<List<Post>> ListPostsByApplicationUserId(String applicationUserId) //Post específico do ID do usuário
        {
            List<Post> list = await _context.Post.Where(p => p.ApplicationUserId.Equals(applicationUserId)).OrderBy(p => p.Data).ToListAsync(); //posts ordenados por data
                                                                                //Retorna um boolean se for igual
            return list;
        }

        public async Task<Post> GetPostById(int postId) //um específico pelo ID
        {
            Post post = await _context.Post.Include(p => p.ApplicationUser).FirstOrDefaultAsync((p => p.Id == postId));

            return post;
        }

        public async Task<Post> CreatePost(Post post)
        {
            var ret = await _context.Post.AddAsync(post);
            
            await _context.SaveChangesAsync();

            ret.State = EntityState.Detached;

            return ret.Entity;
        }

        public async Task<int> UpdatePost(Post post)
        {
            _context.Entry(post).State = EntityState.Modified;

            return await _context.SaveChangesAsync();
        }

        public async Task<bool> DeletePostAsync(int postId)
        {
            var item = await _context.Post.FindAsync(postId);
            _context.Post.Remove(item);

            await _context.SaveChangesAsync();

            return true;
        }

        //public async Task<int> LikePost(Post post)
        //{
        //    _context.Entry(post).State = EntityState.Modified;

        //    return await _context.SaveChangesAsync();
        //}




    }
}
