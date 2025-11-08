using Exato.Back.Features.Office.CriarEmpresa;
using Exato.Back.Features.Office.EditarCadastroDaEmpresa;

namespace Exato.Back.Intelligence.Domain.Public;

public class Cliente : Entity
{
    public int ClienteId { get; set; }

    public short? FaturamentoTipoId { get; set; }

    public short? PrecoId { get; set; }

    public string Nome { get; set; }

    public string? SegundoNome { get; set; }

    public long? CpfCnpj { get; set; }

    public bool GerarPdfConsultas { get; set; }

    public bool ArmazenarPdfConsultas { get; set; }

    public bool HabilitarConsultasPorEmail { get; set; }

    public bool Interno { get; set; }

    public bool PessoaFisica { get; set; }

    public string? Endereco { get; set; }

    public string? Numero { get; set; }

    public string? Complemento { get; set; }

    public string? Bairro { get; set; }

    public string? Cidade { get; set; }

    public string? Uf { get; set; }

    public int? Cep { get; set; }

    public string? Comentarios { get; set; }

    public string? Origem { get; set; }

    public bool Ativo { get; set; }

    public int Saldo { get; set; }

    public short? ResultDesatLimiteHrs { get; set; }

    public bool ClienteEmTeste { get; set; }

    public DateTime IncluidoEm { get; set; }

    public int IncluidoPor { get; set; }

    public DateTime? AlteradoEm { get; set; }

    public int? AlteradoPor { get; set; }

    public DateTime? ExcluidoEm { get; set; }

    public int? ExcluidoPor { get; set; }

    public bool StoreTransactionInput { get; set; }

    public bool StoreTransactionReturn { get; set; }

    public decimal? BalanceInBrl { get; set; }

    public short BalanceType { get; set; }

    public Guid ExternalId { get; set; }

    public short DataAccessLevel { get; set; }

    public int[]? UnauthorizedDatasources { get; set; }

    public DateTime? QuodSuccessfullyEnrollmentAt { get; set; }

    public string? QuodLastEnrollment { get; set; }

    public int? TransLimitPerHour { get; set; }

    public int? TransLimitPerDay { get; set; }

    public int? TransLimitPerWeek { get; set; }

    public int? TransLimitPerMonth { get; set; }

    public int? CreditsLimitPerHour { get; set; }

    public int? CreditsLimitPerDay { get; set; }

    public int? CreditsLimitPerWeek { get; set; }

    public int? CreditsLimitPerMonth { get; set; }

    public decimal? CurrencyLimitPerHour { get; set; }

    public decimal? CurrencyLimitPerDay { get; set; }

    public decimal? CurrencyLimitPerWeek { get; set; }

    public decimal? CurrencyLimitPerMonth { get; set; }

    /// <summary>
    /// Populado via <see cref="BuscarDadosDaEmpresaNaReceitaFederalCommand"/>
    /// </summary>
    public int? QuodSegmentId { get; set; }

    /// <summary>
    /// Populado via <see cref="BuscarDadosDaEmpresaNaReceitaFederalCommand"/>
    /// </summary>
    public string? RazaoSocialRf { get; set; }

    /// <summary>
    /// Populado via <see cref="BuscarDadosDaEmpresaNaReceitaFederalCommand"/>
    /// </summary>
    public string? NomeFantasiaRf { get; set; }

    /// <summary>
    /// Aponta pra <see cref="Realm.Id" />
    /// </summary>
    public short RealmId { get; set; }

    public string? PdfPassword { get; set; }

    public DateTime? MigratedAt { get; set; }

    public Guid? MigratedToClienteExternalId { get; set; }

    public string? Origin { get; set; }

    public DateTime? BalanceUpdatedAt { get; set; }

    public DateTime? LastCreditPurchaseDate { get; set; }

    public string? ExternalDisplayName { get; set; }

    public string? Slug { get; set; }

    public int? ParentOrganizationId { get; set; }

    public bool HighPerformance { get; set; }

    public bool ReceitaCpfUseSerproAsMainSource { get; set; }

    public bool ReceitaCpfNeedPdfProof { get; set; }

    public bool ReceitaCpfShouldReturnMinor18AgeData { get; set; }

    public int? PartnerId { get; set; }

    /// <summary>
    /// Aponta pra <see cref="OrganizationSegment.Id" />
    /// </summary>
    public int? OrganizationSegmentId { get; set; }

    /// <summary>
    /// ID do cliente no Hubspot para fins de cruzamento de dados com sistemas externos.
    /// </summary>
    public long? CrmClientId { get; set; }

