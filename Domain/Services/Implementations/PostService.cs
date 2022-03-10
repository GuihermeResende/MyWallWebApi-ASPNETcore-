using MyWallWebAPI.Domain.Models;
using MyWallWebAPI.Domain.Services.Interfaces;
using MyWallWebAPI.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallWebAPI.Domain.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly PostRepository _postRepository; //tem muitas funções dentro
        private readonly IAuthService _authService;
      

        public PostService(PostRepository postRepository, IAuthService authService)
        {
            _postRepository = postRepository;
            _authService = authService;
        }

        public async Task<List<Post>> ListPosts()
        {
            
           
            List<Post> list = await _postRepository.ListPosts();
  
            return list;
        }

        public async Task<List<Post>> ListMeusPosts()

        {                                            //(_authservice) variável da classe authService trazendo uma função de lá
            ApplicationUser currentUser = await _authService.GetCurrentUser();
            //(_postRepostiroy) variável do postRepository chamando uma função de lá
            List<Post> list = await _postRepository.ListPostsByApplicationUserId(currentUser.Id);

            return list;
        }



        public async Task<Post> GetPost(int postId)
        {
            Post post = await _postRepository.GetPostById(postId);

            if (post == null)
                throw new ArgumentException("Post não existe!");

            return post;
        }

        public async Task<Post> NovoPost(Post post)
        {
            //await espera a requisição da linha realizar, antes de seguir p a proxima    
            ApplicationUser currentUser = await _authService.GetCurrentUser();

            Post novoPost = new Post();

            novoPost.ApplicationUserId = currentUser.Id;
            novoPost.ApplicationUser = currentUser;
            novoPost.Data = DateTime.Now;
            novoPost.Titulo = post.Titulo;
            novoPost.Conteudo = post.Conteudo;

            novoPost = await _postRepository.CreatePost(novoPost);

            return novoPost;
        }

        public async Task<int> UpdatePost(Post post) //esse (post) recebe os atributos da classe Post,ex: titulo, conteudo
        {
            ApplicationUser currentUser = await _authService.GetCurrentUser();

            Post findPost = await _postRepository.GetPostById(post.Id);
            if (findPost == null)
                throw new ArgumentException("Post não existe!");

            if (!findPost.ApplicationUserId.Equals(currentUser.Id))
                throw new ArgumentException("Sem permissão.");

            
            findPost.Titulo = post.Titulo;
            findPost.Conteudo = post.Conteudo;

            return await _postRepository.UpdatePost(findPost);
        }

        public async Task<bool> DeletePostAsync(int postId)
        {
            ApplicationUser currentUser = await _authService.GetCurrentUser();

            Post findPost = await _postRepository.GetPostById(postId);

            if (findPost == null)
                throw new ArgumentException("Post não existe!");

            if (!findPost.ApplicationUserId.Equals(currentUser.Id))
                throw new ArgumentException("Sem permissão.");

            await _postRepository.DeletePostAsync(postId);

            return true;
        }

    }
}
      