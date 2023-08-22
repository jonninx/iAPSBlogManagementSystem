using API.Models.RequestModels;
using API.Models.ResponseModels;

namespace API.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogReadDto>> GetAllBlogsAsync();
        Task<BlogReadDto> GetBlogByIdAsync(Guid id);
        Task<BlogReadDto> CreateBlogAsync(BlogCreateDto blogDto);
        Task<bool> UpdateBlogAsync(Guid id, BlogUpdateDto blogDto);
        Task<bool> DeleteBlogAsync(Guid id);
        Task<bool> AddLikeAsync(Guid blogId, string userId);
        Task<bool> UnlikeBlogPostAsync(Guid blogId, string userId);
    }
}
