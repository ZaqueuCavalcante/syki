using Exato.Web.Domain;
using Exato.Web.Extensions;
using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.BuscarUsuario;

namespace Exato.Back.Features.Office.BuscarUsuario;

public static class BuscarUsuarioMapper
{
    extension(User usuario)
    {
        public BuscarUsuarioOut ToBuscarUsuarioOut(Cliente cliente, Cliente dexterCliente, WebUser webUser, string? phone)
        {
            return new()
            {
                Id = usuario.Id,
                WebUserId = webUser?.Id ?? 0,
                Nome = usuario.FullName ?? "-",
                Email = usuario.Email ?? "-",
                Phone = phone ?? "-",
                Documento = usuario.GetCpf() ?? "-",
                CriadoEm = usuario.CreatedAt,
                Ativo = usuario.Active,
                OnboardStatus = webUser?.GetOnboardStatus(),
                DeletedAt = usuario.DeletedAt,

                Creditos = cliente.Saldo,
                ClienteId = cliente.ClienteId,
                BalanceInBrl = cliente.BalanceInBrl ?? 0,
                BalanceType = cliente.BalanceType.ToEnum<BalanceType>(),
                MetodoDePagamento = (cliente.FaturamentoTipoId ?? 1).ToEnum<MetodoDePagamento>(),

                Claims = webUser?.ExtraClaims.ToClaims() ?? [],

                DexterClienteId = dexterCliente?.ClienteId ?? 0,
                DexterCliente = dexterCliente?.Nome ?? "-",
            };
        }
    }
}
