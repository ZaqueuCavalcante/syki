using Newtonsoft.Json;
using Exato.Back.Intelligence.Api;

namespace Exato.Back.Features.Office.CriarEmpresa;

public record BuscarDadosDaEmpresaNaReceitaFederalCommand(Guid ExternalId) : ICommand;

public class BuscarDadosDaEmpresaNaReceitaFederalCommandHandler(BackDbContext ctx, IHttpClientFactory factory, IntelligenceApiSettings settings) : ICommandHandler<BuscarDadosDaEmpresaNaReceitaFederalCommand>
{
    public async Task Handle(Guid commandId, BuscarDadosDaEmpresaNaReceitaFederalCommand command)
    {
        var empresa = await ctx.PublicCliente
            .Where(x => x.ExternalId == command.ExternalId)
            .FirstAsync();

        var client = factory.CreateClient();
        client.BaseAddress = new Uri(settings.Url);

        var cnpj = empresa.CpfCnpj?.ToString().PadLeft(14, '0');
        var path = $"receita-federal/cnpj.json?token={settings.Token}&cnpj={cnpj}.json";
        var response = await client.GetAsync(path);
        var responseAsString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var result = JsonConvert.DeserializeObject<BuscarDadosCnpjReceitaFederalOut>(responseAsString);

            var num = Convert.ToInt32(result.Result.AtividadeEconomicaPrincipalCodigoNumeroNorm);
            var segmentId = await ctx.IbgeCnaeConsolidado.Where(x => x.SubclasseNum == num)
                .OrderByDescending(x => x.Id)
                .Select(x => x.SegmentoQuod)
                .FirstAsync();

            empresa.QuodSegmentId = segmentId;
            empresa.NomeFantasiaRf = result.Result.NomeFantasia;
            empresa.RazaoSocialRf = result.Result.NomeEmpresarial;
            await ctx.SaveChangesAsync();
        }
        else
        {
            var error = new { response.StatusCode, Data = responseAsString };
            throw new Exception(error.Serialize());
        }
    }
}
