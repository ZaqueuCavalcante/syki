using Microsoft.EntityFrameworkCore;
using Exato.Back.Intelligence.Domain.Public;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_auditar_a_criacao_de_uma_empresa()
    {
        // Arrange
        var client = await _back.LoggedAsOfficeCS();

        var cnpj = DataGen.Cnpj;
        var nome = DataGen.OrgName;
        var razaoSocial = $"{nome} LTDA";
        var clienteId = (await client.CriarEmpresa(nome, cnpj, razaoSocial)).Success.Id;

        var ctx = _back.GetBackDbContext();
        var id = await ctx.ExatoAuditTrails.Where(x => x.EntityId == clienteId.ToString() && x.EntityType == "Cliente" && x.Action == "Insert")
            .Select(x => x.Id).FirstAsync();

        // Act
        var admClient = await _back.LoggedAsExatoAdm();
        var response = await admClient.GetAuditTrail(id);

        // Assert
        var audit = response.Success;
        audit.Name.Should().Be(nameof(Cliente));
        audit.Table.Should().Be("cliente");
        audit.Schema.Should().Be("public");
        audit.EntityId.Should().Be(clienteId.ToString());
        audit.Operation.Should().Be("CriarEmpresaController");
        audit.Action.Should().Be("Insert");
        audit.User.Should().Be(client.User.Name);
        audit.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        audit.Values.First(x => x.Column == "nome").Value.ToString().Should().Be(nome);
        audit.Values.First(x => x.Column == "cpf_cnpj").Value.ToString().Should().Be(long.Parse(cnpj.OnlyNumbers()).ToString());
        audit.Changes.Should().BeEmpty();
    }

    [Test]
    public async Task Deve_auditar_a_alteracao_do_cadastro_da_empresa()
    {
        // Arrange
        var client = await _back.LoggedAsOfficeCS();

        var cnpj = DataGen.Cnpj;
        var nome = DataGen.OrgName;
        var razaoSocial = $"{nome} LTDA";
        var clienteId = (await client.CriarEmpresa(nome, cnpj, razaoSocial)).Success.Id;

        var newCnpj = DataGen.Cnpj;
        await client.EditarCadastroDaEmpresa(
            id: clienteId,
            ativa: false,
            nome: $"{nome} 2",
            cnpj: newCnpj,
            razaoSocial: $"{razaoSocial} 2",
            matrizId: null,
            nomeFantasia: $"{razaoSocial} FANTASIA",
            slug: "empresa-slug-ltda",
            salesContact: "Zezo Gomes"
        );

        var ctx = _back.GetBackDbContext();
        var id = await ctx.ExatoAuditTrails.Where(x => x.EntityId == clienteId.ToString() && x.EntityType == "Cliente" && x.Action == "Update")
            .Select(x => x.Id).FirstAsync();

        // Act
        var admClient = await _back.LoggedAsExatoAdm();
        var response = await admClient.GetAuditTrail(id);

        // Assert
        var audit = response.Success;
        audit.Name.Should().Be(nameof(Cliente));
        audit.Table.Should().Be("cliente");
        audit.Schema.Should().Be("public");
        audit.EntityId.Should().Be(clienteId.ToString());
        audit.Operation.Should().Be("EditarCadastroDaEmpresaController");
        audit.Action.Should().Be("Update");
        audit.User.Should().Be(client.User.Name);
        audit.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        audit.Values.Should().BeEmpty();
        audit.Changes.First(x => x.Column == "ativo").Old.ToString().Should().Be("True");
        audit.Changes.First(x => x.Column == "ativo").New.ToString().Should().Be("False");
        audit.Changes.First(x => x.Column == "nome").Old.ToString().Should().Be(nome);
        audit.Changes.First(x => x.Column == "nome").New.ToString().Should().Be($"{nome} 2");
        audit.Changes.First(x => x.Column == "slug").Old.Should().BeNull();
        audit.Changes.First(x => x.Column == "slug").New.ToString().Should().Be("empresa-slug-ltda");
    }
}
