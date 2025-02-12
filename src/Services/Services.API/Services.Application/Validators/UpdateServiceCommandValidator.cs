using FluentValidation;
using Services.Application.CQRS.Services.Commands.UpdateService;

namespace Services.Application.Validators;

public class UpdateServiceCommandValidator : AbstractValidator<UpdateServiceCommand>
{
    public UpdateServiceCommandValidator()
    {
		RuleFor(x => x.ServiceRequestDto.ServiceName).NotEmpty();

		RuleFor(x => x.ServiceRequestDto.Price).ExclusiveBetween(0, 10000000);
	}
}
