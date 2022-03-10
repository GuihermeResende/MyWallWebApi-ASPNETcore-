using MyWallWebAPI.Domain.Models;
using MyWallWebAPI.Domain.Models.DTOs;
using MyWallWebAPI.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Services.Implementations
{

    public class ChatService
    {
        private readonly AuthService _authService;
        private readonly ChatService _chatService;
        private readonly ChatRepository _chatRepository;


        public ChatService(ChatService chatService, ChatRepository chatRepository)
        {
            _chatService = chatService;
            _chatRepository = chatRepository;
        }

        public async Task <List<Chat>> ListChats()
        {
            List<Chat> list = await _chatRepository.ListChats();
            return list;
        }

        public async Task<Chat> createChat(Chat chat)
        {
            ApplicationUser currentUser = await _authService.GetCurrentUser();
            Chat novoChat = new Chat();

            novoChat.userId = currentUser.Id;
            novoChat.lastDateTimeMessage = chat.lastDateTimeMessage;

            Chat getNovoChat = await _chatRepository.CreateChat(novoChat);

            return getNovoChat;
        }

        public async Task<Chat> GetChatById(int chatId)
        {
            Chat chat = await _chatRepository.GetChatById(chatId);

            if(chat == null)
                throw new ArgumentException("Este chat não existe");

            return chat;
        }

        public async Task<bool> DeleteChat(int chatId)
        {
            ApplicationUser currentUser = await _authService.GetCurrentUser();
            Chat findChat = await _chatRepository.GetChatById(chatId);

            if(findChat == null)
            {
                throw new ArgumentException("Este chat não existe");
            }

            if (!findChat.userId.Equals(currentUser.Id))
             {
                throw new ArgumentException("Voçê não tem permissão");
            }

            await _chatRepository.DeleteChat(chatId);

            return true;

        }
    }
}
