namespace Syki.Back.Extensions;

public static class FluentValidationExtensions
{
    extension<T>(AbstractValidator<T> validator)
    {
        public bool Run(T data, out SykiError error)
        {
            var result = validator.Validate(data);

            if (result.IsValid)
            {
                error = default!;
                return false;
            }

            error = (result.Errors.First().CustomState as SykiError)!;
            return true;
        }
    }

    extension<T, TProperty>(IRuleBuilderOptions<T, TProperty> ruleBuilder)
    {
        public IRuleBuilderOptions<T, TProperty> WithError(SykiError error)
        {
            return ruleBuilder
                .WithErrorCode(error.Code)
                .WithMessage(error.Message)
                .WithState(_ => error);
        }
    }
}
