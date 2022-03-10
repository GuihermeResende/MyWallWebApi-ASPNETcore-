using Microsoft.EntityFrameworkCore;
using MyWallWebAPI.Domain.Models.DTOs;
using MyWallWebAPI.Infrastructure.Data.Contexts; //MySQLContext
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Infrastructure.Data.Repositories
{
    public class ChatRepository
    {
        private readonly MySQLContext _context;


        public ChatRepository(MySQLContext context)
        {
            _context = context;
        }

        public async Task<List<Chat>> ListChats()
        {                                            //eu quero ordenar pelo última mensagem que eu mandei
            List<Chat> list = await _context.Chat.OrderBy(p => p.lastDateTimeMessage).Include(p => p.user).ToListAsync();
            return list;
        }

        public async Task<Chat> CreateChat(Chat chat)
        {
            var ret = await _context.Chat.AddAsync(chat);
            await _context.SaveChangesAsync();
            ret.State = EntityState.Detached;
            return ret.Entity;
        }

        public async Task<Chat> GetChatById(int chatId)
        {
            Chat chat = await _context.Chat.Include(p => p.user).FirstOrDefaultAsync(p => p.id == chatId);
            
            return chat;
        }

        public async Task<int> UpdateChat(Chat chat)
        {
            _context.Entry(chat).State = EntityState.Modified;

            return await _context.SaveChangesAsync(); //retorna 1
        }

        public async Task<bool> DeleteChat(int chatId)
        {
            var item = await _context.Chat.FindAsync(chatId);
            _context.Chat.Remove(item);

            await _context.SaveChangesAsync();

            return true;
        }

    }
}
