using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class IntelligenceBootstrap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.EnsureSchema(
                name: "faturamento");

            migrationBuilder.EnsureSchema(
                name: "ibge");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,")
                .Annotation("Npgsql:PostgresExtension:pg_stat_statements", ",,")
                .Annotation("Npgsql:PostgresExtension:pg_trgm", ",,")
                .Annotation("Npgsql:PostgresExtension:pgcrypto", ",,")
                .Annotation("Npgsql:PostgresExtension:postgres_fdw", ",,")
                .Annotation("Npgsql:PostgresExtension:tablefunc", ",,")
                .Annotation("Npgsql:PostgresExtension:unaccent", ",,")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.RawSql("000BeforeIntelligenceBootstrap.sql");

            migrationBuilder.CreateSequence(
                name: "cliente_cliente_id_seq",
                schema: "public");

            migrationBuilder.CreateSequence(
                name: "consulta_consulta_id_seq",
                schema: "public");

            migrationBuilder.CreateSequence(
                name: "customers_plan_package_id_seq",
                schema: "faturamento");

            migrationBuilder.CreateSequence(
                name: "planos_faturamento_creditos_id_seq",
                schema: "faturamento");

            migrationBuilder.CreateSequence(
                name: "planos_faturamento_dossiers_id_seq",
                schema: "faturamento");

            migrationBuilder.CreateSequence(
                name: "token_acesso_consulta_tipo_token_acesso_consulta_tipo_id_seq",
                schema: "public");

            migrationBuilder.CreateSequence(
                name: "token_acesso_token_acesso_id_seq",
                schema: "public");

            migrationBuilder.CreateSequence(
                name: "transaction_exec_call_log_transaction_exec_call_log_id_seq",
                schema: "public");

            migrationBuilder.CreateSequence(
                name: "users_id_seq",
                schema: "public");

            migrationBuilder.CreateTable(
                name: "captcha_mecanismo",
                schema: "public",
                columns: table => new
                {
                    captcha_mecanismo_id = table.Column<short>(type: "smallint", nullable: false),
                    nome = table.Column<string>(type: "citext", nullable: false),
                    host = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    port = table.Column<int>(type: "integer", nullable: true),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    timeout_ms = table.Column<int>(type: "integer", nullable: true),
                    ativo = table.Column<bool>(type: "boolean", nullable: false),
                    utilizacao_quantidade = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    utilizacao_ultimo_data = table.Column<DateTime>(type: "timestamp", nullable: true),
                    resolvido_quantidade = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    resolvido_ultimo_data = table.Column<DateTime>(type: "timestamp", nullable: true),
                    sucesso_quantidade = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    incorreto_quantidade = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    nao_resolvido_quantidade = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    nao_resolvido_ultimo_data = table.Column<DateTime>(type: "timestamp", nullable: true),
                    tempo_medio_solucao_correta_ms = table.Column<int>(type: "integer", nullable: true),
                    alterado_em = table.Column<DateTime>(type: "timestamp", nullable: false),
                    alterado_por = table.Column<int>(type: "integer", nullable: false),
                    omie_source_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("captcha_mecanismo_pk", x => x.captcha_mecanismo_id);
                    table.CheckConstraint("captcha_mecanismo_nome_check", "char_length((nome)::text) <= 50");
                });

            migrationBuilder.CreateTable(
                name: "cnae_consolidado",
                schema: "ibge",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    secao = table.Column<string>(type: "text", nullable: false, collation: "C"),
                    divisao = table.Column<string>(type: "text", nullable: true, collation: "C"),
                    divisao_num = table.Column<short>(type: "smallint", nullable: true),
                    grupo = table.Column<string>(type: "text", nullable: true, collation: "C"),
                    grupo_num = table.Column<short>(type: "smallint", nullable: true),
                    classe = table.Column<string>(type: "text", nullable: true, collation: "C"),
                    classe_num = table.Column<int>(type: "integer", nullable: true),
                    subclasse = table.Column<string>(type: "text", nullable: true, collation: "C"),
                    subclasse_num = table.Column<int>(type: "integer", nullable: true),
                    denominacao = table.Column<string>(type: "text", nullable: false, collation: "C"),
                    versao = table.Column<decimal>(type: "numeric(2,1)", precision: 2, scale: 1, nullable: false),
                    controle_id = table.Column<short>(type: "smallint", nullable: false),
                    tipo = table.Column<string>(type: "text", nullable: false, collation: "C"),
                    segmento_quod = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cnae_consolidado", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "consulta",
                schema: "public",
                columns: table => new
                {
                    consulta_id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('public.consulta_consulta_id_seq')"),
                    consulta_master_id = table.Column<int>(type: "integer", nullable: true),
                    consulta_tipo_id = table.Column<short>(type: "smallint", nullable: false),
                    token_acesso_id = table.Column<int>(type: "integer", nullable: false),
                    consulta_resultado_tipo_id = table.Column<short>(type: "smallint", nullable: false),
                    consulta_reaproveitada_id = table.Column<int>(type: "integer", nullable: true),
                    origem_id = table.Column<short>(type: "smallint", nullable: false),
                    servidor_id = table.Column<short>(type: "smallint", nullable: true),
                    ip_saida_id = table.Column<short>(type: "smallint", nullable: true),
                    proxy_saida_id = table.Column<int>(type: "integer", nullable: true),
                    uid_base36 = table.Column<string>(type: "citext", nullable: false, collation: "C"),
                    caminho = table.Column<string>(type: "text", nullable: true),
                    ip_remoto = table.Column<int>(type: "integer", nullable: false),
                    acesso_negado = table.Column<bool>(type: "boolean", nullable: false),
                    inicio = table.Column<DateTime>(type: "timestamp", nullable: false),
                    fim = table.Column<DateTime>(type: "timestamp", nullable: true),
                    chave = table.Column<string>(type: "citext", nullable: false, collation: "C"),
                    entrada = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    retorno = table.Column<string>(type: "text", nullable: true),
                    retorno_tam = table.Column<int>(type: "integer", nullable: true),
                    retorno_serializado = table.Column<byte[]>(type: "bytea", nullable: true),
                    retorno_serializado_tam = table.Column<int>(type: "integer", nullable: true),
                    tentativas = table.Column<short>(type: "smallint", nullable: true),
                    captcha_recusados = table.Column<short>(type: "smallint", nullable: true),
                    captcha_falhas = table.Column<short>(type: "smallint", nullable: true),
                    captcha_tempo_decorrido_ms = table.Column<int>(type: "integer", nullable: true),
                    mensagem = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    erro_detalhe = table.Column<string>(type: "text", nullable: true),
                    erro_detalhe_tam = table.Column<int>(type: "integer", nullable: true),
                    retorno_original_compact = table.Column<byte[]>(type: "bytea", nullable: true),
                    retorno_original_compact_tam = table.Column<int>(type: "integer", nullable: true),
                    pdf_resultado_compact = table.Column<byte[]>(type: "bytea", nullable: true),
                    pdf_resultado_compact_tam = table.Column<int>(type: "integer", nullable: true),
                    faturavel = table.Column<bool>(type: "boolean", nullable: false),
                    armazenar_pdf = table.Column<bool>(type: "boolean", nullable: false),
                    custo_cred = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    custo_cred_pdf_geracao = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    custo_cred_pdf_armaz = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    custo_cred_total_inc_subcons = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    retorno_original_apagado = table.Column<bool>(type: "boolean", nullable: false),
                    retorno_desatualizado = table.Column<bool>(type: "boolean", nullable: false),
                    qtd_execucoes_paralelas = table.Column<short>(type: "smallint", nullable: true),
                    qtd_sub_consultas = table.Column<short>(type: "smallint", nullable: true),
                    qtd_sub_consultas_arvore = table.Column<short>(type: "smallint", nullable: true),
                    qtd_pdf_arvore = table.Column<short>(type: "smallint", nullable: true),
                    options = table.Column<string>(type: "text", nullable: true),
                    validation_results = table.Column<string>(type: "text", nullable: true),
                    hostname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    validation_result_risk_indicator = table.Column<short>(type: "smallint", nullable: true),
                    recencia = table.Column<DateTime>(type: "timestamp", nullable: true),
                    external_id = table.Column<Guid>(type: "uuid", nullable: true),
                    input_params = table.Column<string>(type: "jsonb", nullable: true),
                    result = table.Column<string>(type: "jsonb", nullable: true),
                    result_data = table.Column<string>(type: "jsonb", nullable: true),
                    remote_ip = table.Column<long>(type: "bigint", nullable: true),
                    master_uid = table.Column<string>(type: "citext", nullable: true),
                    transaction_cid = table.Column<string>(type: "citext", nullable: true),
                    async = table.Column<bool>(type: "boolean", nullable: true),
                    async_run_persistent = table.Column<bool>(type: "boolean", nullable: true),
                    async_attempts = table.Column<short>(type: "smallint", nullable: true),
                    async_child = table.Column<bool>(type: "boolean", nullable: true),
                    async_last_result_type_id = table.Column<short>(type: "smallint", nullable: true),
                    async_last_message = table.Column<string>(type: "text", nullable: true),
                    async_last_error = table.Column<string>(type: "text", nullable: true),
                    async_last_start = table.Column<DateTime>(type: "timestamp", nullable: true),
                    async_last_end = table.Column<DateTime>(type: "timestamp", nullable: true),
                    async_last_id = table.Column<int>(type: "integer", nullable: true),
                    async_last_uid = table.Column<string>(type: "citext", nullable: true),
                    cost_in_brl = table.Column<decimal>(type: "money", nullable: true),
                    result_profiler_json = table.Column<string>(type: "jsonb", nullable: true),
                    document = table.Column<string>(type: "citext", nullable: true, collation: "C"),
                    start_info_json = table.Column<string>(type: "jsonb", nullable: true),
                    cornerstone_api_call_init_bgcheck_uid = table.Column<Guid>(type: "uuid", nullable: true),
                    pdf_password_type = table.Column<short>(type: "smallint", nullable: true),
                    chatbot_product_request_uid = table.Column<Guid>(type: "uuid", nullable: true),
                    is_repeated_on_same_day_key_organization = table.Column<bool>(type: "boolean", nullable: true),
                    result_pdf_compressed_s3_action_id = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    result_pdf_compressed_s3_uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    original_source_data_compressed_s3_action_id = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    original_source_data_compressed_s3_uploaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    overridden_supplier_product_id = table.Column<short>(type: "smallint", nullable: true),
                    overridden_supplier_product_quantity = table.Column<short>(type: "smallint", nullable: true),
                    batch_run_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("consulta_pk", x => x.consulta_id);
                    table.CheckConstraint("consulta_uid_base36_check", "char_length((uid_base36)::text) <= 25");
                })
                .Annotation("Npgsql:StorageParameter:autovacuum_enabled", "true");

            migrationBuilder.CreateTable(
                name: "consulta_detalhe_tipo",
                schema: "public",
                columns: table => new
                {
                    consulta_detalhe_tipo_id = table.Column<short>(type: "smallint", nullable: false),
                    nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("consulta_detalhe_tipo_pk", x => x.consulta_detalhe_tipo_id);
                });

            migrationBuilder.CreateTable(
                name: "consulta_relatorio_tipo",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    tipo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("consulta_relatorio_tipo_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "consulta_resultado_tipo",
                schema: "public",
                columns: table => new
                {
                    consulta_resultado_tipo_id = table.Column<short>(type: "smallint", nullable: false),
                    nome = table.Column<string>(type: "citext", nullable: false),
                    definitivo = table.Column<bool>(type: "boolean", nullable: false),
                    faturavel = table.Column<bool>(type: "boolean", nullable: false),
                    erro = table.Column<bool>(type: "boolean", nullable: false),
                    gera_comprovante = table.Column<bool>(type: "boolean", nullable: false),
                    gera_registro_consulta = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("consulta_resultado_tipo_pk", x => x.consulta_resultado_tipo_id);
                    table.CheckConstraint("consulta_resultado_tipo_nome_check", "char_length((nome)::text) <= 80");
                });

            migrationBuilder.CreateTable(
                name: "data_source_profiler_config",
                schema: "public",
                columns: table => new
                {
                    data_source_id = table.Column<short>(type: "smallint", nullable: false),
                    profiler_enable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    measure_processor_time = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("data_source_profiler_config_pk", x => x.data_source_id);
                });

            migrationBuilder.CreateTable(
                name: "doccheck_token_settings",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    key_id = table.Column<Guid>(type: "uuid", nullable: false),
                    webhook_url = table.Column<string>(type: "text", nullable: true),
                    redirect_url = table.Column<string>(type: "text", nullable: true),
                    customization_settings_id = table.Column<int>(type: "integer", nullable: true),
                    action_if_minor = table.Column<string>(type: "text", nullable: true, defaultValueSql: "'Reject'::text"),
                    enable_js_post_message = table.Column<bool>(type: "boolean", nullable: true),
                    action_if_pep = table.Column<string>(type: "text", nullable: true, defaultValueSql: "'ManualValidation'::text"),
                    action_if_preape = table.Column<string>(type: "text", nullable: true, defaultValueSql: "'Reject'::text"),
                    action_if_listed_on_sanctions = table.Column<string>(type: "text", nullable: true, defaultValueSql: "'ManualValidation'::text"),
                    webhook_include_details_aml = table.Column<bool>(type: "boolean", nullable: true),
                    action_if_cpf_cancelled = table.Column<string>(type: "text", nullable: true, defaultValueSql: "'Reject'::text"),
                    allow_global_face_comparison = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    action_if_bolsa_familia = table.Column<string>(type: "text", nullable: true),
                    rejected_validation_to_manual_review = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    use_ocr_aws_fallback = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    action_if_detected_fraud = table.Column<string>(type: "text", nullable: true, defaultValueSql: "'ManualValidation'::text"),
                    redirect_to_digital_doc = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    action_if_age_not_in_estimated_range = table.Column<string>(type: "text", nullable: true),
                    replay_throw_error = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    max_match_level_for_rejection = table.Column<int>(type: "integer", nullable: true),
                    max_match_level_for_manual_review = table.Column<int>(type: "integer", nullable: true),
                    min_match_level = table.Column<int>(type: "integer", nullable: true),
                    status_when_min_match_not_met = table.Column<int>(type: "integer", nullable: true),
                    action_if_foreign_document = table.Column<string>(type: "text", nullable: true),
                    action_if_not_official_document = table.Column<string>(type: "text", nullable: true),
                    validation_limit_per_month = table.Column<int>(type: "integer", nullable: true),
                    force_manual_after_n_read_doc = table.Column<int>(type: "integer", nullable: true, defaultValue: 5),
                    custom_domain = table.Column<string>(type: "text", nullable: true),
                    custom_short_domain = table.Column<string>(type: "text", nullable: true),
                    cpfs_custom_domain = table.Column<List<string>>(type: "text[]", nullable: true),
                    use_exato_token = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    number_attempts_match3d3d = table.Column<int>(type: "integer", nullable: true, defaultValue: 5),
                    action_if_validation_not_in_brazil = table.Column<string>(type: "text", nullable: true),
                    force_status = table.Column<string>(type: "text", nullable: true),
                    force_status_enrollment = table.Column<string>(type: "text", nullable: true),
                    force_status_not_enrollment = table.Column<string>(type: "text", nullable: true),
                    use_aws_to_match3d3d = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    aws_liveness_percent_to_approve = table.Column<float>(type: "real", nullable: true, defaultValueSql: "70"),
                    aws_liveness_percent_to_reject = table.Column<float>(type: "real", nullable: true),
                    aws_match3d3d_similarity_percent_to_approve = table.Column<float>(type: "real", nullable: true),
                    aws_match3d3d_similarity_percent_to_reject = table.Column<float>(type: "real", nullable: true),
                    silent_flow = table.Column<bool>(type: "boolean", nullable: true),
                    action_if_preape_match_cpf = table.Column<string>(type: "text", nullable: true),
                    action_if_preape_match_probability_09 = table.Column<string>(type: "text", nullable: true),
                    action_if_preape_match_probability_08_or_less = table.Column<string>(type: "text", nullable: true),
                    minutes_to_expire_validation_link = table.Column<int>(type: "integer", nullable: true),
                    action_if_lawsuit_occurrences = table.Column<string>(type: "text", nullable: true),
                    action_if_google_news_occurrences = table.Column<string>(type: "text", nullable: true),
                    base_age_to_consider_minor = table.Column<int>(type: "integer", nullable: true),
                    ocr_supplier = table.Column<string>(type: "text", nullable: true),
                    use_ai_to_detect_texts_from_document = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("doccheck_token_settings_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "faturamento_tipo",
                schema: "public",
                columns: table => new
                {
                    faturamento_tipo_id = table.Column<short>(type: "smallint", nullable: false),
                    nome = table.Column<string>(type: "citext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("faturamento_tipo_pk", x => x.faturamento_tipo_id);
                    table.CheckConstraint("faturamento_tipo_nome_check", "char_length((nome)::text) <= 50");
                });

            migrationBuilder.CreateTable(
                name: "organization_segment",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "varchar", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("organization_segment_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "origem",
                schema: "public",
                columns: table => new
                {
                    origem_id = table.Column<short>(type: "smallint", nullable: false),
                    nome = table.Column<string>(type: "citext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("origem_pk", x => x.origem_id);
                    table.CheckConstraint("origem_nome_check", "char_length((nome)::text) <= 50");
                });

            migrationBuilder.CreateTable(
                name: "planos",
                schema: "faturamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descricao = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    data_inclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    data_alteracao = table.Column<DateTime>(type: "timestamp", nullable: true),
                    data_exclusao = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("planos_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "planos_creditos",
                schema: "faturamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('faturamento.planos_faturamento_creditos_id_seq')"),
                    nome = table.Column<string>(type: "citext", nullable: true),
                    zero_2k = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: true, defaultValueSql: "0.190"),
                    dois_5k = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: true, defaultValueSql: "0.162"),
                    cinco_10k = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: true, defaultValueSql: "0.141"),
                    dez_25k = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: true, defaultValueSql: "0.127"),
                    vinte_cinco_50k = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: true, defaultValueSql: "0.112"),
                    cinquenta_100k = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: true, defaultValueSql: "0.098"),
                    cem_250k = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: true, defaultValueSql: "0.084"),
                    duzentos_cinquenta_500k = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: true, defaultValueSql: "0.070"),
                    quinhentos_1m = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: true, defaultValueSql: "0.056"),
                    acima_1m = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: true, defaultValueSql: "0.049"),
                    is_default = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("planos_faturamento_creditos_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "planos_relatorios",
                schema: "faturamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('faturamento.planos_faturamento_dossiers_id_seq')"),
                    nome = table.Column<string>(type: "citext", nullable: true),
                    padrao = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("planos_faturamento_dossiers_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "realms",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false),
                    uid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "citext", nullable: false),
                    description = table.Column<string>(type: "citext", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("realms_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "servidor",
                schema: "public",
                columns: table => new
                {
                    servidor_id = table.Column<short>(type: "smallint", nullable: false),
                    nome_maquina = table.Column<string>(type: "citext", nullable: false),
                    ip_interno_entrada = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    processar_lotes = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("servidor_pk", x => x.servidor_id);
                });

            migrationBuilder.CreateTable(
                name: "token_acesso",
                schema: "public",
                columns: table => new
                {
                    token_acesso_id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('public.token_acesso_token_acesso_id_seq')"),
                    cliente_id = table.Column<int>(type: "integer", nullable: false),
                    usuario_id = table.Column<int>(type: "integer", nullable: true),
                    token = table.Column<string>(type: "citext", nullable: false),
                    acesso_total = table.Column<bool>(type: "boolean", nullable: false),
                    ips_autorizados = table.Column<string>(type: "text", nullable: true),
                    valido_ate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    incluido_em = table.Column<DateTime>(type: "timestamp", nullable: false),
                    incluido_por = table.Column<int>(type: "integer", nullable: false),
                    alterado_em = table.Column<DateTime>(type: "timestamp", nullable: true),
                    alterado_por = table.Column<int>(type: "integer", nullable: true),
                    excluido_em = table.Column<DateTime>(type: "timestamp", nullable: true),
                    excluido_por = table.Column<int>(type: "integer", nullable: true),
                    insert_transaction = table.Column<bool>(type: "boolean", nullable: true),
                    store_transaction_input = table.Column<bool>(type: "boolean", nullable: true),
                    store_transaction_return = table.Column<bool>(type: "boolean", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    key_id = table.Column<Guid>(type: "uuid", nullable: true),
                    secret_hash = table.Column<string>(type: "text", nullable: true),
                    key_type = table.Column<short>(type: "smallint", nullable: false),
                    trans_limit_per_hour = table.Column<int>(type: "integer", nullable: true),
                    trans_limit_per_day = table.Column<int>(type: "integer", nullable: true),
                    trans_limit_per_week = table.Column<int>(type: "integer", nullable: true),
                    trans_limit_per_month = table.Column<int>(type: "integer", nullable: true),
                    credits_limit_per_hour = table.Column<int>(type: "integer", nullable: true),
                    credits_limit_per_day = table.Column<int>(type: "integer", nullable: true),
                    credits_limit_per_week = table.Column<int>(type: "integer", nullable: true),
                    credits_limit_per_month = table.Column<int>(type: "integer", nullable: true),
                    currency_limit_per_hour = table.Column<decimal>(type: "numeric", nullable: true),
                    currency_limit_per_day = table.Column<decimal>(type: "numeric", nullable: true),
                    currency_limit_per_week = table.Column<decimal>(type: "numeric", nullable: true),
                    currency_limit_per_month = table.Column<decimal>(type: "numeric", nullable: true),
                    nf_group = table.Column<int>(type: "integer", nullable: true),
                    billing_instructions = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("token_acesso_pk", x => x.token_acesso_id);
                    table.CheckConstraint("token_acesso_token_check", "char_length((token)::text) <= 32");
                });

            migrationBuilder.CreateTable(
                name: "token_acesso_consulta_tipo",
                schema: "public",
                columns: table => new
                {
                    token_acesso_consulta_tipo_id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('public.token_acesso_consulta_tipo_token_acesso_consulta_tipo_id_seq')"),
                    token_acesso_id = table.Column<int>(type: "integer", nullable: true),
                    consulta_tipo_id = table.Column<short>(type: "smallint", nullable: true),
                    incluido_em = table.Column<DateTime>(type: "timestamp", nullable: false),
                    incluido_por = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("token_acesso_consulta_tipo_pk", x => x.token_acesso_consulta_tipo_id);
                });

            migrationBuilder.CreateTable(
                name: "token_acesso_quantidades",
                schema: "public",
                columns: table => new
                {
                    token_acesso_id = table.Column<int>(type: "integer", nullable: false),
                    day = table.Column<DateOnly>(type: "date", nullable: false),
                    hour = table.Column<TimeOnly>(type: "time", nullable: false),
                    trans_total = table.Column<int>(type: "integer", nullable: false),
                    credits_total = table.Column<int>(type: "integer", nullable: false),
                    currency_total = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("token_acesso_quantidades_pk", x => new { x.token_acesso_id, x.day, x.hour });
                });

            migrationBuilder.CreateTable(
                name: "token_criptografia",
                schema: "public",
                columns: table => new
                {
                    token_criptografia_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    cliente_id = table.Column<int>(type: "integer", nullable: false),
                    token = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    incluido_em = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    incluido_por = table.Column<int>(type: "integer", nullable: false),
                    alterado_em = table.Column<DateTime>(type: "timestamp", nullable: true),
                    alterado_por = table.Column<int>(type: "integer", nullable: true),
                    excluido_em = table.Column<DateTime>(type: "timestamp", nullable: true),
                    excluido_por = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("token_criptografia_pkey", x => x.token_criptografia_id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_exec_call_log",
                schema: "public",
                columns: table => new
                {
                    transaction_exec_call_log_id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('public.transaction_exec_call_log_transaction_exec_call_log_id_seq')"),
                    inserted_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    transaction_exec_call_log_uid = table.Column<Guid>(type: "uuid", nullable: false),
                    transaction_uid = table.Column<string>(type: "citext", nullable: false),
                    transaction_cid = table.Column<string>(type: "citext", nullable: true),
                    operation_method = table.Column<string>(type: "citext", nullable: true),
                    execution_mode = table.Column<string>(type: "citext", nullable: true),
                    data_source_type_id = table.Column<int>(type: "integer", nullable: true),
                    data_source_type_enum = table.Column<string>(type: "text", nullable: true),
                    identity_id = table.Column<int>(type: "integer", nullable: true),
                    identity_key_id = table.Column<Guid>(type: "uuid", nullable: true),
                    identity_token_str = table.Column<string>(type: "citext", nullable: true),
                    remote_addr = table.Column<IPAddress>(type: "inet", nullable: true),
                    server_instance = table.Column<string>(type: "citext", nullable: true),
                    load_balancer_trace_header = table.Column<string>(type: "text", nullable: true),
                    load_balancer_trace_field = table.Column<string>(type: "text", nullable: true),
                    load_balancer_trace_id = table.Column<string>(type: "citext", nullable: true),
                    http_method = table.Column<string>(type: "citext", nullable: true),
                    http_uri = table.Column<string>(type: "text", nullable: true),
                    http_host = table.Column<string>(type: "text", nullable: true),
                    http_headers = table.Column<string[]>(type: "text[]", nullable: true),
                    http_body_text = table.Column<string>(type: "text", nullable: true),
                    http_body_raw = table.Column<byte[]>(type: "bytea", nullable: true),
                    event_counters_info = table.Column<string>(type: "jsonb", nullable: true),
                    additional_info = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("transaction_exec_call_log_pk", x => new { x.transaction_exec_call_log_id, x.inserted_on_utc });
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('public.users_id_seq')"),
                    email = table.Column<string>(type: "varchar", nullable: true, defaultValueSql: "''"),
                    encrypted_password = table.Column<string>(type: "varchar", nullable: false, defaultValueSql: "''"),
                    reset_password_token = table.Column<string>(type: "varchar", nullable: true),
                    reset_password_sent_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    remember_created_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    sign_in_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    current_sign_in_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    last_sign_in_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    current_sign_in_ip = table.Column<IPAddress>(type: "inet", nullable: true),
                    last_sign_in_ip = table.Column<IPAddress>(type: "inet", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    provider = table.Column<string>(type: "varchar", nullable: true),
                    uid = table.Column<string>(type: "varchar", nullable: true),
                    invitation_token = table.Column<string>(type: "varchar", nullable: true),
                    invitation_created_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    invitation_sent_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    invitation_accepted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    invitation_limit = table.Column<int>(type: "integer", nullable: true),
                    invited_by_id = table.Column<int>(type: "integer", nullable: true),
                    invited_by_type = table.Column<string>(type: "varchar", nullable: true),
                    invitations_count = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    full_name = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: true),
                    cliente_id = table.Column<int>(type: "integer", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    @internal = table.Column<bool>(name: "internal", type: "boolean", nullable: false),
                    visible = table.Column<bool>(type: "boolean", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_by = table.Column<int>(type: "integer", nullable: true),
                    external_id = table.Column<Guid>(type: "uuid", nullable: true),
                    cpf = table.Column<long>(type: "bigint", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    updated_by = table.Column<int>(type: "integer", nullable: true),
                    realm_id = table.Column<short>(type: "smallint", nullable: false),
                    origem_id = table.Column<short>(type: "smallint", nullable: true),
                    migrated_at = table.Column<DateOnly>(type: "date", nullable: true),
                    migrated_to_user_external_id = table.Column<Guid>(type: "uuid", nullable: true),
                    origin = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "consulta_tipo",
                schema: "public",
                columns: table => new
                {
                    consulta_tipo_id = table.Column<short>(type: "smallint", nullable: false),
                    captcha_mecanismo_id = table.Column<short>(type: "smallint", nullable: true),
                    nome = table.Column<string>(type: "citext", nullable: false),
                    fonte = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    quantidade_min_subconsultas = table.Column<short>(type: "smallint", nullable: false),
                    faturar_pelas_subconsultas = table.Column<bool>(type: "boolean", nullable: false),
                    faturar_result_nao_encontrado = table.Column<bool>(type: "boolean", nullable: false),
                    custo_cred = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    custo_cred_pdf_geracao = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    custo_cred_pdf_armaz = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    visivel = table.Column<bool>(type: "boolean", nullable: false),
                    disponivel = table.Column<bool>(type: "boolean", nullable: false),
                    motivo_indisponivel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    cliente_limite_ms = table.Column<int>(type: "integer", nullable: true),
                    proxy_usar = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    proxy_forcar = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    alternar_ips_e_proxies = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    block_threads_during_ip_proxy_selection = table.Column<bool>(type: "boolean", nullable: true),
                    random_ip_proxy_selection = table.Column<bool>(type: "boolean", nullable: true),
                    bloqueio_ip_timeout_qtd = table.Column<short>(type: "smallint", nullable: true),
                    bloqueio_ip_timeout_tempo_ms = table.Column<int>(type: "integer", nullable: true),
                    bloqueio_ip_erro_qtd = table.Column<short>(type: "smallint", nullable: true),
                    bloqueio_ip_erro_tempo_ms = table.Column<int>(type: "integer", nullable: true),
                    bloqueio_proxy_timeout_qtd = table.Column<short>(type: "smallint", nullable: true),
                    bloqueio_proxy_timeout_tempo_ms = table.Column<int>(type: "integer", nullable: true),
                    bloqueio_proxy_erro_qtd = table.Column<short>(type: "smallint", nullable: true),
                    bloqueio_proxy_erro_tempo_ms = table.Column<int>(type: "integer", nullable: true),
                    get_timeout_ms = table.Column<int>(type: "integer", nullable: true),
                    captcha_timeout_ms = table.Column<int>(type: "integer", nullable: true),
                    post_timeout_ms = table.Column<int>(type: "integer", nullable: true),
                    espera_entre_captcha_e_post_ms = table.Column<int>(type: "integer", nullable: true),
                    espera_antes_nova_tentativa_ms = table.Column<int>(type: "integer", nullable: true),
                    max_tentativas = table.Column<short>(type: "smallint", nullable: true),
                    duracao_cache_hrs = table.Column<short>(type: "smallint", nullable: true),
                    max_simultaneas_lote = table.Column<short>(type: "smallint", nullable: true),
                    max_tentativas_lote = table.Column<short>(type: "smallint", nullable: true),
                    email_entrada_consultas = table.Column<string>(type: "citext", nullable: true),
                    habilitar_log_detalhado = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    gerar_pdf = table.Column<bool>(type: "boolean", nullable: true),
                    armazenar_pdf = table.Column<bool>(type: "boolean", nullable: true),
                    limite_simult_api = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)100),
                    limite_simult_por_cliente_api = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)20),
                    qtd_execucoes_paralelas = table.Column<short>(type: "smallint", nullable: true),
                    pool_sessoes_habilitar = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    pool_sessoes_tamanho = table.Column<short>(type: "smallint", nullable: true),
                    pool_sessoes_tempo_exp_ms = table.Column<int>(type: "integer", nullable: true),
                    pool_sessoes_qtd_produtores = table.Column<short>(type: "smallint", nullable: true),
                    pool_sessoes_interv_refresh_ms = table.Column<int>(type: "integer", nullable: true),
                    session_pool_last_request_using_ip = table.Column<bool>(type: "boolean", nullable: true),
                    session_pool_persist_using_datastore = table.Column<bool>(type: "boolean", nullable: true),
                    captcha_otimizar_taxa_recusados = table.Column<bool>(type: "boolean", nullable: true),
                    ordem_exibicao = table.Column<short>(type: "smallint", nullable: true),
                    exibir_nas_listagens = table.Column<bool>(type: "boolean", nullable: true),
                    alterado_em = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    alterado_por = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    allow_batch_proc = table.Column<bool>(type: "boolean", nullable: true),
                    external_id = table.Column<Guid>(type: "uuid", nullable: true),
                    price_in_brl = table.Column<decimal>(type: "money", nullable: true),
                    is_dossier = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    omie_source_id = table.Column<long>(type: "bigint", nullable: true),
                    ext_sup = table.Column<bool>(type: "boolean", nullable: true),
                    supplier_product_id = table.Column<short>(type: "smallint", nullable: true),
                    consulta_relatorio_tipo_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("consulta_tipo_pk", x => x.consulta_tipo_id);
                    table.CheckConstraint("consulta_tipo_email_entrada_consultas_check", "char_length((email_entrada_consultas)::text) <= 80");
                    table.CheckConstraint("consulta_tipo_nome_check", "char_length((nome)::text) <= 80");
                    table.ForeignKey(
                        name: "fk_consulta_tipo_captcha_mecanismo_captcha_mecanismo_id",
                        column: x => x.captcha_mecanismo_id,
                        principalSchema: "public",
                        principalTable: "captcha_mecanismo",
                        principalColumn: "captcha_mecanismo_id");
                    table.ForeignKey(
                        name: "fk_consulta_tipo_consulta_relatorio_tipo_consulta_relatorio_ti",
                        column: x => x.consulta_relatorio_tipo_id,
                        principalSchema: "public",
                        principalTable: "consulta_relatorio_tipo",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "precificacao",
                schema: "faturamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    plano_id = table.Column<int>(type: "integer", nullable: false),
                    consulta_tipo_id = table.Column<int>(type: "integer", nullable: false),
                    faixas_id = table.Column<int>(type: "integer", nullable: false),
                    inicio_faixa = table.Column<int>(type: "integer", nullable: false),
                    fim_faixa = table.Column<int>(type: "integer", nullable: true),
                    valor_unitario = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("precificacao_pkey", x => x.id);
                    table.CheckConstraint("precificacao_check", "(fim_faixa IS NULL) OR (fim_faixa > inicio_faixa)");
                    table.ForeignKey(
                        name: "fk_precificacao_planos_plano_id",
                        column: x => x.plano_id,
                        principalSchema: "faturamento",
                        principalTable: "planos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cliente",
                schema: "public",
                columns: table => new
                {
                    cliente_id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('public.cliente_cliente_id_seq')"),
                    faturamento_tipo_id = table.Column<short>(type: "smallint", nullable: true),
                    preco_id = table.Column<short>(type: "smallint", nullable: true),
                    nome = table.Column<string>(type: "citext", nullable: false),
                    segundo_nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    cpf_cnpj = table.Column<long>(type: "bigint", nullable: true),
                    gerar_pdf_consultas = table.Column<bool>(type: "boolean", nullable: false),
                    armazenar_pdf_consultas = table.Column<bool>(type: "boolean", nullable: false),
                    habilitar_consultas_por_email = table.Column<bool>(type: "boolean", nullable: false),
                    interno = table.Column<bool>(type: "boolean", nullable: false),
                    pessoa_fisica = table.Column<bool>(type: "boolean", nullable: false),
                    endereco = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    numero = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    complemento = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    bairro = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    cidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    uf = table.Column<string>(type: "character(2)", fixedLength: true, maxLength: 2, nullable: true),
                    cep = table.Column<int>(type: "integer", nullable: true),
                    comentarios = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    origem = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ativo = table.Column<bool>(type: "boolean", nullable: false),
                    saldo = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    result_desat_limite_hrs = table.Column<short>(type: "smallint", nullable: true),
                    cliente_em_teste = table.Column<bool>(type: "boolean", nullable: false),
                    incluido_em = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    incluido_por = table.Column<int>(type: "integer", nullable: false),
                    alterado_em = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    alterado_por = table.Column<int>(type: "integer", nullable: true),
                    excluido_em = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    excluido_por = table.Column<int>(type: "integer", nullable: true),
                    store_transaction_input = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    store_transaction_return = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    balance_in_brl = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true, defaultValueSql: "0"),
                    balance_type = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)2),
                    external_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    data_access_level = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    unauthorized_datasources = table.Column<int[]>(type: "integer[]", nullable: true),
                    quod_successfully_enrollment_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    quod_last_enrollment = table.Column<string>(type: "jsonb", nullable: true),
                    trans_limit_per_hour = table.Column<int>(type: "integer", nullable: true),
                    trans_limit_per_day = table.Column<int>(type: "integer", nullable: true),
                    trans_limit_per_week = table.Column<int>(type: "integer", nullable: true),
                    trans_limit_per_month = table.Column<int>(type: "integer", nullable: true),
                    credits_limit_per_hour = table.Column<int>(type: "integer", nullable: true),
                    credits_limit_per_day = table.Column<int>(type: "integer", nullable: true),
                    credits_limit_per_week = table.Column<int>(type: "integer", nullable: true),
                    credits_limit_per_month = table.Column<int>(type: "integer", nullable: true),
                    currency_limit_per_hour = table.Column<decimal>(type: "numeric", nullable: true),
                    currency_limit_per_day = table.Column<decimal>(type: "numeric", nullable: true),
                    currency_limit_per_week = table.Column<decimal>(type: "numeric", nullable: true),
                    currency_limit_per_month = table.Column<decimal>(type: "numeric", nullable: true),
                    quod_segment_id = table.Column<int>(type: "integer", nullable: true),
                    razao_social_rf = table.Column<string>(type: "citext", nullable: true),
                    nome_fantasia_rf = table.Column<string>(type: "citext", nullable: true),
                    realm_id = table.Column<short>(type: "smallint", nullable: false),
                    pdf_password = table.Column<string>(type: "text", nullable: true),
                    migrated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    migrated_to_cliente_external_id = table.Column<Guid>(type: "uuid", nullable: true),
                    origin = table.Column<string>(type: "text", nullable: true),
                    balance_updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_credit_purchase_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    external_display_name = table.Column<string>(type: "text", nullable: true),
                    slug = table.Column<string>(type: "text", nullable: true),
                    parent_organization_id = table.Column<int>(type: "integer", nullable: true),
                    high_performance = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    receita_cpf_use_serpro_as_main_source = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    receita_cpf_need_pdf_proof = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    receita_cpf_should_return_minor18_age_data = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    partner_id = table.Column<int>(type: "integer", nullable: true),
                    organization_segment_id = table.Column<int>(type: "integer", nullable: true),
                    crm_client_id = table.Column<long>(type: "bigint", nullable: true),
                    exato_sales_contact = table.Column<string>(type: "text", nullable: true),
                    invoice_slug = table.Column<string>(type: "text", nullable: true),
                    use_ocr_exato = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    quod_customer_external_id = table.Column<Guid>(type: "uuid", nullable: true),
                    use_serpro_data_valid_facial = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    use_doccheck = table.Column<bool>(type: "boolean", nullable: true),
                    dossier_id_to_execute_pf = table.Column<int>(type: "integer", nullable: true, defaultValue: 5040),
                    dossier_id_to_execute_pj = table.Column<int>(type: "integer", nullable: true, defaultValue: 5045),
                    dossier_id_to_execute_pf_credit_analysis = table.Column<int>(type: "integer", nullable: true, defaultValue: 5041),
                    dossier_id_to_execute_pj_credit_analysis = table.Column<int>(type: "integer", nullable: true, defaultValue: 5046),
                    doccheck_use_enrollment_by_organization = table.Column<bool>(type: "boolean", nullable: true),
                    billing_id = table.Column<long>(type: "bigint", nullable: true),
                    is_billing_customer = table.Column<bool>(type: "boolean", nullable: true),
                    block_sensitive_data_in_query_string = table.Column<bool>(type: "boolean", nullable: true),
                    billing_instructions = table.Column<string>(type: "jsonb", nullable: true),
                    public_id = table.Column<string>(type: "character(14)", fixedLength: true, maxLength: 14, nullable: true, defaultValueSql: "generate_base36_id()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("cliente_pk", x => x.cliente_id);
                    table.ForeignKey(
                        name: "Reffaturamento_tipo72",
                        column: x => x.faturamento_tipo_id,
                        principalSchema: "public",
                        principalTable: "faturamento_tipo",
                        principalColumn: "faturamento_tipo_id");
                    table.ForeignKey(
                        name: "fk_cliente_public_realms_realm_id",
                        column: x => x.realm_id,
                        principalSchema: "public",
                        principalTable: "realms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_organization_segment_id",
                        column: x => x.organization_segment_id,
                        principalSchema: "public",
                        principalTable: "organization_segment",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_parent_organization_id",
                        column: x => x.parent_organization_id,
                        principalSchema: "public",
                        principalTable: "cliente",
                        principalColumn: "cliente_id");
                });

            migrationBuilder.CreateTable(
                name: "cliente_contact",
                schema: "faturamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    cliente_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    inserted_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    additional_data = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("cliente_contact_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_cliente_contact_cliente_cliente_id",
                        column: x => x.cliente_id,
                        principalSchema: "public",
                        principalTable: "cliente",
                        principalColumn: "cliente_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cliente_planos_relatorios",
                schema: "faturamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    cliente_id = table.Column<int>(type: "integer", nullable: false),
                    plano_id = table.Column<int>(type: "integer", nullable: false),
                    ativo = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    data_atribuicao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    data_desativacao = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("cliente_planos_relatorios_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_cliente_planos_relatorios_cliente_cliente_id",
                        column: x => x.cliente_id,
                        principalSchema: "public",
                        principalTable: "cliente",
                        principalColumn: "cliente_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_cliente_planos_relatorios_plano_plano_id",
                        column: x => x.plano_id,
                        principalSchema: "faturamento",
                        principalTable: "planos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "planos_clientes",
                schema: "faturamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('faturamento.customers_plan_package_id_seq')"),
                    cliente_id = table.Column<int>(type: "integer", nullable: true),
                    planos_relatorios_id = table.Column<int>(type: "integer", nullable: true),
                    planos_creditos_id = table.Column<int>(type: "integer", nullable: true),
                    franquia_minima = table.Column<int>(type: "integer", nullable: true, defaultValue: 495),
                    faturamento_por_faixa = table.Column<bool>(type: "boolean", nullable: false),
                    planos_doccheck_id = table.Column<int>(type: "integer", nullable: true),
                    faturamento_por_rateio = table.Column<bool>(type: "boolean", nullable: true),
                    detalhar_relatorios = table.Column<bool>(type: "boolean", nullable: true),
                    exibir_nao_consumidores = table.Column<bool>(type: "boolean", nullable: true),
                    cliente_contact_id = table.Column<int>(type: "integer", nullable: true),
                    previous_customer = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    unmasked_customer = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    summary_customer = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    v1_customer = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("customers_plan_package_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_planos_clientes_cliente_cliente_id",
                        column: x => x.cliente_id,
                        principalSchema: "public",
                        principalTable: "cliente",
                        principalColumn: "cliente_id");
                    table.ForeignKey(
                        name: "fk_planos_clientes_cliente_contact_cliente_contact_id",
                        column: x => x.cliente_contact_id,
                        principalSchema: "faturamento",
                        principalTable: "cliente_contact",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_planos_clientes_planos_credito_planos_creditos_id",
                        column: x => x.planos_creditos_id,
                        principalSchema: "faturamento",
                        principalTable: "planos_creditos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_planos_clientes_planos_relatorio_planos_relatorios_id",
                        column: x => x.planos_relatorios_id,
                        principalSchema: "faturamento",
                        principalTable: "planos_relatorios",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_captcha_mecanismo_nome",
                schema: "public",
                table: "captcha_mecanismo",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "cliente_ak_external_id",
                schema: "public",
                table: "cliente",
                column: "external_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "cliente_ak_raiz",
                schema: "public",
                table: "cliente",
                column: "cliente_id",
                unique: true,
                filter: "(parent_organization_id IS NULL)")
                .Annotation("Npgsql:IndexInclude", new[] { "external_id", "nome", "cpf_cnpj", "pessoa_fisica" });

            migrationBuilder.CreateIndex(
                name: "ix_cliente_faturamento_tipo_id",
                schema: "public",
                table: "cliente",
                column: "faturamento_tipo_id");

            migrationBuilder.CreateIndex(
                name: "ix_cliente_organization_segment_id",
                schema: "public",
                table: "cliente",
                column: "organization_segment_id");

            migrationBuilder.CreateIndex(
                name: "ix_cliente_parent_organization_id",
                schema: "public",
                table: "cliente",
                column: "parent_organization_id");

            migrationBuilder.CreateIndex(
                name: "ix_cliente_realm_id",
                schema: "public",
                table: "cliente",
                column: "realm_id");

            migrationBuilder.CreateIndex(
                name: "ix_cliente_contact_cliente_id",
                schema: "faturamento",
                table: "cliente_contact",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "ix_cliente_planos_relatorios_cliente_id_plano_id",
                schema: "faturamento",
                table: "cliente_planos_relatorios",
                columns: new[] { "cliente_id", "plano_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_cliente_planos_relatorios_plano_id",
                schema: "faturamento",
                table: "cliente_planos_relatorios",
                column: "plano_id");

            migrationBuilder.CreateIndex(
                name: "ix_consulta_transaction_cid",
                schema: "public",
                table: "consulta",
                column: "transaction_cid",
                unique: true,
                filter: "(transaction_cid IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "ix_consulta_uid_base36",
                schema: "public",
                table: "consulta",
                column: "uid_base36",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "consulta_id" })
                .Annotation("Relational:Collation", new[] { "C" });

            migrationBuilder.CreateIndex(
                name: "ix_consulta_detalhe_tipo_nome",
                schema: "public",
                table: "consulta_detalhe_tipo",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_consulta_resultado_tipo_nome",
                schema: "public",
                table: "consulta_resultado_tipo",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_consulta_tipo_captcha_mecanismo_id",
                schema: "public",
                table: "consulta_tipo",
                column: "captcha_mecanismo_id");

            migrationBuilder.CreateIndex(
                name: "ix_consulta_tipo_consulta_relatorio_tipo_id",
                schema: "public",
                table: "consulta_tipo",
                column: "consulta_relatorio_tipo_id");

            migrationBuilder.CreateIndex(
                name: "ix_consulta_tipo_consulta_tipo_id",
                schema: "public",
                table: "consulta_tipo",
                column: "consulta_tipo_id",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "nome", "visivel", "disponivel", "cliente_limite_ms" });

            migrationBuilder.CreateIndex(
                name: "ix_consulta_tipo_nome",
                schema: "public",
                table: "consulta_tipo",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "doccheck_token_settings_ie_key_id",
                schema: "public",
                table: "doccheck_token_settings",
                column: "key_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_faturamento_tipo_nome",
                schema: "public",
                table: "faturamento_tipo",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "origem_ak_nome",
                schema: "public",
                table: "origem",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_planos_clientes_cliente_contact_id",
                schema: "faturamento",
                table: "planos_clientes",
                column: "cliente_contact_id");

            migrationBuilder.CreateIndex(
                name: "ix_planos_clientes_cliente_id",
                schema: "faturamento",
                table: "planos_clientes",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "ix_planos_clientes_planos_creditos_id",
                schema: "faturamento",
                table: "planos_clientes",
                column: "planos_creditos_id");

            migrationBuilder.CreateIndex(
                name: "ix_planos_clientes_planos_relatorios_id",
                schema: "faturamento",
                table: "planos_clientes",
                column: "planos_relatorios_id");

            migrationBuilder.CreateIndex(
                name: "ix_precificacao_plano_id_consulta_tipo_id_faixas_id_valor_unit",
                schema: "faturamento",
                table: "precificacao",
                columns: new[] { "plano_id", "consulta_tipo_id", "faixas_id", "valor_unitario" },
                unique: true);

            migrationBuilder.RawSql("001AfterIntelligenceBootstrap.sql");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cliente_planos_relatorios",
                schema: "faturamento");

            migrationBuilder.DropTable(
                name: "cnae_consolidado",
                schema: "ibge");

            migrationBuilder.DropTable(
                name: "consulta",
                schema: "public");

            migrationBuilder.DropTable(
                name: "consulta_detalhe_tipo",
                schema: "public");

            migrationBuilder.DropTable(
                name: "consulta_resultado_tipo",
                schema: "public");

            migrationBuilder.DropTable(
                name: "consulta_tipo",
                schema: "public");

            migrationBuilder.DropTable(
                name: "data_source_profiler_config",
                schema: "public");

            migrationBuilder.DropTable(
                name: "doccheck_token_settings",
                schema: "public");

            migrationBuilder.DropTable(
                name: "origem",
                schema: "public");

            migrationBuilder.DropTable(
                name: "planos_clientes",
                schema: "faturamento");

            migrationBuilder.DropTable(
                name: "precificacao",
                schema: "faturamento");

            migrationBuilder.DropTable(
                name: "servidor",
                schema: "public");

            migrationBuilder.DropTable(
                name: "token_acesso",
                schema: "public");

            migrationBuilder.DropTable(
                name: "token_acesso_consulta_tipo",
                schema: "public");

            migrationBuilder.DropTable(
                name: "token_acesso_quantidades",
                schema: "public");

            migrationBuilder.DropTable(
                name: "token_criptografia",
                schema: "public");

            migrationBuilder.DropTable(
                name: "transaction_exec_call_log",
                schema: "public");

            migrationBuilder.DropTable(
                name: "users",
                schema: "public");

            migrationBuilder.DropTable(
                name: "captcha_mecanismo",
                schema: "public");

            migrationBuilder.DropTable(
                name: "consulta_relatorio_tipo",
                schema: "public");

            migrationBuilder.DropTable(
                name: "cliente_contact",
                schema: "faturamento");

            migrationBuilder.DropTable(
                name: "planos_creditos",
                schema: "faturamento");

            migrationBuilder.DropTable(
                name: "planos_relatorios",
                schema: "faturamento");

            migrationBuilder.DropTable(
                name: "planos",
                schema: "faturamento");

            migrationBuilder.DropTable(
                name: "cliente",
                schema: "public");

            migrationBuilder.DropTable(
                name: "faturamento_tipo",
                schema: "public");

            migrationBuilder.DropTable(
                name: "realms",
                schema: "public");

            migrationBuilder.DropTable(
                name: "organization_segment",
                schema: "public");

            migrationBuilder.DropSequence(
                name: "cliente_cliente_id_seq",
                schema: "public");

            migrationBuilder.DropSequence(
                name: "consulta_consulta_id_seq",
                schema: "public");

            migrationBuilder.DropSequence(
                name: "customers_plan_package_id_seq",
                schema: "faturamento");

            migrationBuilder.DropSequence(
                name: "planos_faturamento_creditos_id_seq",
                schema: "faturamento");

            migrationBuilder.DropSequence(
                name: "planos_faturamento_dossiers_id_seq",
                schema: "faturamento");

            migrationBuilder.DropSequence(
                name: "token_acesso_consulta_tipo_token_acesso_consulta_tipo_id_seq",
                schema: "public");

            migrationBuilder.DropSequence(
                name: "token_acesso_token_acesso_id_seq",
                schema: "public");

            migrationBuilder.DropSequence(
                name: "transaction_exec_call_log_transaction_exec_call_log_id_seq",
                schema: "public");

            migrationBuilder.DropSequence(
                name: "users_id_seq",
                schema: "public");
        }
    }
}
