using API.Models.RequestModels;
using API.Models.ResponseModels;

namespace API.Interfaces
{
    public interface ICommentService
    {
        Task<PaginatedResponse<CommentReadDto>> GetPaginatedCommentsForBlogAsync(Guid blogId, CommentPaginationDto pagination);
        Task<CommentReadDto> GetCommentByIdAsync(Guid commentId);
        Task<CommentReadDto> AddCommentToBlogAsync(Guid blogId, CommentCreateDto commentDto);
        Task<bool> DeleteCommentAsync(Guid commentId);
    }
}
