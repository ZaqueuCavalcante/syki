using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class ConsultaTipoDbConfig : IEntityTypeConfiguration<ConsultaTipo>
{
    public void Configure(EntityTypeBuilder<ConsultaTipo> entity)
    {
        entity.ToTable("consulta_tipo", "public", x =>
        {
            x.HasCheckConstraint("consulta_tipo_nome_check", "char_length((nome)::text) <= 80");
            x.HasCheckConstraint("consulta_tipo_email_entrada_consultas_check", "char_length((email_entrada_consultas)::text) <= 80");
        });

        entity.HasKey(e => e.ConsultaTipoId)
            .HasName("consulta_tipo_pk");

        entity.Property(e => e.ConsultaTipoId)
            .ValueGeneratedNever()
            .HasColumnName("consulta_tipo_id");

        entity.Property(e => e.CaptchaMecanismoId)
            .HasColumnName("captcha_mecanismo_id");

        entity.Property(e => e.AllowBatchProc)
            .HasColumnName("allow_batch_proc");

        entity.Property(e => e.AlteradoEm)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()")
            .HasColumnName("alterado_em");

        entity.Property(e => e.AlteradoPor)
            .HasDefaultValue(0)
            .HasColumnName("alterado_por");

        entity.Property(e => e.AlternarIpsEProxies)
            .HasDefaultValue(true)
            .HasColumnName("alternar_ips_e_proxies");

        entity.Property(e => e.ArmazenarPdf)
            .HasColumnName("armazenar_pdf");

        entity.Property(e => e.BlockThreadsDuringIpProxySelection)
            .HasColumnName("block_threads_during_ip_proxy_selection");

        entity.Property(e => e.BloqueioIpErroQtd)
            .HasColumnName("bloqueio_ip_erro_qtd");

        entity.Property(e => e.BloqueioIpErroTempoMs)
            .HasColumnName("bloqueio_ip_erro_tempo_ms");

        entity.Property(e => e.BloqueioIpTimeoutQtd)
            .HasColumnName("bloqueio_ip_timeout_qtd");

        entity.Property(e => e.BloqueioIpTimeoutTempoMs)
            .HasColumnName("bloqueio_ip_timeout_tempo_ms");

        entity.Property(e => e.BloqueioProxyErroQtd)
            .HasColumnName("bloqueio_proxy_erro_qtd");

        entity.Property(e => e.BloqueioProxyErroTempoMs)
            .HasColumnName("bloqueio_proxy_erro_tempo_ms");

        entity.Property(e => e.BloqueioProxyTimeoutQtd)
            .HasColumnName("bloqueio_proxy_timeout_qtd");

        entity.Property(e => e.BloqueioProxyTimeoutTempoMs)
            .HasColumnName("bloqueio_proxy_timeout_tempo_ms");

        entity.Property(e => e.CaptchaOtimizarTaxaRecusados)
            .HasColumnName("captcha_otimizar_taxa_recusados");

        entity.Property(e => e.CaptchaTimeoutMs)
            .HasColumnName("captcha_timeout_ms");

        entity.Property(e => e.ClienteLimiteMs)
            .HasColumnName("cliente_limite_ms");

        entity.Property(e => e.CustoCred)
            .HasDefaultValue((short)0)
            .HasColumnName("custo_cred");

        entity.Property(e => e.CustoCredPdfArmaz)
            .HasDefaultValue((short)0)
            .HasColumnName("custo_cred_pdf_armaz");

        entity.Property(e => e.CustoCredPdfGeracao)
            .HasDefaultValue((short)0)
            .HasColumnName("custo_cred_pdf_geracao");

        entity.Property(e => e.Disponivel)
            .HasColumnName("disponivel");

        entity.Property(e => e.DuracaoCacheHrs)
            .HasColumnName("duracao_cache_hrs");

        entity.Property(e => e.EmailEntradaConsultas)
            .HasColumnType("citext")
            .HasColumnName("email_entrada_consultas");

        entity.Property(e => e.EsperaAntesNovaTentativaMs)
            .HasColumnName("espera_antes_nova_tentativa_ms");

        entity.Property(e => e.EsperaEntreCaptchaEPostMs)
            .HasColumnName("espera_entre_captcha_e_post_ms");

        entity.Property(e => e.ExibirNasListagens)
            .HasColumnName("exibir_nas_listagens");

        entity.Property(e => e.ExtSup)
            .HasColumnName("ext_sup");

        entity.Property(e => e.ExternalId)
            .HasColumnName("external_id");

        entity.Property(e => e.FaturarPelasSubconsultas)
            .HasColumnName("faturar_pelas_subconsultas");

        entity.Property(e => e.FaturarResultNaoEncontrado)
            .HasColumnName("faturar_result_nao_encontrado");

        entity.Property(e => e.Fonte)
            .HasMaxLength(100)
            .HasColumnName("fonte");

        entity.Property(e => e.GerarPdf)
            .HasColumnName("gerar_pdf");

        entity.Property(e => e.GetTimeoutMs)
            .HasColumnName("get_timeout_ms");

        entity.Property(e => e.HabilitarLogDetalhado)
            .HasDefaultValue(false)
            .HasColumnName("habilitar_log_detalhado");

        entity.Property(e => e.IsDossier)
            .HasDefaultValue(false)
            .HasColumnName("is_dossier");

        entity.Property(e => e.LimiteSimultApi)
            .HasDefaultValue((short)100)
            .HasColumnName("limite_simult_api");

        entity.Property(e => e.LimiteSimultPorClienteApi)
            .HasDefaultValue((short)20)
            .HasColumnName("limite_simult_por_cliente_api");

        entity.Property(e => e.MaxSimultaneasLote)
            .HasColumnName("max_simultaneas_lote");

        entity.Property(e => e.MaxTentativas)
            .HasColumnName("max_tentativas");

        entity.Property(e => e.MaxTentativasLote)
            .HasColumnName("max_tentativas_lote");

        entity.Property(e => e.MotivoIndisponivel)
            .HasMaxLength(50)
            .HasColumnName("motivo_indisponivel");

        entity.Property(e => e.Nome)
            .HasColumnType("citext")
            .HasColumnName("nome");

        entity.Property(e => e.OmieSourceId)
            .HasColumnName("omie_source_id");

        entity.Property(e => e.OrdemExibicao)
            .HasColumnName("ordem_exibicao");

        entity.Property(e => e.PoolSessoesHabilitar)
            .HasDefaultValue(false)
            .HasColumnName("pool_sessoes_habilitar");

        entity.Property(e => e.PoolSessoesIntervRefreshMs)
            .HasColumnName("pool_sessoes_interv_refresh_ms");

        entity.Property(e => e.PoolSessoesQtdProdutores)
            .HasColumnName("pool_sessoes_qtd_produtores");

        entity.Property(e => e.PoolSessoesTamanho)
            .HasColumnName("pool_sessoes_tamanho");

        entity.Property(e => e.PoolSessoesTempoExpMs)
            .HasColumnName("pool_sessoes_tempo_exp_ms");

        entity.Property(e => e.PostTimeoutMs)
            .HasColumnName("post_timeout_ms");

        entity.Property(e => e.PriceInBrl)
            .HasColumnType("money")
            .HasColumnName("price_in_brl");

        entity.Property(e => e.ProxyForcar)
            .HasDefaultValue(false)
            .HasColumnName("proxy_forcar");

        entity.Property(e => e.ProxyUsar)
            .HasDefaultValue(true)
            .HasColumnName("proxy_usar");

        entity.Property(e => e.QtdExecucoesParalelas)
            .HasColumnName("qtd_execucoes_paralelas");

        entity.Property(e => e.QuantidadeMinSubconsultas)
            .HasColumnName("quantidade_min_subconsultas");

        entity.Property(e => e.RandomIpProxySelection)
            .HasColumnName("random_ip_proxy_selection");

        entity.Property(e => e.SessionPoolLastRequestUsingIp)
            .HasColumnName("session_pool_last_request_using_ip");

        entity.Property(e => e.SessionPoolPersistUsingDatastore)
            .HasColumnName("session_pool_persist_using_datastore");

        entity.Property(e => e.SupplierProductId)
            .HasColumnName("supplier_product_id");

        entity.Property(e => e.Visivel)
            .HasColumnName("visivel");
        
        entity.Property(e => e.ConsultaRelatorioTipoId)
            .HasColumnName("consulta_relatorio_tipo_id");

        entity.HasOne<CaptchaMecanismo>()
            .WithMany()
            .HasPrincipalKey(x => x.CaptchaMecanismoId)
            .HasForeignKey(d => d.CaptchaMecanismoId);

        entity.HasOne<ConsultaRelatorioTipo>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(d => d.ConsultaRelatorioTipoId);

        entity.HasIndex(e => e.ConsultaTipoId, "consulta_tipo_ak_id_name")
            .IncludeProperties(e => new { e.Nome, e.Visivel, e.Disponivel, e.ClienteLimiteMs })
            .IsUnique();

        entity.HasIndex(e => e.Nome, "consulta_tipo_ak_nome")
            .IsUnique();
    }
}
