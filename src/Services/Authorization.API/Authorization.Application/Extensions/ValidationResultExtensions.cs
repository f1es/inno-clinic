using FluentValidation.Results;
using System.Text;

namespace Authorization.Application.Extensions;

public static class ValidationResultExtensions
{
	public static string GetErrors(this ValidationResult validationResult)
	{
		var errors = new StringBuilder();

		foreach (var error in validationResult.Errors)
		{
			errors.AppendLine(error.ErrorMessage);
		}

		return errors.ToString();
	}
}
