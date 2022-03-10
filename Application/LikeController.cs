using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components; //Route
using Microsoft.AspNetCore.Mvc; //ApiController
using MyWallWebAPI.Domain.Models;
using MyWallWebAPI.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace MyWallWebAPI.Application
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class LikeController : ControllerBase
    {

        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpGet("list-likes")]
        public async Task<ActionResult> ListLikes()
        {
            try
            {
                List<Like> list = await _likeService.ListLikes();

                return Ok(list);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("list-meus-likes")]

        public async Task<ActionResult> ListMeusLikes()
        {

            try
            {
                List<Like> list = await _likeService.ListMeusLikes();

                return Ok(list);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet("get-like")]

        public async Task<ActionResult> GetLike([FromQuery] int likeId) //sempre quando eu passo um parametro, precisa do[from body] 
                                                                        //ou outro.
        {
            try
            {
                Like like = await _likeService.GetLike(likeId);

                return Ok(like);

            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [HttpPost("create-like")] //Like like porque estou criando um novo objeto de like, não pegando algo pelo id,etc
        public async Task<ActionResult> CreateLike([FromBody] int postId)
        {
            try
            {
                Like like = await _likeService.CreateLike(postId);
                
                return Ok(like);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete-like")]
        public async Task<ActionResult> DeleteLike([FromBody] int likeId)

        {

            try
            {
                return Ok(await _likeService.DeleteLike(likeId));
                  
            }

            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
                                                          
        }

    }
}