    public string? ExatoSalesContact { get; set; }

    /// <summary>
    /// A nick name for the client, used to create a billing file name.
    /// </summary>
    public string? InvoiceSlug { get; set; }

    public bool? UseOcrExato { get; set; }

    public Guid? QuodCustomerExternalId { get; set; }

    public bool? UseSerproDataValidFacial { get; set; }

    public bool? UseDoccheck { get; set; }

    public int? DossierIdToExecutePf { get; set; }

    public int? DossierIdToExecutePj { get; set; }

    public int? DossierIdToExecutePfCreditAnalysis { get; set; }

    public int? DossierIdToExecutePjCreditAnalysis { get; set; }

    public bool? DoccheckUseEnrollmentByOrganization { get; set; }

    public long? BillingId { get; set; }

    public bool? IsBillingCustomer { get; set; }

    public bool? BlockSensitiveDataInQueryString { get; set; }

    /// <summary>
    /// Field to store billing information, created in order to facilitate OMIE integration. Created by Marcelo Navarro in 2025-04-25
    /// </summary>
    public string? BillingInstructions { get; set; }

    public string? PublicId { get; set; }

    /// <summary>
    /// Apenas para facilitar nas queries. <br/>
    /// Propriedade não mapeada pelo EF.
    /// </summary>
    public bool IsParent { get; set; }

    public Cliente() { }

    public Cliente(string nome, string razaoSocial, string cnpj)
    {
        // Cadastro
        Nome = nome;
        Ativo = true;
        ExternalDisplayName = nome;
        RazaoSocialRf = razaoSocial;
        CpfCnpj = long.Parse(cnpj.OnlyNumbers());
        QuodSegmentId = null;
        NomeFantasiaRf = null;
        Slug = null;
        ParentOrganizationId = null;
        OrganizationSegmentId = null;

        // Consultas
        HighPerformance = false;
        BlockSensitiveDataInQueryString = true;
        DataAccessLevel = Shared.Enums.DataAccessLevel.DadosDeCadastroCompleto.ToShort();
        TransLimitPerWeek = null;

        GerarPdfConsultas = true;
        ArmazenarPdfConsultas = true;
        HabilitarConsultasPorEmail = true;
        PdfPassword = cnpj.OnlyNumbers()[..6];

        ReceitaCpfNeedPdfProof = true;
        ReceitaCpfUseSerproAsMainSource = true;
        ReceitaCpfShouldReturnMinor18AgeData = false;

        // Faturamento
        Saldo = 0;
        BalanceInBrl = 0.00M;
        IsBillingCustomer = true;
        BalanceType = Shared.Enums.BalanceType.Creditos.ToShort();
        FaturamentoTipoId = MetodoDePagamento.PosPago.ToShort();

        Interno = false;
        PessoaFisica = false;
        ClienteEmTeste = false;
        StoreTransactionInput = true;
        StoreTransactionReturn = true;

        RealmId = 1;
        Origin = "Novo Admin";
        Origem = "Novo Admin";
        IncluidoPor = 7;
        IncluidoEm = DateTime.Now;

        PartnerId = null;
        CrmClientId = null;
        UseOcrExato = false;
        UseSerproDataValidFacial = false;

        ExatoSalesContact = null;

        var externalId = Guid.NewGuid();
        ExternalId = externalId;
        QuodCustomerExternalId = externalId;

        AddDomainEvent(new EmpresaCriadaDomainEvent(ExternalId));
    }

    public Cliente(string nome, string? cpf)
    {
        // Cadastro
        Nome = nome;
        Ativo = true;
        PessoaFisica = true;
        ExternalDisplayName = nome;
        CpfCnpj = cpf.OnlyNumbers().HasValue() ? long.Parse(cpf.OnlyNumbers()) : null;

        GerarPdfConsultas = true;
        ArmazenarPdfConsultas = true;
        HabilitarConsultasPorEmail = true;
        DataAccessLevel = Shared.Enums.DataAccessLevel.DadosMascarados.ToShort();

        // Faturamento
        Saldo = 0;
        BalanceInBrl = 0.00M;
        IsBillingCustomer = false;
        BalanceType = Shared.Enums.BalanceType.Reais.ToShort();
        FaturamentoTipoId = MetodoDePagamento.PrePago.ToShort();

        Interno = false;
        ClienteEmTeste = false;

        RealmId = 1;
        Origin = "Novo Admin";
        Origem = "Novo Admin";
        IncluidoPor = 7;
        IncluidoEm = DateTime.Now;

        UnauthorizedDatasources = [13100];
        TransLimitPerDay = 100;

        ExternalId = Guid.NewGuid();
    }

