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

        public async Task<IEnumerable<BlogReadDto>> GetAllBlogsAsync()
        {
            var blogs = await _context.BlogPosts.ToListAsync();
            return _mapper.Map<IEnumerable<BlogReadDto>>(blogs);
        }

        public async Task<BlogReadDto> GetBlogByIdAsync(Guid id)
        {
            var blog = await _context.BlogPosts.FindAsync(id);
            if (blog == null) return null;

            return _mapper.Map<BlogReadDto>(blog);
        }

        public async Task<BlogReadDto> CreateBlogAsync(BlogCreateDto blogDto)
        {
            var blogEntity = _mapper.Map<BlogPost>(blogDto);

            _context.BlogPosts.Add(blogEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<BlogReadDto>(blogEntity);
        }

        public async Task<bool> UpdateBlogAsync(Guid id, BlogUpdateDto blogDto)
        {
            var blog = await _context.BlogPosts.FindAsync(id);
            if (blog == null) return false;

            _mapper.Map(blogDto, blog);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteBlogAsync(Guid id)
        {
            var blog = await _context.BlogPosts.FindAsync(id);
            if (blog == null) return false;

            _context.BlogPosts.Remove(blog);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddLikeAsync(Guid blogId, string userId)
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

        public async Task<bool> UnlikeBlogPostAsync(Guid blogId, string userId)
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
    }
}
