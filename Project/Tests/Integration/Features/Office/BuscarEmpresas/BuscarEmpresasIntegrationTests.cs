using Exato.Shared.Features.Office.BuscarEmpresas;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_buscar_empresas()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();
        var cnpj = DataGen.Cnpj;
        var name = DataGen.OrgName + " 68143654";
        var razaoSocial = $"{name} LTDA";

        await client.CriarEmpresa(name, cnpj, razaoSocial);

        // Act
        var response = await client.BuscarEmpresas(new BuscarEmpresasIn { Search = name });

        // Assert
        response.Total.Should().Be(1);
        var item = response.Items.Should().ContainSingle().Subject;
        item.Id.Should().BeGreaterThan(0);
        item.Nome.Should().Be(name);
        item.Ativa.Should().BeTrue();
        item.CNPJ.Should().Be(cnpj.OnlyNumbers());
        item.RazaoSocial.Should().Be(razaoSocial);
        item.MetodoDePagamento.Should().Be(MetodoDePagamento.PosPago);
        item.CriadaEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
    }

    [Test]
    public async Task Deve_filtrar_empresas_por_status()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();
        var cnpj = DataGen.Cnpj;
        var name = $"Uber {DataGen.Numbers}";
        var razaoSocial = $"{name} LTDA";
        var empresa = await client.CriarEmpresa(name, cnpj, razaoSocial);
        var id = empresa.Success.Id;

        var edit = await client.EditarCadastroDaEmpresa(id, false, name, cnpj, razaoSocial, null, null, null, null);
        edit.ShouldBeSuccess();

        // Act
        var response = await client.BuscarEmpresas(new BuscarEmpresasIn { Search = name, IsActive = false });

        // Assert
        response.Total.Should().Be(1);
        var item = response.Items.Should().ContainSingle().Subject;
        item.Ativa.Should().BeFalse();
    }

    [Test]
    public async Task Deve_filtrar_empresas_por_metodo_de_pagamento()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();
        var cnpj = DataGen.Cnpj;
        var name = $"Google {DataGen.Numbers}";
        var razaoSocial = $"{name} LTDA";
        var empresa = await client.CriarEmpresa(name, cnpj, razaoSocial);
        var id = empresa.Success.Id;

        await client.EditarFaturamentoDaEmpresa(id, false, MetodoDePagamento.PrePago);

        // Act
        var response = await client.BuscarEmpresas(new BuscarEmpresasIn
        {
            Search = name,
            PaymentMethod = MetodoDePagamento.PrePago,
        });

        // Assert
        response.Total.Should().Be(1);
        var item = response.Items.Should().ContainSingle().Subject;
        item.MetodoDePagamento.Should().Be(MetodoDePagamento.PrePago);
    }

    [Test]
    public async Task Deve_filtrar_empresas_por_tipo_de_empresa()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();
        var cnpj = DataGen.Cnpj;
        var name = $"JBS {DataGen.Numbers}";
        var razaoSocial = $"{name} LTDA";

        var matriz = await client.CriarEmpresa(name, cnpj, razaoSocial);
        var filial = await client.CriarEmpresa(name, cnpj, razaoSocial);
        await client.EditarCadastroDaEmpresa(filial.Success.Id, true, name, cnpj, razaoSocial, matriz.Success.Id, null, null, null);

        // Act
        var response = await client.BuscarEmpresas(new BuscarEmpresasIn
        {
            Search = name,
            ClientType = TipoDeEmpresa.Filial,
        });

        // Assert
        response.Total.Should().Be(1);
        var item = response.Items.Should().ContainSingle().Subject;
        item.Nome.Should().Be(name);
    }
}
