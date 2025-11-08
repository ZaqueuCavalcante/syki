using Exato.Shared.Features.Office.CreateUser;
using Exato.Back.Features.Cross.CreateExatoUser;

namespace Exato.Back.Features.Office.CreateUser;

public class CreateUserService(CreateExatoUserService service) : IOfficeService
{
    private class Validator : AbstractValidator<CreateUserIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(InvalidUserName.I);
            RuleFor(x => x.Name).MaximumLength(50).WithError(InvalidUserName.I);

            RuleFor(x => x.Email).NotEmpty().WithError(InvalidEmail.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CreateUserOut, ExatoError>> Create(CreateUserIn data)
    {
        if (V.Run(data, out var error)) return error;

        var result = await service.Create(data.ToCreateExatoUserIn());

        if (result.IsError) return result.Error;

        return result.Success.ToCreateUserOut();
    }
}
