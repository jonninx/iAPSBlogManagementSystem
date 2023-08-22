using API.Models.RequestModels;
using FluentValidation;

namespace API.Validators
{
    public class BlogUpdateDtoValidator : AbstractValidator<BlogUpdateDto>
    {
        public BlogUpdateDtoValidator()
        {
            RuleFor(dto => dto.Title).NotEmpty();
            RuleFor(dto => dto.Content).NotEmpty();
        }
    }
}
