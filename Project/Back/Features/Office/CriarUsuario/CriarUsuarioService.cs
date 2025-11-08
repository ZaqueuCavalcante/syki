using Exato.Web;
using Exato.Web.Domain;
using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.CriarUsuario;

namespace Exato.Back.Features.Office.CriarUsuario;

public class CriarUsuarioService(BackDbContext ctx, WebDbContext webCtx) : IOfficeService
{
    private class Validator : AbstractValidator<CriarUsuarioIn>
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

    public async Task<OneOf<CriarUsuarioOut, ExatoError>> Create(CriarUsuarioIn data)
    {
        if (V.Run(data, out var error)) return error;

        var emailUsed = await webCtx.UserEmails.AnyAsync(u => u.Email == data.Email);
        if (emailUsed) return EmailAlreadyUsed.I;

        await using var transaction = await ctx.Database.BeginTransactionAsync();

        var cliente = new Cliente(data.Nome, data.Cpf);
        await ctx.SaveChangesAsync(cliente);

        var user = new User(cliente.ClienteId, data.Nome, data.Email, data.Cpf);
        await ctx.SaveChangesAsync(user);

        var orgUser = new OrganizationUser(cliente.ClienteId, user.Id, true);
        await ctx.SaveChangesAsync(orgUser);

        var token = new TokenAcesso(cliente.ClienteId, user.Id);
        await ctx.SaveChangesAsync(token);

        await using var webTransaction = await webCtx.Database.BeginTransactionAsync();

        var webUser = new WebUser(data.Nome, data.Email, data.Cpf, data.Claims);
        await webCtx.SaveChangesAsync(webUser);

        var webUserEmail = new UserEmail(webUser.Id, data.Email);
        var webUserCompany = new WebUserCompany(webUser.Id, user.ExternalId!.Value, cliente.ExternalId, token.Token);

        webCtx.AddRange(webUserEmail, webUserCompany);
        await webCtx.SaveChangesAsync();

        await transaction.CommitAsync();
        await webTransaction.CommitAsync();

        return new CriarUsuarioOut() { Id = user.Id };
    }
}
