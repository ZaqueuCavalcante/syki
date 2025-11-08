using Exato.Web;
using Exato.Shared.Features.Office.EditarCadastroDoUsuario;

namespace Exato.Back.Features.Office.EditarCadastroDoUsuario;

public class EditarCadastroDoUsuarioService(BackDbContext ctx, WebDbContext webCtx) : IOfficeService
{
    private class Validator : AbstractValidator<EditarCadastroDoUsuarioIn>
    {
        public Validator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithError(NomeDeUsuarioInvalido.I);
            RuleFor(x => x.Nome).MaximumLength(150).WithError(NomeDeUsuarioInvalido.I);

            RuleFor(x => x.Email).Must(x => x.IsValidEmail()).WithError(InvalidEmail.I);

            When(x => x.Cpf.HasValue(), () =>
            {
                RuleFor(x => x.Cpf).Must(x => x.IsValidCpf()).WithError(InvalidCpf.I);
            });
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<EditarCadastroDoUsuarioOut, ExatoError>> Editar(int id, EditarCadastroDoUsuarioIn data)
    {
        if (V.Run(data, out var error)) return error;

        var user = await ctx.PublicUsers
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        if (user == null) return UserNotFound.I;

        var userExternalId = user.ExternalId.ToString();
        var webUserId = await webCtx.WebUserCompanies.AsNoTracking()
            .Where(x => x.UserExternalId == userExternalId)
            .Select(x => x.UserId).FirstAsync();

        var emailUsed = await webCtx.UserEmails.AnyAsync(u => u.UserId != webUserId && u.Email == data.Email);
        if (emailUsed) return EmailAlreadyUsed.I;

        var webUser = await webCtx.Users
            .Where(x => x.Id == webUserId)
            .FirstAsync();
        var webUserEmail = await webCtx.UserEmails
            .Where(x => x.UserId == webUserId)
            .FirstAsync();

        webUser.EditarCadastro(data.Nome, data.Email, data.Cpf);
        webUserEmail.Editar(data.Email);
        await webCtx.SaveChangesAsync();

        user.EditarCadastro(data.Nome, data.Email, data.Cpf);
        await ctx.SaveChangesAsync();

        return new EditarCadastroDoUsuarioOut() { Id = id };
    }
}
