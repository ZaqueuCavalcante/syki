using Exato.Back.Intelligence.Domain.Public;
using Exato.Back.Intelligence.Configs.Public;

namespace Exato.Back.Database;

public partial class BackDbContext
{
    public DbSet<CaptchaMecanismo> PublicCaptchaMecanismo { get; set; }
    public DbSet<Cliente> PublicCliente { get; set; }
    public DbSet<Consulta> PublicConsulta { get; set; }
    public DbSet<ConsultaDetalheTipo> PublicConsultaDetalheTipo { get; set; }
    public DbSet<ConsultaRelatorioTipo> PublicConsultaRelatorioTipo { get; set; }
    public DbSet<ConsultaResultadoTipo> PublicConsultaResultadoTipo { get; set; }
    public DbSet<ConsultaTipo> PublicConsultaTipo { get; set; }
    public DbSet<DataSourceProfilerConfig> PublicDataSourceProfilerConfig { get; set; }
    public DbSet<FaturamentoTipo> PublicFaturamentoTipo { get; set; }
    public DbSet<OrganizationSegment> PublicOrganizationSegment { get; set; }
    public DbSet<OrganizationUser> PublicOrganizationUser { get; set; }
    public DbSet<Origem> PublicOrigem { get; set; }
    public DbSet<Realm> PublicRealms { get; set; }
    public DbSet<Servidor> PublicServidor { get; set; }
    public DbSet<TokenAcessoConsultaTipo> PublicTokenAcessoConsultaTipo { get; set; }
    public DbSet<TokenAcesso> PublicTokenAcesso { get; set; }
    public DbSet<TokenAcessoQuantidade> PublicTokenAcessoQuantidades { get; set; }
    public DbSet<TokenCriptografia> PublicTokenCriptografia { get; set; }
    public DbSet<TransactionExecCallLog> PublicTransactionExecCallLog { get; set; }
    public DbSet<User> PublicUsers { get; set; }
    public DbSet<UserPhoneNumber> PublicUserPhoneNumbers { get; set; }

    public DbSet<DoccheckTokenSetting> PublicDoccheckTokenSettings { get; set; }

    private static void ConfigurePublicSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CaptchaMecanismoDbConfig());

        modelBuilder.ApplyConfiguration(new ClienteDbConfig());
        modelBuilder.HasSequence(ClienteDbConfig.ClienteIdSeq, "public");

        modelBuilder.ApplyConfiguration(new ConsultaDbConfig());
        modelBuilder.HasSequence(ConsultaDbConfig.ConsultaIdSeq, "public");

        modelBuilder.ApplyConfiguration(new ConsultaDetalheTipoDbConfig());

        modelBuilder.ApplyConfiguration(new ConsultaRelatorioTipoDbConfig());

        modelBuilder.ApplyConfiguration(new ConsultaResultadoTipoDbConfig());

        modelBuilder.ApplyConfiguration(new ConsultaTipoDbConfig());

        modelBuilder.ApplyConfiguration(new DataSourceProfilerConfigDbConfig());

        modelBuilder.ApplyConfiguration(new FaturamentoTipoDbConfig());

        modelBuilder.ApplyConfiguration(new OrganizationSegmentDbConfig());

        modelBuilder.ApplyConfiguration(new OrigemDbConfig());

        modelBuilder.ApplyConfiguration(new RealmDbConfig());

        modelBuilder.ApplyConfiguration(new ServidorDbConfig());

        modelBuilder.ApplyConfiguration(new TokenAcessoConsultaTipoDbConfig());
        modelBuilder.HasSequence(TokenAcessoConsultaTipoDbConfig.TokenAcessoConsultaTipoIdSeq, "public");

        modelBuilder.ApplyConfiguration(new TokenAcessoDbConfig());
        modelBuilder.HasSequence(TokenAcessoDbConfig.TokenAcessoIdSeq, "public");

        modelBuilder.ApplyConfiguration(new TokenAcessoQuantidadeDbConfig());

        modelBuilder.ApplyConfiguration(new TokenCriptografiaDbConfig());

        modelBuilder.ApplyConfiguration(new TransactionExecCallLogDbConfig());
        modelBuilder.HasSequence(TransactionExecCallLogDbConfig.TransactionExecCallLogIdSeq, "public");

        modelBuilder.HasSequence(UserDbConfig.UsersIdSeq, "public");
        modelBuilder.ApplyConfiguration(new UserDbConfig());

        modelBuilder.ApplyConfiguration(new OrganizationUserDbConfig());

        modelBuilder.ApplyConfiguration(new UserPhoneNumberDbConfig());

        modelBuilder.ApplyConfiguration(new DoccheckTokenSettingDbConfig());
    }

    private static void AddPostgresExtensions(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("citext")
            .HasPostgresExtension("pg_trgm")
            .HasPostgresExtension("pgcrypto")
            .HasPostgresExtension("unaccent")
            .HasPostgresExtension("uuid-ossp")
            .HasPostgresExtension("tablefunc")
            .HasPostgresExtension("postgres_fdw")
            .HasPostgresExtension("pg_stat_statements");
    }
}