    public string? GetDocument()
    {
        if (CpfCnpj == null) return null;

        var doc = CpfCnpj.ToString();

        return doc.PadLeft(14, '0');
    }

    public TipoDeEmpresa GetTipo()
    {
        return ParentOrganizationId != null ? TipoDeEmpresa.Filial : IsParent ? TipoDeEmpresa.Matriz : TipoDeEmpresa.Avulsa;
    }

    public List<int> Relatorios()
    {
        var ids = new List<int>();

        if (DossierIdToExecutePf != null) ids.Add(DossierIdToExecutePf.Value);
        if (DossierIdToExecutePj != null) ids.Add(DossierIdToExecutePj.Value);
        if (DossierIdToExecutePfCreditAnalysis != null) ids.Add(DossierIdToExecutePfCreditAnalysis.Value);
        if (DossierIdToExecutePjCreditAnalysis != null) ids.Add(DossierIdToExecutePjCreditAnalysis.Value);

        return ids;
    }

    public void EditarCadastro(
        bool ativa,
        string nome,
        string cnpj,
        string razaoSocial,
        int? matrizId,
        string? nomeFantasia,
        string? slug,
        string? salesContact)
    {
        Ativo = ativa;
        Nome = nome;

        var newCnpj = long.Parse(cnpj.OnlyNumbers());
        if (newCnpj != CpfCnpj)
        {
            AddDomainEvent(new CadastroDaEmpresaEditadoDomainEvent(ExternalId));
        }
        CpfCnpj = newCnpj;

        ExternalDisplayName = nome;
        RazaoSocialRf = razaoSocial;
        ParentOrganizationId = matrizId;
        NomeFantasiaRf = nomeFantasia;
        Slug = slug;
        ExatoSalesContact = salesContact;
    }

    public void EditarRelatorios(
        int pf,
        int pj,
        int pfQuod,
        int pjQuod)
    {
        DossierIdToExecutePf = pf;
        DossierIdToExecutePj = pj;
        DossierIdToExecutePfCreditAnalysis = pfQuod;
        DossierIdToExecutePjCreditAnalysis = pjQuod;
    }

    public void EditarConsultas(
        bool highPerformance,
        bool blockSensitiveDataInQueryString,
        DataAccessLevel dataAccessLevel,
        int? transLimitPerWeek,
        bool gerarPdfConsultas,
        bool habilitarConsultasPorEmail,
        bool receitaCpfUseSerproAsMainSource,
        bool receitaCpfShouldReturnMinor18AgeData)
    {
        HighPerformance = highPerformance;
        BlockSensitiveDataInQueryString = blockSensitiveDataInQueryString;
        DataAccessLevel = dataAccessLevel.ToShort();
        TransLimitPerWeek = transLimitPerWeek;

        GerarPdfConsultas = gerarPdfConsultas;
        HabilitarConsultasPorEmail = habilitarConsultasPorEmail;

        ReceitaCpfUseSerproAsMainSource = receitaCpfUseSerproAsMainSource;
        ReceitaCpfShouldReturnMinor18AgeData = receitaCpfShouldReturnMinor18AgeData;
    }

    public void EditarFaturamento(
        bool habilitado,
        MetodoDePagamento metodoDePagamento)
    {
        IsBillingCustomer = habilitado;

        FaturamentoTipoId = metodoDePagamento.ToShort();

        BalanceType = metodoDePagamento == MetodoDePagamento.PrePago ?
            Shared.Enums.BalanceType.Reais.ToShort() : Shared.Enums.BalanceType.Creditos.ToShort();
    }

    public void EditarCreditos(int credits)
    {
        Saldo += credits;
    }

    public void EditarSaldo(decimal amount)
    {
        BalanceInBrl += amount;
    }

    public bool IsMatriz => ParentOrganizationId == null;
    public bool IsFilial => ParentOrganizationId != null;

    public bool IsPrePago => FaturamentoTipoId == MetodoDePagamento.PrePago.ToShort();
    public bool IsPosPago => FaturamentoTipoId == MetodoDePagamento.PosPago.ToShort();

    public bool IsReais => BalanceType == Shared.Enums.BalanceType.Reais.ToShort();
    public bool IsCreditos => BalanceType == Shared.Enums.BalanceType.Creditos.ToShort();

    public void SoftDelete()
    {
        Ativo = false;
        ExcluidoEm = DateTime.Now;
        ExcluidoPor = 7;
    }
}
