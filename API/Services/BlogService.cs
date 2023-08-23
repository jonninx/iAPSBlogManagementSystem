using API.Interfaces;
using API.Models.RequestModels;
using API.Models.ResponseModels;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class BlogService : IBlogService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BlogService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        //Paginated
        public async Task<PaginatedResponse<BlogReadDto>> GetBlogPostsAsync(int pageNumber = 1, int pageSize = 10, string searchTerm = null, string tag = null)
        {
            try
            {
                var query = _context.BlogPosts.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(b => b.Title.Contains(searchTerm) || b.Content.Contains(searchTerm));
                }

                if (!string.IsNullOrEmpty(tag))
                {
                    query = query.Where(b => b.Tags.Contains(tag)); // Assuming Tags is a string, adjust if necessary
                }

                var totalCount = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var blogPosts = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var result = _mapper.Map<IEnumerable<BlogReadDto>>(blogPosts);

                return new PaginatedResponse<BlogReadDto>
                {
                    Data = result,
                    CurrentPage = pageNumber,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    TotalCount = totalCount
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<BlogReadDto> GetBlogByIdAsync(Guid id)
        {
            try
            {
                var blog = await _context.BlogPosts.FindAsync(id);
                if (blog == null) return null;

                return _mapper.Map<BlogReadDto>(blog);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<BlogReadDto> CreateBlogAsync(BlogCreateDto blogDto)
        {
            try
            {
                var blogEntity = _mapper.Map<BlogPost>(blogDto);

                var user = await _context.Users.FirstOrDefaultAsync(k => k.Email == blogDto.CreatorId);

                if(user is not null)
                {
                    blogEntity.CreatorId = user.Id;
                } else
                {
                    return null;
                }

                _context.BlogPosts.Add(blogEntity);
                await _context.SaveChangesAsync();

                return _mapper.Map<BlogReadDto>(blogEntity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<bool> UpdateBlogAsync(Guid id, BlogUpdateDto blogDto)
        {
            try
            {
                var blog = await _context.BlogPosts.FindAsync(id);
                if (blog == null) return false;

                _mapper.Map(blogDto, blog);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteBlogAsync(Guid id)
        {
            try
            {
                var blog = await _context.BlogPosts.FindAsync(id);
                if (blog == null) return false;

                _context.BlogPosts.Remove(blog);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> AddLikeAsync(Guid blogId, string userId)
        {
            try
            {
                var existingLike = await _context.BlogLikes.FirstOrDefaultAsync(l => l.BlogId == blogId && l.UserId == userId);

                if (existingLike != null)
                {
                    return false;
                }

                var like = new BlogLike
                {
                    BlogId = blogId,
                    UserId = userId
                };

                _context.BlogLikes.Add(like);

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UnlikeBlogPostAsync(Guid blogId, string userId)
        {
            try
            {
                var blogLike = await _context.BlogLikes
                                         .Where(bl => bl.BlogId == blogId && bl.UserId == userId)
                                         .FirstOrDefaultAsync();

                if (blogLike == null)
                {
                    return false;
                }

                _context.BlogLikes.Remove(blogLike);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
