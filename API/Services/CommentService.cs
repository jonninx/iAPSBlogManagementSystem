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

        public async Task<PaginatedResponse<CommentReadDto>> GetPaginatedCommentsForBlogAsync(Guid blogId, CommentPaginationDto pagination)
        {
            try
            {
                var totalItems = await _dbContext.Comments.Where(c => c.BlogPostId == blogId).CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pagination.PageSize);

                var comments = await _dbContext.Comments
                                              .Where(c => c.BlogPostId == blogId)
                                              .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                                              .Take(pagination.PageSize)
                                              .ToListAsync();

                var commentDtos = _mapper.Map<IEnumerable<CommentReadDto>>(comments);

                return new PaginatedResponse<CommentReadDto>
                {
                    Data = commentDtos,
                    TotalCount = totalItems,
                    TotalPages = totalPages,
                    CurrentPage = pagination.PageNumber,
                    ItemsPerPage = pagination.PageSize
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CommentReadDto> GetCommentByIdAsync(Guid commentId)
        {
            try
            {
                var comment = await _dbContext.Comments.FindAsync(commentId);

                if (comment == null)
                {
                    return null;
                }

                return _mapper.Map<CommentReadDto>(comment);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CommentReadDto> AddCommentToBlogAsync(Guid blogId, CommentCreateDto commentDto)
        {
            try
            {
                var comment = _mapper.Map<Comment>(commentDto);

                _dbContext.Comments.Add(comment);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<CommentReadDto>(comment);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteCommentAsync(Guid commentId)
        {
            try
            {
                var comment = await _dbContext.Comments.FindAsync(commentId);
                if (comment == null)
                    throw new KeyNotFoundException("Comment not found");

                _dbContext.Comments.Remove(comment);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
