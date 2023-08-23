using API.Interfaces;
using API.Models.RequestModels;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IImageService _imageService;

        public BlogsController(IBlogService blogService, IImageService imageService)
        {
            _blogService = blogService;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogs(int pageNumber = 1, int pageSize = 10, string searchTerm = null, string tag = null)
        {
            var paginatedBlogs = await _blogService.GetBlogPostsAsync(pageNumber, pageSize, searchTerm, tag);
            return Ok(paginatedBlogs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlog(Guid id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            if (blog == null)
                return NotFound();

            return Ok(blog);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Creator")]
        public async Task<IActionResult> CreateBlog(BlogCreateDto blogDto)
        {
            if (blogDto == null)
            {
                return BadRequest("Blog data cannot be null.");
            }

            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized("Unable to determine the logged-in user.");
            }

            blogDto.CreatorId = userId;

            var success = await _blogService.CreateBlogAsync(blogDto);

            if (success is not null)
            {
                return Ok("Blog created successfully.");
            }
            else
            {
                return StatusCode(500, "A problem occurred while handling your request.");
            }
        }

        [Authorize(Roles = "Administrator,Creator")]
        [HttpPut("{blogId}")]
        public async Task<IActionResult> UpdateBlog(Guid blogId, BlogUpdateDto blogUpdateDto)
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var blog = await _blogService.GetBlogByIdAsync(blogId);
            if (blog == null)
            {
                return NotFound();
            }

            if (blog.CreatorId != currentUserId && !User.IsInRole("Administrator"))
            {
                return Forbid();
            }

            var result = await _blogService.UpdateBlogAsync(blogId, blogUpdateDto);
            if (!result)
            {
                return BadRequest("Failed to update the blog post.");
            }

            return NoContent();
        }

        [Authorize(Roles = "Administrator,Creator")]
        [HttpDelete("{blogId}")]
        public async Task<IActionResult> DeleteBlog(Guid blogId)
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var blog = await _blogService.GetBlogByIdAsync(blogId);
            if (blog == null)
            {
                return NotFound();
            }

            if (blog.CreatorId != currentUserId && !User.IsInRole("Administrator"))
            {
                return Forbid();
            }

            var result = await _blogService.DeleteBlogAsync(blogId);
            if (!result)
            {
                return BadRequest("Failed to delete the blog post.");
            }

            return NoContent();
        }

        [HttpPost("{blogId}/like")]
        [Authorize]
        public async Task<IActionResult> LikeBlogPost(Guid blogId)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var success = await _blogService.AddLikeAsync(blogId, userId);

            if (success)
            {
                return Ok("Blog post liked successfully.");
            }
            else
            {
                return BadRequest("Unable to like blog post.");
            }
        }

        [HttpDelete("unlike/{blogId}")]
        [Authorize]
        public async Task<IActionResult> UnlikeBlogPost(Guid blogId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _blogService.UnlikeBlogPostAsync(blogId, userId);
            if (result)
            {
                return Ok(new { message = "Blog post unliked successfully." });
            }
            else
            {
                return BadRequest("Failed to unlike the blog post or blog post was not previously liked by this user.");
            }
        }

        [HttpPost("{blogId}/image")]
        [Authorize(Roles = "Administrator, Creator")]
        public async Task<IActionResult> UploadImage(Guid blogId, [FromForm] ImageCreateDto imageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // You can verify if the blog with the given ID exists 
            // (this step can be done in the service too).

            var image = await _imageService.AddImageToBlogAsync(blogId, imageDto);
            if (image == null)
            {
                return BadRequest("Error uploading the image.");
            }

            return CreatedAtAction(nameof(GetImageById), new { id = image.ImageId }, image);
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetImageById(Guid id)
        {
            var image = await _imageService.GetImagesForBlogAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            return Ok(image);
        }

        [HttpPut("{id}/image")]
        [Authorize(Roles = "Administrator, Creator")]
        public async Task<IActionResult> UpdateImage(Guid id, [FromBody] ImageUpdateDto imageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _imageService.UpdateImageAsync(id, imageDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}/image")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            var result = await _imageService.DeleteImageAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
