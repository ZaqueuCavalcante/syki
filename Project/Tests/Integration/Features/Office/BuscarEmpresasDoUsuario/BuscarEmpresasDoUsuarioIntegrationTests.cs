namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_buscar_as_empresas_do_usuario()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();
        var cnpj = DataGen.Cnpj;
        var name = $"Nubank {DataGen.Numbers}";
        var razaoSocial = $"{name} LTDA";

        var clienteId = (await client.CriarEmpresa(name, cnpj, razaoSocial)).Success.Id;
        var userId = (await client.CriarUsuario(DataGen.UserName, DataGen.Email, DataGen.Cpf, [])).Success.Id;

        await client.VincularEmpresaUsuario(clienteId, userId);

        // Act
        var response = await client.BuscarEmpresasDoUsuario(userId, new());

        // Assert
        response.Total.Should().Be(1);
        var item = response.Items.Should().ContainSingle().Subject;
        item.Id.Should().Be(clienteId);
        item.Nome.Should().Be(name);
        item.Ativa.Should().BeTrue();
        item.CNPJ.Should().Be(cnpj.OnlyNumbers());
    }
}
