using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Back.Migrations
{
    /// <inheritdoc />
    public partial class Bootstrap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "syki");

            migrationBuilder.CreateTable(
                name: "command_batches",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    event_id = table.Column<Guid>(type: "uuid", nullable: true),
                    source_command_id = table.Column<Guid>(type: "uuid", nullable: true),
                    next_command_id = table.Column<Guid>(type: "uuid", nullable: true),
                    size = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_command_batches", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "commands",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    data = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    duration = table.Column<int>(type: "integer", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    processor_id = table.Column<Guid>(type: "uuid", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true),
                    event_id = table.Column<Guid>(type: "uuid", nullable: true),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    original_id = table.Column<Guid>(type: "uuid", nullable: true),
                    batch_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_commands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "domain_events",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    data = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    occurred_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    processor_id = table.Column<Guid>(type: "uuid", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true),
                    duration = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_domain_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "feature_flags",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cross_login = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_feature_flags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "institutions",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_institutions", x => x.id);
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
                name: "user_registers",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
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
                    start_at = table.Column<DateOnly>(type: "date", nullable: false),
                    end_at = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_academic_periods", x => new { x.id, x.institution_id });
                    table.ForeignKey(
                        name: "fk_academic_periods_institutions_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "institutions",
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
                    state = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_campi", x => x.id);
                    table.ForeignKey(
                        name: "fk_campi_institutions_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "institutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_courses", x => x.id);
                    table.ForeignKey(
                        name: "fk_courses_institutions_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "institutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "disciplines",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_disciplines", x => x.id);
                    table.ForeignKey(
                        name: "fk_disciplines_institutions_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "institutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "institution_configs",
                schema: "syki",
                columns: table => new
                {
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    note_limit = table.Column<decimal>(type: "numeric", nullable: false),
                    frequency_limit = table.Column<decimal>(type: "numeric(4,2)", precision: 4, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_institution_configs", x => x.institution_id);
                    table.ForeignKey(
                        name: "fk_institution_configs_institutions_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "institutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    timeless = table.Column<bool>(type: "boolean", nullable: false),
                    target = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_notifications_institutions_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "institutions",
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
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                        name: "fk_users_institutions_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "institutions",
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
                    start_at = table.Column<DateOnly>(type: "date", nullable: false),
                    end_at = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_enrollment_periods", x => new { x.id, x.institution_id });
                    table.ForeignKey(
                        name: "fk_enrollment_periods_academic_periods_id_institution_id",
                        columns: x => new { x.id, x.institution_id },
                        principalSchema: "syki",
                        principalTable: "academic_periods",
                        principalColumns: new[] { "id", "institution_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_curriculums",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_course_curriculums", x => x.id);
                    table.ForeignKey(
                        name: "fk_course_curriculums_courses_course_id",
                        column: x => x.course_id,
                        principalSchema: "syki",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_course_curriculums_institutions_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "institutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "courses__disciplines",
                schema: "syki",
                columns: table => new
                {
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discipline_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_courses__disciplines", x => new { x.course_id, x.discipline_id });
                    table.ForeignKey(
                        name: "fk_courses__disciplines_courses_course_id",
                        column: x => x.course_id,
                        principalSchema: "syki",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_courses__disciplines_disciplines_discipline_id",
                        column: x => x.discipline_id,
                        principalSchema: "syki",
                        principalTable: "disciplines",
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
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    data = table.Column<JsonDocument>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_audit_logs_asp_net_users_institution_id_user_id",
                        columns: x => new { x.institution_id, x.user_id },
                        principalSchema: "syki",
                        principalTable: "users",
                        principalColumns: new[] { "institution_id", "id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reset_password_tokens",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    used_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reset_password_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_reset_password_tokens_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "syki",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reset_password_tokens_institutions_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "institutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teachers",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teachers", x => x.id);
                    table.ForeignKey(
                        name: "fk_teachers_asp_net_users_institution_id_id",
                        columns: x => new { x.institution_id, x.id },
                        principalSchema: "syki",
                        principalTable: "users",
                        principalColumns: new[] { "institution_id", "id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teachers_institutions_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "institutions",
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
                    viewed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users__notifications", x => new { x.user_id, x.notification_id });
                    table.ForeignKey(
                        name: "fk_users__notifications_asp_net_users_user_id",
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
                name: "course_curriculums__disciplines",
                schema: "syki",
                columns: table => new
                {
                    course_curriculum_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discipline_id = table.Column<Guid>(type: "uuid", nullable: false),
                    period = table.Column<byte>(type: "smallint", nullable: false),
                    credits = table.Column<byte>(type: "smallint", nullable: false),
                    workload = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_course_curriculums__disciplines", x => new { x.course_curriculum_id, x.discipline_id });
                    table.ForeignKey(
                        name: "fk_course_curriculums__disciplines_course_curriculums_course_c",
                        column: x => x.course_curriculum_id,
                        principalSchema: "syki",
                        principalTable: "course_curriculums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_course_curriculums__disciplines_disciplines_discipline_id",
                        column: x => x.discipline_id,
                        principalSchema: "syki",
                        principalTable: "disciplines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_offerings",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    campus_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_curriculum_id = table.Column<Guid>(type: "uuid", nullable: false),
                    period = table.Column<string>(type: "text", nullable: false),
                    shift = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_course_offerings", x => x.id);
                    table.ForeignKey(
                        name: "fk_course_offerings_academic_periods_period_institution_id",
                        columns: x => new { x.period, x.institution_id },
                        principalSchema: "syki",
                        principalTable: "academic_periods",
                        principalColumns: new[] { "id", "institution_id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_course_offerings_campi_campus_id",
                        column: x => x.campus_id,
                        principalSchema: "syki",
                        principalTable: "campi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_course_offerings_course_curriculums_course_curriculum_id",
                        column: x => x.course_curriculum_id,
                        principalSchema: "syki",
                        principalTable: "course_curriculums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_course_offerings_courses_course_id",
                        column: x => x.course_id,
                        principalSchema: "syki",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_course_offerings_institutions_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "institutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "classes",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discipline_id = table.Column<Guid>(type: "uuid", nullable: false),
                    teacher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    period_id = table.Column<string>(type: "text", nullable: false),
                    vacancies = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    workload = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_classes", x => x.id);
                    table.ForeignKey(
                        name: "fk_classes_academic_periods_period_id_institution_id",
                        columns: x => new { x.period_id, x.institution_id },
                        principalSchema: "syki",
                        principalTable: "academic_periods",
                        principalColumns: new[] { "id", "institution_id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_classes_disciplines_discipline_id",
                        column: x => x.discipline_id,
                        principalSchema: "syki",
                        principalTable: "disciplines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_classes_teachers_teacher_id",
                        column: x => x.teacher_id,
                        principalSchema: "syki",
                        principalTable: "teachers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "students",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    institution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_offering_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    enrollment_code = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    yield_coefficient = table.Column<decimal>(type: "numeric(4,2)", precision: 4, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_students", x => x.id);
                    table.ForeignKey(
                        name: "fk_students_asp_net_users_institution_id_id",
                        columns: x => new { x.institution_id, x.id },
                        principalSchema: "syki",
                        principalTable: "users",
                        principalColumns: new[] { "institution_id", "id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_students_course_offerings_course_offering_id",
                        column: x => x.course_offering_id,
                        principalSchema: "syki",
                        principalTable: "course_offerings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_students_institutions_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "institutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "class_activities",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    note = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    due_date = table.Column<DateOnly>(type: "date", nullable: false),
                    due_hour = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_class_activities", x => x.id);
                    table.ForeignKey(
                        name: "fk_class_activities_classes_class_id",
                        column: x => x.class_id,
                        principalSchema: "syki",
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "class_lessons",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_at = table.Column<string>(type: "text", nullable: false),
                    end_at = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_class_lessons", x => x.id);
                    table.ForeignKey(
                        name: "fk_class_lessons_classes_class_id",
                        column: x => x.class_id,
                        principalSchema: "syki",
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "schedules",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    day = table.Column<string>(type: "text", nullable: false),
                    start_at = table.Column<string>(type: "text", nullable: false),
                    end_at = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_schedules", x => x.id);
                    table.ForeignKey(
                        name: "fk_schedules_classes_class_id",
                        column: x => x.class_id,
                        principalSchema: "syki",
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "classes__students",
                schema: "syki",
                columns: table => new
                {
                    class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    syki_student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_discipline_status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_classes__students", x => new { x.class_id, x.syki_student_id });
                    table.ForeignKey(
                        name: "fk_classes__students_classes_class_id",
                        column: x => x.class_id,
                        principalSchema: "syki",
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_classes__students_students_syki_student_id",
                        column: x => x.syki_student_id,
                        principalSchema: "syki",
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_class_notes",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    note = table.Column<decimal>(type: "numeric(4,2)", precision: 4, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_student_class_notes", x => x.id);
                    table.ForeignKey(
                        name: "fk_student_class_notes_classes_class_id",
                        column: x => x.class_id,
                        principalSchema: "syki",
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_student_class_notes_students_student_id",
                        column: x => x.student_id,
                        principalSchema: "syki",
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "class_activity_works",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_activity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    syki_student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    link = table.Column<string>(type: "text", nullable: true),
                    note = table.Column<decimal>(type: "numeric", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_class_activity_works", x => x.id);
                    table.ForeignKey(
                        name: "fk_class_activity_works_class_activities_class_activity_id",
                        column: x => x.class_activity_id,
                        principalSchema: "syki",
                        principalTable: "class_activities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_class_activity_works_students_syki_student_id",
                        column: x => x.syki_student_id,
                        principalSchema: "syki",
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "class_lesson_attendances",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    lesson_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    present = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_class_lesson_attendances", x => x.id);
                    table.ForeignKey(
                        name: "fk_class_lesson_attendances_class_lessons_lesson_id",
                        column: x => x.lesson_id,
                        principalSchema: "syki",
                        principalTable: "class_lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_class_lesson_attendances_classes_class_id",
                        column: x => x.class_id,
                        principalSchema: "syki",
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_class_lesson_attendances_students_student_id",
                        column: x => x.student_id,
                        principalSchema: "syki",
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "syki",
                table: "roles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { new Guid("5912ebe1-9e6a-4ce1-90bf-8490534fb4e1"), "5912ebe1-9e6a-4ce1-90bf-8490534fb4e1", "Adm", "ADM" },
                    { new Guid("78691a7a-f554-42d7-a5cf-8d474b6de0dd"), "78691a7a-f554-42d7-a5cf-8d474b6de0dd", "Academic", "ACADEMIC" },
                    { new Guid("ca6f394f-6fd9-48cc-90a8-b379636a24e7"), "ca6f394f-6fd9-48cc-90a8-b379636a24e7", "Teacher", "TEACHER" },
                    { new Guid("f9ad5139-06c3-4ce2-9748-ecc498b087c7"), "f9ad5139-06c3-4ce2-9748-ecc498b087c7", "Student", "STUDENT" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_academic_periods_institution_id",
                schema: "syki",
                table: "academic_periods",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_audit_logs_institution_id_user_id",
                schema: "syki",
                table: "audit_logs",
                columns: new[] { "institution_id", "user_id" });

            migrationBuilder.CreateIndex(
                name: "ix_campi_institution_id",
                schema: "syki",
                table: "campi",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_class_activities_class_id",
                schema: "syki",
                table: "class_activities",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "ix_class_activity_works_class_activity_id",
                schema: "syki",
                table: "class_activity_works",
                column: "class_activity_id");

            migrationBuilder.CreateIndex(
                name: "ix_class_activity_works_syki_student_id",
                schema: "syki",
                table: "class_activity_works",
                column: "syki_student_id");

            migrationBuilder.CreateIndex(
                name: "ix_class_lesson_attendances_class_id",
                schema: "syki",
                table: "class_lesson_attendances",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "ix_class_lesson_attendances_lesson_id_student_id",
                schema: "syki",
                table: "class_lesson_attendances",
                columns: new[] { "lesson_id", "student_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_class_lesson_attendances_student_id",
                schema: "syki",
                table: "class_lesson_attendances",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "ix_class_lessons_class_id",
                schema: "syki",
                table: "class_lessons",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "ix_classes_discipline_id",
                schema: "syki",
                table: "classes",
                column: "discipline_id");

            migrationBuilder.CreateIndex(
                name: "ix_classes_period_id_institution_id",
                schema: "syki",
                table: "classes",
                columns: new[] { "period_id", "institution_id" });

            migrationBuilder.CreateIndex(
                name: "ix_classes_teacher_id",
                schema: "syki",
                table: "classes",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "ix_classes__students_syki_student_id",
                schema: "syki",
                table: "classes__students",
                column: "syki_student_id");

            migrationBuilder.CreateIndex(
                name: "ix_course_curriculums_course_id",
                schema: "syki",
                table: "course_curriculums",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "ix_course_curriculums_institution_id",
                schema: "syki",
                table: "course_curriculums",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_course_curriculums__disciplines_discipline_id",
                schema: "syki",
                table: "course_curriculums__disciplines",
                column: "discipline_id");

            migrationBuilder.CreateIndex(
                name: "ix_course_offerings_campus_id",
                schema: "syki",
                table: "course_offerings",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "ix_course_offerings_course_curriculum_id",
                schema: "syki",
                table: "course_offerings",
                column: "course_curriculum_id");

            migrationBuilder.CreateIndex(
                name: "ix_course_offerings_course_id",
                schema: "syki",
                table: "course_offerings",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "ix_course_offerings_institution_id",
                schema: "syki",
                table: "course_offerings",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_course_offerings_period_institution_id",
                schema: "syki",
                table: "course_offerings",
                columns: new[] { "period", "institution_id" });

            migrationBuilder.CreateIndex(
                name: "ix_courses_institution_id",
                schema: "syki",
                table: "courses",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_courses__disciplines_discipline_id",
                schema: "syki",
                table: "courses__disciplines",
                column: "discipline_id");

            migrationBuilder.CreateIndex(
                name: "ix_disciplines_institution_id",
                schema: "syki",
                table: "disciplines",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_institution_id",
                schema: "syki",
                table: "notifications",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_reset_password_tokens_institution_id",
                schema: "syki",
                table: "reset_password_tokens",
                column: "institution_id");

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
                name: "role_name_index",
                schema: "syki",
                table: "roles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_schedules_class_id",
                schema: "syki",
                table: "schedules",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "ix_student_class_notes_class_id_student_id_type",
                schema: "syki",
                table: "student_class_notes",
                columns: new[] { "class_id", "student_id", "type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_student_class_notes_student_id",
                schema: "syki",
                table: "student_class_notes",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "ix_students_course_offering_id",
                schema: "syki",
                table: "students",
                column: "course_offering_id");

            migrationBuilder.CreateIndex(
                name: "ix_students_enrollment_code",
                schema: "syki",
                table: "students",
                column: "enrollment_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_students_institution_id_id",
                schema: "syki",
                table: "students",
                columns: new[] { "institution_id", "id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_teachers_institution_id_id",
                schema: "syki",
                table: "teachers",
                columns: new[] { "institution_id", "id" },
                unique: true);

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
                name: "email_index",
                schema: "syki",
                table: "users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "user_name_index",
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
                name: "class_activity_works",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "class_lesson_attendances",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "classes__students",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "command_batches",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "commands",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "course_curriculums__disciplines",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "courses__disciplines",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "domain_events",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "enrollment_periods",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "feature_flags",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "institution_configs",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "reset_password_tokens",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "role_claims",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "schedules",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "student_class_notes",
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
                name: "class_activities",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "class_lessons",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "students",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "notifications",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "classes",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "course_offerings",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "disciplines",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "teachers",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "academic_periods",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "campi",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "course_curriculums",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "users",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "courses",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "institutions",
                schema: "syki");
        }
    }
}
