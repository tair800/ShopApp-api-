using FluentValidation;
using ShopApp_API_.Apps.AdminApp.Dtos.CategoryDto;

namespace ShopApp_API_.Apps.AdminApp.Validators.CategoryValidators
{
    public class CategoryCreateValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateValidator()
        {
            RuleFor(c => c.Name)
              .NotEmpty().WithMessage("Name is required")
              .MaximumLength(50).WithMessage("Maximum length is 50");

            RuleFor(c => c.Image)
                .NotNull().WithMessage("Image can't be empty")
                .Must(img => img.ContentType.Contains("images/")).WithMessage("Invalid image type")
                .Must(img => img.Length / 1024 > 1000).WithMessage("Invalid image size");

            //RuleFor(c => c).Custom((c, context) =>
            //{
            //    if(!(c.Image is not null && c.Image.ContentType.Contains("images/")))
            //    {
            //        context.AddFailure("Photo", "is required");
            //    }

            //    if(c.Image is not null && c.Image.Length / 1024 > 500)
            //    {
            //        context.AddFailure("Photo", "size doesnt match");
            //    }
            //});
        }


    }
}
