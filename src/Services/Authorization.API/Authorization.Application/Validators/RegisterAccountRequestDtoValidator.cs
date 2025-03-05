using Authorization.Core.Dto.Request;
using FluentValidation;

namespace Authorization.Application.Validators;

public class RegisterAccountRequestDtoValidator : AbstractValidator<RegisterAccountRequestDto>
{
    public RegisterAccountRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x => x.Password)
            .MinimumLength(6);

        var phoneNumberPattern = @"[\+][\s]?[0-9]{3}[\s]?[(]?[0-9]{2}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{2}[-\s\.]?[0-9]{2}";

        RuleFor(x => x.PhoneNumber)
            .Matches(phoneNumberPattern);
    }
}
