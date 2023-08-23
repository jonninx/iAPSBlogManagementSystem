using API.Models.RequestModels;
using API.Models.ResponseModels;

namespace API.Interfaces
{
    public interface IBlogService
    {
        // Not paginated
        // Task<IEnumerable<BlogReadDto>> GetAllBlogsAsync();

        // Paginated
        Task<PaginatedResponse<BlogReadDto>> GetBlogPostsAsync(int pageNumber, int pageSize, string searchTerm, string tag);

        Task<BlogReadDto> GetBlogByIdAsync(Guid id);
        Task<BlogReadDto> CreateBlogAsync(BlogCreateDto blogDto);
        Task<bool> UpdateBlogAsync(Guid id, BlogUpdateDto blogDto);
        Task<bool> DeleteBlogAsync(Guid id);
        Task<bool> AddLikeAsync(Guid blogId, string userId);
        Task<bool> UnlikeBlogPostAsync(Guid blogId, string userId);
    }
}
