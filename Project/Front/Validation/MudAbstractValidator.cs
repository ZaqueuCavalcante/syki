using FluentValidation;

namespace Exato.Front.Validation;

public abstract class MudAbstractValidator<T> : AbstractValidator<T>
{
	public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
	{
		if (model is not T typed) return ["Invalid model type"];

		var ctx = ValidationContext<T>.CreateWithOptions(typed, x => x.IncludeProperties(propertyName));

		var result = await ValidateAsync(ctx);

		if (result.IsValid) return [];

		return result.IsValid ? [] :  result.Errors.Select(e => e.ErrorMessage);
	};
}
