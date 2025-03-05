using FluentValidation;
using Services.Application.CQRS.ServicesCategories.Commands.CreateServiceCategory;

namespace Services.Application.Validators;

public class CreateServiceCategoryCommandValidator : AbstractValidator<CreateServiceCategoryCommand>
{
    public CreateServiceCategoryCommandValidator()
    {
        RuleFor(x => x.ServiceCategoryRequestDto.CategoryName).NotEmpty();

        RuleFor(x => x.ServiceCategoryRequestDto.TimeSlotSize).GreaterThan(0).NotEmpty();
    }
}
