using API.Models.RequestModels;
using API.Models.ResponseModels;

namespace API.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentReadDto>> GetAllCommentsForBlogAsync(Guid blogId);
        Task<CommentReadDto> AddCommentToBlogAsync(Guid blogId, CommentCreateDto commentDto);
        Task<bool> DeleteCommentAsync(Guid commentId);
    }
}
