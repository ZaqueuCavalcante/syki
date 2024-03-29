using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "syki");

            migrationBuilder.CreateTable(
                name: "faculdades",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_faculdades", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    data = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tasks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_registers",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    trial_start = table.Column<DateOnly>(type: "date", nullable: true),
                    trial_end = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_registers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "academic_periods",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start = table.Column<DateOnly>(type: "date", nullable: false),
                    end = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_academic_periods", x => new { x.id, x.institution_id });
                    table.ForeignKey(
                        name: "fk_academic_periods_institutions_faculdade_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "faculdades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "campi",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_campi", x => x.id);
                    table.ForeignKey(
                        name: "fk_campi_institutions_faculdade_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "faculdades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cursos",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    faculdade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "text", nullable: false),
                    tipo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cursos", x => x.id);
                    table.ForeignKey(
                        name: "fk_cursos_institutions_faculdade_id",
                        column: x => x.faculdade_id,
                        principalSchema: "syki",
                        principalTable: "faculdades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "disciplinas",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    faculdade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_disciplinas", x => x.id);
                    table.ForeignKey(
                        name: "fk_disciplinas_institutions_faculdade_id",
                        column: x => x.faculdade_id,
                        principalSchema: "syki",
                        principalTable: "faculdades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    faculdade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_notifications_institutions_faculdade_id",
                        column: x => x.faculdade_id,
                        principalSchema: "syki",
                        principalTable: "faculdades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.UniqueConstraint("ak_asp_net_users_institution_id_id", x => new { x.institution_id, x.id });
                    table.ForeignKey(
                        name: "fk_users_faculdades_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "faculdades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_claims",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_role_claims_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "syki",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "enrollment_periods",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start = table.Column<DateOnly>(type: "date", nullable: false),
                    end = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_enrollment_periods", x => new { x.id, x.institution_id });
                    table.ForeignKey(
                        name: "fk_enrollment_periods_academic_periods_academic_period_id_acad",
                        columns: x => new { x.id, x.institution_id },
                        principalSchema: "syki",
                        principalTable: "academic_periods",
                        principalColumns: new[] { "id", "institution_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grades",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    faculdade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    curso_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grades", x => x.id);
                    table.ForeignKey(
                        name: "fk_grades_cursos_curso_id",
                        column: x => x.curso_id,
                        principalSchema: "syki",
                        principalTable: "cursos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_grades_institutions_faculdade_id",
                        column: x => x.faculdade_id,
                        principalSchema: "syki",
                        principalTable: "faculdades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cursos__disciplinas",
                schema: "syki",
                columns: table => new
                {
                    curso_id = table.Column<Guid>(type: "uuid", nullable: false),
                    disciplina_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cursos__disciplinas", x => new { x.curso_id, x.disciplina_id });
                    table.ForeignKey(
                        name: "fk_cursos__disciplinas_cursos_curso_id",
                        column: x => x.curso_id,
                        principalSchema: "syki",
                        principalTable: "cursos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_cursos__disciplinas_disciplinas_disciplina_id",
                        column: x => x.disciplina_id,
                        principalSchema: "syki",
                        principalTable: "disciplinas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audit_logs",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_type = table.Column<string>(type: "text", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    faculdade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    data = table.Column<JsonDocument>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_audit_logs_asp_net_users_syki_user_id",
                        columns: x => new { x.faculdade_id, x.user_id },
                        principalSchema: "syki",
                        principalTable: "users",
                        principalColumns: new[] { "institution_id", "id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "professores",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    faculdade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_professores", x => x.id);
                    table.ForeignKey(
                        name: "fk_professores_asp_net_users_syki_user_id",
                        columns: x => new { x.faculdade_id, x.id },
                        principalSchema: "syki",
                        principalTable: "users",
                        principalColumns: new[] { "institution_id", "id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_professores_institutions_faculdade_id",
                        column: x => x.faculdade_id,
                        principalSchema: "syki",
                        principalTable: "faculdades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reset_password_tokens",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    used_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reset_password_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_reset_password_tokens_asp_net_users_syki_user_id",
                        column: x => x.user_id,
                        principalSchema: "syki",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_claims",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_claims_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "syki",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_logins",
                schema: "syki",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    provider_key = table.Column<string>(type: "text", nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_logins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_user_logins_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "syki",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                schema: "syki",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_user_roles_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "syki",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "syki",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_tokens",
                schema: "syki",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_user_tokens_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "syki",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users__notifications",
                schema: "syki",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    notification_id = table.Column<Guid>(type: "uuid", nullable: false),
                    viewed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users__notifications", x => new { x.user_id, x.notification_id });
                    table.ForeignKey(
                        name: "fk_users__notifications_asp_net_users_syki_user_id",
                        column: x => x.user_id,
                        principalSchema: "syki",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_users__notifications_notifications_notification_id",
                        column: x => x.notification_id,
                        principalSchema: "syki",
                        principalTable: "notifications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grades__disciplinas",
                schema: "syki",
                columns: table => new
                {
                    grade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    disciplina_id = table.Column<Guid>(type: "uuid", nullable: false),
                    periodo = table.Column<byte>(type: "smallint", nullable: false),
                    creditos = table.Column<byte>(type: "smallint", nullable: false),
                    carga_horaria = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grades__disciplinas", x => new { x.disciplina_id, x.grade_id });
                    table.ForeignKey(
                        name: "fk_grades__disciplinas_disciplinas_disciplina_id",
                        column: x => x.disciplina_id,
                        principalSchema: "syki",
                        principalTable: "disciplinas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_grades__disciplinas_grades_grade_id",
                        column: x => x.grade_id,
                        principalSchema: "syki",
                        principalTable: "grades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ofertas",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    faculdade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    curso_id = table.Column<Guid>(type: "uuid", nullable: false),
                    grade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    periodo = table.Column<string>(type: "text", nullable: false),
                    turno = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ofertas", x => x.id);
                    table.ForeignKey(
                        name: "fk_ofertas_academic_periods_academic_period_id_academic_period",
                        columns: x => new { x.periodo, x.faculdade_id },
                        principalSchema: "syki",
                        principalTable: "academic_periods",
                        principalColumns: new[] { "id", "institution_id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ofertas_campi_campus_temp_id",
                        column: x => x.campus_id,
                        principalSchema: "syki",
                        principalTable: "campi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ofertas_cursos_curso_id",
                        column: x => x.curso_id,
                        principalSchema: "syki",
                        principalTable: "cursos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ofertas_grades_grade_id",
                        column: x => x.grade_id,
                        principalSchema: "syki",
                        principalTable: "grades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ofertas_institutions_faculdade_id",
                        column: x => x.faculdade_id,
                        principalSchema: "syki",
                        principalTable: "faculdades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "turmas",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    faculdade_id = table.Column<Guid>(type: "uuid", nullable: false),
                    disciplina_id = table.Column<Guid>(type: "uuid", nullable: false),
                    professor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    periodo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_turmas", x => x.id);
                    table.ForeignKey(
                        name: "fk_turmas_academic_periods_academic_period_id_academic_period_",
                        columns: x => new { x.periodo, x.faculdade_id },
                        principalSchema: "syki",
                        principalTable: "academic_periods",
                        principalColumns: new[] { "id", "institution_id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_turmas_disciplinas_disciplina_id",
                        column: x => x.disciplina_id,
                        principalSchema: "syki",
                        principalTable: "disciplinas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_turmas_professores_professor_id",
                        column: x => x.professor_id,
                        principalSchema: "syki",
                        principalTable: "professores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "alunos",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    oferta_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "text", nullable: false),
                    matricula = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_alunos", x => x.id);
                    table.ForeignKey(
                        name: "fk_alunos_asp_net_users_user_id",
                        columns: x => new { x.institution_id, x.id },
                        principalSchema: "syki",
                        principalTable: "users",
                        principalColumns: new[] { "institution_id", "id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_alunos_institutions_faculdade_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "faculdades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_alunos_ofertas_oferta_id1",
                        column: x => x.oferta_id,
                        principalSchema: "syki",
                        principalTable: "ofertas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aulas",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    turma_id = table.Column<Guid>(type: "uuid", nullable: false),
                    day = table.Column<DateOnly>(type: "date", nullable: false),
                    start = table.Column<string>(type: "text", nullable: false),
                    end = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_aulas", x => x.id);
                    table.ForeignKey(
                        name: "fk_aulas_turmas_turma_id",
                        column: x => x.turma_id,
                        principalSchema: "syki",
                        principalTable: "turmas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "evaluation_units",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    turma_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_evaluation_units", x => x.id);
                    table.ForeignKey(
                        name: "fk_evaluation_units_turmas_turma_id",
                        column: x => x.turma_id,
                        principalSchema: "syki",
                        principalTable: "turmas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "horarios",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    turma_id = table.Column<Guid>(type: "uuid", nullable: false),
                    dia = table.Column<string>(type: "text", nullable: false),
                    start = table.Column<string>(type: "text", nullable: false),
                    end = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_horarios", x => x.id);
                    table.ForeignKey(
                        name: "fk_horarios_turmas_turma_id",
                        column: x => x.turma_id,
                        principalSchema: "syki",
                        principalTable: "turmas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "turmas__alunos",
                schema: "syki",
                columns: table => new
                {
                    turma_id = table.Column<Guid>(type: "uuid", nullable: false),
                    aluno_id = table.Column<Guid>(type: "uuid", nullable: false),
                    situacao = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_turmas__alunos", x => new { x.aluno_id, x.turma_id });
                    table.ForeignKey(
                        name: "fk_turmas__alunos_alunos_aluno_id",
                        column: x => x.aluno_id,
                        principalSchema: "syki",
                        principalTable: "alunos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_turmas__alunos_turmas_turma_id",
                        column: x => x.turma_id,
                        principalSchema: "syki",
                        principalTable: "turmas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chamadas",
                schema: "syki",
                columns: table => new
                {
                    aula_id = table.Column<Guid>(type: "uuid", nullable: false),
                    aluno_id = table.Column<Guid>(type: "uuid", nullable: false),
                    presente = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chamadas", x => new { x.aula_id, x.aluno_id });
                    table.ForeignKey(
                        name: "fk_chamadas_alunos_aluno_id",
                        column: x => x.aluno_id,
                        principalSchema: "syki",
                        principalTable: "alunos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_chamadas_aulas_aula_id",
                        column: x => x.aula_id,
                        principalSchema: "syki",
                        principalTable: "aulas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "evaluations",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    unit_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_evaluations", x => x.id);
                    table.ForeignKey(
                        name: "fk_evaluations_evaluation_unit_evaluation_unit_id",
                        column: x => x.unit_id,
                        principalSchema: "syki",
                        principalTable: "evaluation_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "syki",
                table: "roles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { new Guid("2e12254d-431d-4c18-865b-f792e3e78a1d"), "ffebfcb7-c09b-4fad-8593-2bf18de1efb8", "Aluno", "ALUNO" },
                    { new Guid("342ec282-ff3d-4ed1-9f9d-7eca2473c1ce"), "a9680994-4ca8-4274-b55d-6664b4aa9768", "Academico", "ACADEMICO" },
                    { new Guid("7643c6fb-0afa-4d04-bee9-484c4b5a6f6a"), "a59859aa-fa8e-4ebc-9ee4-2840f88fe4d5", "Professor", "PROFESSOR" },
                    { new Guid("a07563d5-223a-4af7-880b-d65bac1c386e"), "f83fb461-f4ba-43d8-b3b0-248624f1a364", "Adm", "ADM" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_academic_periods_institution_id",
                schema: "syki",
                table: "academic_periods",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_alunos_institution_id_id",
                schema: "syki",
                table: "alunos",
                columns: new[] { "institution_id", "id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_alunos_oferta_id",
                schema: "syki",
                table: "alunos",
                column: "oferta_id");

            migrationBuilder.CreateIndex(
                name: "ix_audit_logs_faculdade_id_user_id",
                schema: "syki",
                table: "audit_logs",
                columns: new[] { "faculdade_id", "user_id" });

            migrationBuilder.CreateIndex(
                name: "ix_aulas_turma_id",
                schema: "syki",
                table: "aulas",
                column: "turma_id");

            migrationBuilder.CreateIndex(
                name: "ix_campi_institution_id",
                schema: "syki",
                table: "campi",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_chamadas_aluno_id",
                schema: "syki",
                table: "chamadas",
                column: "aluno_id");

            migrationBuilder.CreateIndex(
                name: "ix_cursos_faculdade_id",
                schema: "syki",
                table: "cursos",
                column: "faculdade_id");

            migrationBuilder.CreateIndex(
                name: "ix_cursos__disciplinas_disciplina_id",
                schema: "syki",
                table: "cursos__disciplinas",
                column: "disciplina_id");

            migrationBuilder.CreateIndex(
                name: "ix_disciplinas_faculdade_id",
                schema: "syki",
                table: "disciplinas",
                column: "faculdade_id");

            migrationBuilder.CreateIndex(
                name: "ix_evaluation_units_turma_id",
                schema: "syki",
                table: "evaluation_units",
                column: "turma_id");

            migrationBuilder.CreateIndex(
                name: "ix_evaluations_unit_id",
                schema: "syki",
                table: "evaluations",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_grades_curso_id",
                schema: "syki",
                table: "grades",
                column: "curso_id");

            migrationBuilder.CreateIndex(
                name: "ix_grades_faculdade_id",
                schema: "syki",
                table: "grades",
                column: "faculdade_id");

            migrationBuilder.CreateIndex(
                name: "ix_grades__disciplinas_grade_id",
                schema: "syki",
                table: "grades__disciplinas",
                column: "grade_id");

            migrationBuilder.CreateIndex(
                name: "ix_horarios_turma_id",
                schema: "syki",
                table: "horarios",
                column: "turma_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_faculdade_id",
                schema: "syki",
                table: "notifications",
                column: "faculdade_id");

            migrationBuilder.CreateIndex(
                name: "ix_ofertas_campus_id",
                schema: "syki",
                table: "ofertas",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "ix_ofertas_curso_id",
                schema: "syki",
                table: "ofertas",
                column: "curso_id");

            migrationBuilder.CreateIndex(
                name: "ix_ofertas_faculdade_id",
                schema: "syki",
                table: "ofertas",
                column: "faculdade_id");

            migrationBuilder.CreateIndex(
                name: "ix_ofertas_grade_id",
                schema: "syki",
                table: "ofertas",
                column: "grade_id");

            migrationBuilder.CreateIndex(
                name: "ix_ofertas_periodo_faculdade_id",
                schema: "syki",
                table: "ofertas",
                columns: new[] { "periodo", "faculdade_id" });

            migrationBuilder.CreateIndex(
                name: "ix_professores_faculdade_id_id",
                schema: "syki",
                table: "professores",
                columns: new[] { "faculdade_id", "id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_reset_password_tokens_token",
                schema: "syki",
                table: "reset_password_tokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_reset_password_tokens_user_id",
                schema: "syki",
                table: "reset_password_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_claims_role_id",
                schema: "syki",
                table: "role_claims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "syki",
                table: "roles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_turmas_disciplina_id",
                schema: "syki",
                table: "turmas",
                column: "disciplina_id");

            migrationBuilder.CreateIndex(
                name: "ix_turmas_periodo_faculdade_id",
                schema: "syki",
                table: "turmas",
                columns: new[] { "periodo", "faculdade_id" });

            migrationBuilder.CreateIndex(
                name: "ix_turmas_professor_id",
                schema: "syki",
                table: "turmas",
                column: "professor_id");

            migrationBuilder.CreateIndex(
                name: "ix_turmas__alunos_turma_id",
                schema: "syki",
                table: "turmas__alunos",
                column: "turma_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_claims_user_id",
                schema: "syki",
                table: "user_claims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_logins_user_id",
                schema: "syki",
                table: "user_logins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_registers_email",
                schema: "syki",
                table: "user_registers",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_role_id",
                schema: "syki",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "syki",
                table: "users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "syki",
                table: "users",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users__notifications_notification_id",
                schema: "syki",
                table: "users__notifications",
                column: "notification_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_logs",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "chamadas",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "cursos__disciplinas",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "enrollment_periods",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "evaluations",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "grades__disciplinas",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "horarios",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "reset_password_tokens",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "role_claims",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "tasks",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "turmas__alunos",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "user_claims",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "user_logins",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "user_registers",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "user_roles",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "user_tokens",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "users__notifications",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "aulas",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "evaluation_units",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "alunos",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "notifications",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "turmas",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "ofertas",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "disciplinas",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "professores",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "academic_periods",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "campi",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "grades",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "users",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "cursos",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "faculdades",
                schema: "syki");
        }
    }
}
