namespace API.Models.RequestModels
{
    public class ImageCreateDto
    {
        public IFormFile ImageFile { get; set; }
        public string Description { get; set; }
    }
}
