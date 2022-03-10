using MyWallWebAPI.Domain.Models;
using MyWallWebAPI.Domain.Models.DTOs;
using MyWallWebAPI.Domain.Services.Interfaces;
using MyWallWebAPI.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Services.Implementations
{
    public class MessageService
    {
        private readonly MessageRepository _messageRepository;
        private readonly ChatService _chatService;
        private readonly ChatRepository _chatRepository;
        private readonly IAuthService _authService;
        private readonly UserRepository _userRepository;
        public MessageService(ChatService chatService, ChatRepository chatRepository, IAuthService authService, UserRepository userRepository)
        {
            _chatService = chatService;
            _chatRepository = chatRepository;
            _authService = authService;

        }

        public async Task<List<Message>> ListMessages()
        {
            List<Message> list = await _messageRepository.ListMessages();
            return list;
        }

        public async Task<List<Message>> ListMessagesByApplicationUserId()
        {
            ApplicationUser currentUser = await _authService.GetCurrentUser();
            List<Message> list = await _messageRepository.ListMessagesByApplicationUserId(currentUser.Id);

            return list;
        }

        public async Task<Message> getMessageById(int messageId)
        {
            Message message = await _messageRepository.getMessageById(messageId);

            if(message == null)
            {
                throw new ArgumentException("A mensagem não existe");
            }

            return message;
        }

        public async Task <Message> CreateMessage(int chatId, string conteudo, String userId)
        {
            ApplicationUser currentUser = await _authService.GetCurrentUser();
            ApplicationUser getRecieverUser = await _userRepository.GetUser(userId); //outro user no BD
            Chat chat = await _chatRepository.GetChatById(chatId);
            
            if (!chat.userId.Equals(currentUser.Id)) //Não existe chat
            {
                await _chatRepository.CreateChat(chat);
               
            }

            Message message = new Message();
            message.applicationUserId = currentUser.Id;
            message.userReciever = getRecieverUser.Id;
            message.chatId = chat.id;
            message.data = chat.lastDateTimeMessage; //
            message.conteudo = conteudo;

            message = await _messageRepository.createMessage(message);
               
         
            return message;

        }
    }
}
