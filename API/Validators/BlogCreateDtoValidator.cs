using API.Models.RequestModels;
using FluentValidation;

namespace API.Validators
{
    public class BlogCreateDtoValidator : AbstractValidator<BlogCreateDto>
    {
        public BlogCreateDtoValidator()
        {
            RuleFor(dto => dto.Title).NotEmpty();
            RuleFor(dto => dto.Content).NotEmpty();
        }
    }
}
