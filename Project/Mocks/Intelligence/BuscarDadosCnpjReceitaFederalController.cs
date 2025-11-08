using Exato.Shared.Enums;
using Exato.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Exato.Back.Intelligence.Api;

namespace Exato.Mocks.Intelligence;

[ApiController]
public class BuscarDadosCnpjReceitaFederalController : ControllerBase
{
    [HttpGet("receita-federal/cnpj.json")]
    public IActionResult Get([FromQuery] string? cnpj)
    {
        cnpj = cnpj.OnlyNumbers();

        if (cnpj == "30680829000143")
        {
            var result = new BuscarDadosCnpjReceitaFederalOut
            {
                TransactionResultType = TransactionResultType.SuccessWithRemarks,
                Result = new()
                {
                    NomeFantasia = "BANCO NU",
                    NomeEmpresarial = "NU FINANCEIRA S.A.",
                    AtividadeEconomicaPrincipalCodigoNumeroNorm = "6436100",
                }
            };

            return Ok(result);
        }

        if (cnpj == "55330739000153")
        {
            var result = new BuscarDadosCnpjReceitaFederalOut
            {
                TransactionResultType = TransactionResultType.SuccessWithRemarks,
                Result = new()
                {
                    NomeFantasia = "DELOITTE RECEITA FEDERAL",
                    NomeEmpresarial = "DELOITTE RECEITA FEDERAL S.A.",
                    AtividadeEconomicaPrincipalCodigoNumeroNorm = "6436100",
                }
            };

            return Ok(result);
        }

        return BadRequest(new { Message = "Empresa não encontrada" });
    }
}
