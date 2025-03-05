using FluentValidation;
using Services.Application.CQRS.ServicesCategories.Commands.UpdateServiceCategory;

namespace Services.Application.Validators;

public class UpdateServiceCategoryCommandValidator : AbstractValidator<UpdateServiceCategoryCommand>
{
    public UpdateServiceCategoryCommandValidator()
    {
		RuleFor(x => x.ServiceCategoryRequestDto.CategoryName).NotEmpty();

		RuleFor(x => x.ServiceCategoryRequestDto.TimeSlotSize).GreaterThan(0).NotEmpty();
	}
}
