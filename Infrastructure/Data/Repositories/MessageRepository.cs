using Microsoft.EntityFrameworkCore;
using MyWallWebAPI.Domain.Models;
using MyWallWebAPI.Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Infrastructure.Data.Repositories
{
    public class MessageRepository
    {
        private readonly MySQLContext _context;


        public async Task<List<Message>> ListMessages()
        {
            List<Message> list = await _context.Message.Include(p => p.user).ToListAsync();
            return list;
        }

        public async Task<List<Message>> ListMessagesByApplicationUserId(String userId)
        {
            List<Message> list = await _context.Message.Where(p => p.applicationUserId.Equals(userId)).ToListAsync();

            return list;
        }

        public async Task<Message> getMessageById(int messageId)
        {
            Message message = await _context.Message.Include(p => p.user).FirstOrDefaultAsync(p => p.id == messageId);

            return message;
        }

        public async Task<Message> createMessage(Message message)
        {
            var ret = await _context.Message.AddAsync(message);

            await _context.SaveChangesAsync();

            ret.State = EntityState.Detached;

            return ret.Entity;

        }

        public async Task<Message> getUserAndChatId(String userId, int chatId) //saber se já tem um chat
        {
            Message message = await _context.Message.Where(p => p.user.Equals(userId) && p.chatId.Equals(chatId)).FirstOrDefaultAsync();
            return message;
        }

        /*public async Task<ApplicationUser>  getReceiverUser(String userId)  //pegar algum usuário do BD
        {
            ApplicationUser user = await _context.User.Where(p => p.Equals(userId)).FirstOrDefaultAsync();
            return user;

        }
        */
        public async Task<bool> DeleteLikeAsync(int messageId)
        {
            var item = await _context.Message.FindAsync(messageId);
            _context.Message.Remove(item);

            await _context.SaveChangesAsync();

            return true;
        }
    }

}
