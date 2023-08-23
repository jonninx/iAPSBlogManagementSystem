using API.Interfaces;
using API.Models.RequestModels;
using API.Models.ResponseModels;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace API.Controllers
{
    [ApiController]
    [Route("api/blogs/{blogId}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly INotificationService _notificationService;
        private readonly IBlogService _blogService;

        public CommentsController(ICommentService commentService, INotificationService notificationService, IBlogService blogService)
        {
            _commentService = commentService;
            _notificationService = notificationService;
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginatedCommentsForBlog(Guid blogId, [FromQuery] CommentPaginationDto pagination)
        {
            var paginatedComments = await _commentService.GetPaginatedCommentsForBlogAsync(blogId, pagination);
            if (paginatedComments.Data == null || !paginatedComments.Data.Any())
            {
                return NotFound();
            }
            return Ok(paginatedComments);
        }

        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetComment(Guid id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentToBlog(Guid blogId, CommentCreateDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _commentService.AddCommentToBlogAsync(blogId, commentDto);

            if (result is null)
            {
                return BadRequest("Error in adding the comment.");
            }

            var blog = await _blogService.GetBlogByIdAsync(blogId);
            if (blog != null)
            {
                var notification = new Notification
                {
                    BlogId = blogId,
                    UserId = blog.CreatorId,
                    Content = $"A new comment was added to your post titled '{blog.Title}'."
                };
                await _notificationService.CreateNotificationAsync(notification);
            }

            return CreatedAtAction(nameof(GetComment), new { id = result.Id }, commentDto);
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var success = await _commentService.DeleteCommentAsync(commentId);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
