using MyWallWebAPI.Domain.Models;
using MyWallWebAPI.Domain.Services.Interfaces;
using MyWallWebAPI.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Services.Implementations
{
    public class LikeService : ILikeService
    {
        private readonly LikeRepository _likeRepository;
        private readonly IPostService _postService;
        private readonly PostRepository _postRepository;
        private readonly IAuthService _authService;
        private readonly DislikeRepository _dislikeRepository;

        public LikeService(LikeRepository likeRepository, IPostService postService,PostRepository postRepository,IAuthService authSerivce, DislikeRepository dislikeRepository)
        {
            _likeRepository = likeRepository;
            _postService = postService;
            _postRepository = postRepository;
            _authService = authSerivce;
            _dislikeRepository = dislikeRepository;
        }

        public async Task<List<Like>> ListLikes()
        {
            List<Like> list = await _likeRepository.ListLikes();

            return list;

        }
    
        public async Task<List<Like>> ListMeusLikes()
        {
            ApplicationUser currentUser = await _authService.GetCurrentUser();
            List<Like> list = await _likeRepository.ListLikesByApplicationUserId(currentUser.Id);

            return list;

        }

        public async Task<Like> GetLike(int likeId) //like é uma informação da classe, por isso q precisa criar likeId
        {
            Like like = await _likeRepository.GetLikeById(likeId);

            if (like == null)
                throw new ArgumentException("Este like não existe");

            return like;
        }

        public async Task<Like> CreateLike(int postId)

        {
            
            ApplicationUser currentUser = await _authService.GetCurrentUser();

            Post post = await _postService.GetPost(postId);

            if(post == null)
            {
                throw new ArgumentException("O id do post é inválido");
            }

           
            Like findLike = await _likeRepository.GetLikeByUserIdAndPostId(currentUser.Id, postId);

                //like       id do user    //usuário autenticado    
           // if(getUserAndPost.userId.Equals(currentUser.Id) && getUserAndPost.postId.Equals(postID))
            
            if(findLike != null)
            {

                throw new ArgumentException("Você já deu like no Post");
            }

            Dislike findDislike = await _dislikeRepository.GetDislikeByUserIdAndPostId(currentUser.Id, postId);

            if(findDislike != null)
            {
                post.dislikeCount--;
                await _postRepository.UpdatePost(post);
                await _dislikeRepository.DeleteDislike(findDislike.id);

            }

            // Criando um like  
            Like like = new Like();

            like.postId = postId;
            like.userId = currentUser.Id;

            Like getLike = await _likeRepository.CreateLike(like); //atualizar isso no banco de dados.
           
            post.likeCount ++; //saber qual post deve ser contado (+1), através do ID do post
            
            await _postRepository.UpdatePost(post);

            return getLike; 
        }

        public async Task<bool> DeleteLike(int likeId)
        {

            ApplicationUser currentUser = await _authService.GetCurrentUser();
            Like findLike = await _likeRepository.GetLikeById(likeId);

            if (findLike == null)
                throw new ArgumentException("O like não existe");

            //findlike = um like
            //userId = (atributo da classe Like)

            //id do like, id usuario = (usuário que estou passado. id)

            if (!findLike.userId.Equals(currentUser.Id))
                throw new ArgumentException("Você não tem permissão");

            await _likeRepository.DeleteLikeAsync(likeId);
           
            return true;

        }
    }
}
