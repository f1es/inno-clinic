using FluentValidation;
using Services.Application.CQRS.Services.Commands.CreateService;

namespace Services.Application.Validators;

public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator()
    {
        RuleFor(x => x.ServiceRequestDto.ServiceName).NotEmpty();

        RuleFor(x => x.ServiceRequestDto.Price).ExclusiveBetween(0, 10000000);
    }
}
