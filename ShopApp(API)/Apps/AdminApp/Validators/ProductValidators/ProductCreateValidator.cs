using FluentValidation;
using ShopApp_API_.Apps.AdminApp.Dtos.ProductDto;

namespace ShopApp_API_.Apps.AdminApp.Validators.ProductValidators
{
    public class ProductCreateValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Maximum length is 50");

            RuleFor(p => p.SalePrice)
                .NotEmpty().WithMessage("Sale price is required")
                .GreaterThan(100).WithMessage("Price should be greater than 100");

            RuleFor(p => p.CurrentPrice)
             .NotEmpty().WithMessage("Current price is required")
             .GreaterThan(100).WithMessage("Price should be greater than 100");

            RuleFor(p => p)
                .Custom((p, context) =>
                {
                    if (p.SalePrice < p.CurrentPrice)
                    {
                        context.AddFailure("Sale price cant be greater than Current price");
                    }
                });



        }
    }
}
