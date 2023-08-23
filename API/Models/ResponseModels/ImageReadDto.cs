namespace API.Models.ResponseModels
{
    public class ImageReadDto
    {
        public Guid ImageId { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
    }
}
