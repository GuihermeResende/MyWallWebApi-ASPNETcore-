using Microsoft.AspNetCore.Authorization; //Authorization
using Microsoft.AspNetCore.Components;
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

    public class DislikeController : ControllerBase
    {

        private readonly IDislikeService _dislikeService;

        public DislikeController(IDislikeService dislikeService)
        {
            _dislikeService = dislikeService;
        }


        [HttpGet("list-dislikes")]
        public async Task<ActionResult> listDislikes()
        {
            try
            {
                List<Dislike> list = await _dislikeService.ListDislikes();

                return Ok(list);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("list-meus-dislikes")]
        public async Task<ActionResult> ListDislikesByApplicationUserId()
        {
            try
            {
                List<Dislike> list = await _dislikeService.ListDislikesByApplicationUserId();

                return Ok(list);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("get-dislike")]

        public async Task<ActionResult> GetDislike([FromQuery] int dislikeId)
        {
            try
            {
                Dislike dislike = await _dislikeService.GetDislike(dislikeId);

                return Ok(dislike);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("create-dislike")]

        public async Task<ActionResult> CreateDislike([FromBody] int postId)
        {
            try
            {
                Dislike dislike = await _dislikeService.CreateDislike(postId);

                return Ok(dislike);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delte-dislike")]

        public async Task<ActionResult> DeleteDislike([FromBody] int dislikeId)
        {
            try
            {
                return Ok(await _dislikeService.DeleteDislike(dislikeId));
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}