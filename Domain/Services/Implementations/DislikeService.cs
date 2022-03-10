using MyWallWebAPI.Domain.Models;
using MyWallWebAPI.Domain.Services.Interfaces;
using MyWallWebAPI.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Services.Implementations
{
    public class DislikeService : IDislikeService
    {
        private readonly DislikeRepository _dislikeRepository;
        private readonly IAuthService _authService;
        private readonly IPostService _postService;
        private readonly PostRepository _postRepository;
        private readonly LikeRepository _likeRepository;

        public DislikeService(DislikeRepository dislikeRepository, IAuthService authService, IPostService postService,PostRepository postRepository, LikeRepository likeRepository)
        {
            _dislikeRepository = dislikeRepository;
            _authService = authService;
            _postService = postService;
            _postRepository = postRepository;
            _likeRepository = likeRepository;
        }

        public async Task<List<Dislike>> ListDislikes()
        {
            List<Dislike> list = await _dislikeRepository.ListDislikes();

            return list;
        }

        public async Task<List<Dislike>> ListDislikesByApplicationUserId()
        {
            ApplicationUser currentUser = await _authService.GetCurrentUser();
            List<Dislike> list = await _dislikeRepository.ListDeslikesByApplicationUserId(currentUser.Id);

            return list;

        }

        public async Task<Dislike> GetDislike(int deslikeId)
        {

            Dislike dislike = await _dislikeRepository.GetDislikesById(deslikeId);

            if (dislike == null)

                throw new ArgumentException("Este dislike não existe");

            return dislike;

        }

        public async Task<Dislike> CreateDislike(int postId)
        {
            ApplicationUser currentUser = await _authService.GetCurrentUser();

            // Passo 1: Verificar se o postId é válido. Se não for mandar uma mensagem de erro. Caso contrário, continuar.
            Post post = await _postService.GetPost(postId);

            if (post == null)
            {
                throw new ArgumentException("O id do post é inválido");
            }

            // Passo 2: Verificar se o currentUser já deu dislike neste post. Se já deu mandar mensagem de erro. Caso contrário, continuar.
            Dislike findDislike = await _dislikeRepository.GetDislikeByUserIdAndPostId(currentUser.Id, postId);

            if (findDislike != null)
            {
                throw new ArgumentException("Voçê já deu dislike no post");
            }

            // Passo 3: Verificar se o currentUser já deu like neste post. Se já tiver dado o like, remover o like. 
            Like findLike = await _likeRepository.GetLikeByUserIdAndPostId(currentUser.Id, postId);


            if (findLike != null)

            {
                // Passo 3.1: Diminuir o contador.
                post.likeCount--;
                await _postRepository.UpdatePost(post);

                // Passo 3.2: Remover o like.
                await _likeRepository.DeleteLikeAsync(findLike.id);
                
            }

            // Passo 4: Tudo ok para dar o deslike. 

            // Passo 4.1: Criar o objeto do Dislike;
            Dislike dislike = new Dislike();
            dislike.userId = currentUser.Id;
            dislike.postId = postId;

            Dislike getDislike = await _dislikeRepository.CreateDislike(dislike); //criando o dislike. Salvando no BD

            // Passo 4.2: Atualizar o contador dos dislikes no Post.
            post.dislikeCount++;
            await _postRepository.UpdatePost(post);

            return getDislike;
        }
  
        public async Task<bool> DeleteDislike(int dislikeId)
        {
            ApplicationUser currentUser = await _authService.GetCurrentUser();
            Dislike findDislike= await _dislikeRepository.GetDislikesById(dislikeId);

            if(findDislike == null)
            {
                throw new ArgumentException("Esse dislike não existe");
            }

            if (!findDislike.userId.Equals(currentUser.Id))
                  throw new ArgumentException("Você não pode dar dislike no Post");

            await _dislikeRepository.DeleteDislike(dislikeId);

            return true;
        }
    }
}
