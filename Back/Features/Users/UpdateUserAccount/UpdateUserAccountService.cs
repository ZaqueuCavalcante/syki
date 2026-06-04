namespace Syki.Back.Features.Users.UpdateUserAccount;

public class UpdateUserAccountService(SykiDbContext ctx) : ISykiService
{
    private class Validator : AbstractValidator<UpdateUserAccountIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(InvalidUserName.I);
            RuleFor(x => x.Name).MaximumLength(100).WithError(InvalidUserName.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<SykiSuccess, SykiError>> Update(UpdateUserAccountIn data)
    {
        if (V.Run(data, out var error)) return error;

        var user = await ctx.Users.FirstAsync(u => u.Id == ctx.RequestUser.Id);
        user.Name = data.Name;
        await ctx.SaveChangesAsync();

        return SykiSuccess.I;
    }
}
