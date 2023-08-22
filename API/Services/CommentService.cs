using API.Interfaces;
using API.Models.RequestModels;
using API.Models.ResponseModels;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CommentService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentReadDto>> GetAllCommentsForBlogAsync(Guid blogId)
        {
            var comments = await _dbContext.Comments.Where(c => c.BlogPostId == blogId).ToListAsync();
            return _mapper.Map<IEnumerable<CommentReadDto>>(comments);
        }

        public async Task<CommentReadDto> AddCommentToBlogAsync(Guid blogId, CommentCreateDto commentDto)
        {
            var comment = _mapper.Map<Comment>(commentDto);

            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CommentReadDto>(comment);
        }

        public async Task<bool> DeleteCommentAsync(Guid commentId)
        {
            var comment = await _dbContext.Comments.FindAsync(commentId);
            if (comment == null)
                throw new KeyNotFoundException("Comment not found");

            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
