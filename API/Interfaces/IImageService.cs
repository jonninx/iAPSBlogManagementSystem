using API.Models.RequestModels;
using API.Models.ResponseModels;

namespace API.Interfaces
{
    public interface IImageService
    {
        Task<ImageReadDto> AddImageToBlogAsync(Guid blogId, ImageCreateDto imageDto);
        Task<IEnumerable<ImageReadDto>> GetImagesForBlogAsync(Guid blogId);
        Task<bool> UpdateImageAsync(Guid id, ImageUpdateDto imageDto);
        Task<bool> DeleteImageAsync(Guid imageId);
    }
}
