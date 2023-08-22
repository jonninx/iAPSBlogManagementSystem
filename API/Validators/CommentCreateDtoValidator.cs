using API.Models.RequestModels;
using FluentValidation;

namespace API.Validators
{
    public class CommentCreateDtoValidator : AbstractValidator<CommentCreateDto>
    {
        public CommentCreateDtoValidator()
        {
            RuleFor(dto => dto.Content).NotEmpty();
        }
    }
}
