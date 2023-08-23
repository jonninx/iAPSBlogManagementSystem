using API.Interfaces;
using API.Models.RequestModels;
using API.Models.ResponseModels;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class ImageService : IImageService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ImageService(AppDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ImageReadDto> AddImageToBlogAsync(Guid blogId, ImageCreateDto imageDto)
        {
            try
            {
                var folderPath = _configuration.GetSection("ImageSettings:FolderPath").Value;

                if (imageDto.ImageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageDto.ImageFile.FileName);
                    var fullPath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await imageDto.ImageFile.CopyToAsync(stream);
                    }

                    var image = new Image
                    {
                        BlogId = blogId,
                        ImagePath = fullPath,
                        Description = imageDto.Description
                    };

                    _context.Images.Add(image);
                    await _context.SaveChangesAsync();

                    return _mapper.Map<ImageReadDto>(image);
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ImageReadDto>> GetImagesForBlogAsync(Guid blogId)
        {
            try
            {
                var images = await _context.Images
                                       .Where(i => i.BlogId == blogId)
                                       .ToListAsync();

                return _mapper.Map<IEnumerable<ImageReadDto>>(images);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateImageAsync(Guid imageId, ImageUpdateDto imageDto)
        {
            try
            {
                var image = await _context.Images.FindAsync(imageId);
                if (image == null)
                {
                    return false;
                }

                image.Description = imageDto.Description;

                _context.Images.Update(image);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteImageAsync(Guid imageId)
        {
            try
            {
                var image = await _context.Images.FindAsync(imageId);
                if (image == null)
                {
                    return false;
                }

                if (File.Exists(image.ImagePath))
                {
                    File.Delete(image.ImagePath);
                }

                _context.Images.Remove(image);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
