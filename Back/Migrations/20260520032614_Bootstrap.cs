using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

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
                name: "audit_trails",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    activity_id = table.Column<string>(type: "text", nullable: false),
                    operation = table.Column<string>(type: "text", nullable: false),
                    entity_id = table.Column<string>(type: "text", nullable: false),
                    entity_type = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    institution_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data = table.Column<JsonDocument>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_trails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "command_batches",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institution_id = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    source_command_id = table.Column<int>(type: "integer", nullable: true),
                    next_command_id = table.Column<int>(type: "integer", nullable: true),
                    size = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_command_batches", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "institutions",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_institutions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "academic_periods",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institution_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    start_at = table.Column<DateOnly>(type: "date", nullable: false),
                    end_at = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_academic_periods", x => x.id);
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
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institution_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    state = table.Column<int>(type: "integer", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false)
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
                name: "commands",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institution_id = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    data = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    duration = table.Column<int>(type: "integer", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    processor_id = table.Column<Guid>(type: "uuid", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true),
                    parent_id = table.Column<int>(type: "integer", nullable: true),
                    original_id = table.Column<int>(type: "integer", nullable: true),
                    batch_id = table.Column<int>(type: "integer", nullable: true),
                    not_before = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    activity_id = table.Column<string>(type: "text", nullable: true),
                    max_retries = table.Column<int>(type: "integer", nullable: false),
                    retry_attempt = table.Column<int>(type: "integer", nullable: false),
                    backoff_strategy = table.Column<int>(type: "integer", nullable: false),
                    base_delay_seconds = table.Column<int>(type: "integer", nullable: false),
                    logs = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_commands", x => x.id);
                    table.ForeignKey(
                        name: "fk_commands_institutions_institution_id",
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
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institution_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    course_type = table.Column<int>(type: "integer", nullable: false)
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
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institution_id = table.Column<int>(type: "integer", nullable: false),
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
                name: "enrollment_periods",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institution_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    start_at = table.Column<DateOnly>(type: "date", nullable: false),
                    end_at = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_enrollment_periods", x => x.id);
                    table.ForeignKey(
                        name: "fk_enrollment_periods_institutions_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "institutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    owner_id = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    permissions = table.Column<List<int>>(type: "integer[]", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                    table.ForeignKey(
                        name: "fk_roles_institutions_owner_id",
                        column: x => x.owner_id,
                        principalSchema: "syki",
                        principalTable: "institutions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institution_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    profile_photo = table.Column<string>(type: "text", nullable: true),
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
                name: "course_curriculums",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institution_id = table.Column<int>(type: "integer", nullable: false),
                    course_id = table.Column<int>(type: "integer", nullable: false),
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
                name: "courses_disciplines",
                schema: "syki",
                columns: table => new
                {
                    course_id = table.Column<int>(type: "integer", nullable: false),
                    discipline_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_courses_disciplines", x => new { x.course_id, x.discipline_id });
                    table.ForeignKey(
                        name: "fk_courses_disciplines_courses_course_id",
                        column: x => x.course_id,
                        principalSchema: "syki",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_courses_disciplines_discipline_discipline_id",
                        column: x => x.discipline_id,
                        principalSchema: "syki",
                        principalTable: "disciplines",
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
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_role_claims_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "syki",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "magic_links",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    used_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_magic_links", x => x.id);
                    table.ForeignKey(
                        name: "fk_magic_links_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "syki",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "students",
                schema: "syki",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    institution_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    enrollment_code = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
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
                        name: "fk_students_institutions_institution_id",
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
                    id = table.Column<int>(type: "integer", nullable: false),
                    institution_id = table.Column<int>(type: "integer", nullable: false),
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
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_claims_users_user_id",
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
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_logins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_user_logins_users_user_id",
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
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    institution_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => new { x.institution_id, x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_user_roles_institutions_institution_id",
                        column: x => x.institution_id,
                        principalSchema: "syki",
                        principalTable: "institutions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "syki",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
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
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_user_tokens_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "syki",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_curriculum_discipline",
                schema: "syki",
                columns: table => new
                {
                    course_curriculum_id = table.Column<int>(type: "integer", nullable: false),
                    discipline_id = table.Column<int>(type: "integer", nullable: false),
                    period = table.Column<byte>(type: "smallint", nullable: false),
                    credits = table.Column<byte>(type: "smallint", nullable: false),
                    workload = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_course_curriculum_discipline", x => new { x.course_curriculum_id, x.discipline_id });
                    table.ForeignKey(
                        name: "fk_course_curriculum_discipline_course_curriculum_course_curri",
                        column: x => x.course_curriculum_id,
                        principalSchema: "syki",
                        principalTable: "course_curriculums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_course_curriculum_discipline_discipline_discipline_id",
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
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    institution_id = table.Column<int>(type: "integer", nullable: false),
                    campus_id = table.Column<int>(type: "integer", nullable: false),
                    course_id = table.Column<int>(type: "integer", nullable: false),
                    course_curriculum_id = table.Column<int>(type: "integer", nullable: false),
                    academic_period_id = table.Column<int>(type: "integer", nullable: false),
                    session = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_course_offerings", x => x.id);
                    table.ForeignKey(
                        name: "fk_course_offerings_academic_period_academic_period_id",
                        column: x => x.academic_period_id,
                        principalSchema: "syki",
                        principalTable: "academic_periods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_course_offerings_campi_campus_id",
                        column: x => x.campus_id,
                        principalSchema: "syki",
                        principalTable: "campi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_course_offerings_course_curriculum_course_curriculum_id",
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
                name: "teachers_campi",
                schema: "syki",
                columns: table => new
                {
                    teacher_id = table.Column<int>(type: "integer", nullable: false),
                    campus_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teachers_campi", x => new { x.teacher_id, x.campus_id });
                    table.ForeignKey(
                        name: "fk_teachers_campi_campi_campus_id",
                        column: x => x.campus_id,
                        principalSchema: "syki",
                        principalTable: "campi",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teachers_campi_teachers_teacher_id",
                        column: x => x.teacher_id,
                        principalSchema: "syki",
                        principalTable: "teachers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teachers_disciplines",
                schema: "syki",
                columns: table => new
                {
                    teacher_id = table.Column<int>(type: "integer", nullable: false),
                    discipline_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teachers_disciplines", x => new { x.teacher_id, x.discipline_id });
                    table.ForeignKey(
                        name: "fk_teachers_disciplines_discipline_discipline_id",
                        column: x => x.discipline_id,
                        principalSchema: "syki",
                        principalTable: "disciplines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teachers_disciplines_teachers_teacher_id",
                        column: x => x.teacher_id,
                        principalSchema: "syki",
                        principalTable: "teachers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_academic_periods_institution_id",
                schema: "syki",
                table: "academic_periods",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_campi_institution_id",
                schema: "syki",
                table: "campi",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_commands_institution_id",
                schema: "syki",
                table: "commands",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_course_curriculum_discipline_discipline_id",
                schema: "syki",
                table: "course_curriculum_discipline",
                column: "discipline_id");

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
                name: "ix_course_offerings_academic_period_id",
                schema: "syki",
                table: "course_offerings",
                column: "academic_period_id");

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
                name: "ix_courses_institution_id",
                schema: "syki",
                table: "courses",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_courses_disciplines_discipline_id",
                schema: "syki",
                table: "courses_disciplines",
                column: "discipline_id");

            migrationBuilder.CreateIndex(
                name: "ix_disciplines_code",
                schema: "syki",
                table: "disciplines",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_disciplines_institution_id",
                schema: "syki",
                table: "disciplines",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_enrollment_periods_institution_id",
                schema: "syki",
                table: "enrollment_periods",
                column: "institution_id");

            migrationBuilder.CreateIndex(
                name: "ix_magic_links_user_id",
                schema: "syki",
                table: "magic_links",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_claims_role_id",
                schema: "syki",
                table: "role_claims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_roles_owner_id_normalized_name",
                schema: "syki",
                table: "roles",
                columns: new[] { "owner_id", "normalized_name" },
                unique: true)
                .Annotation("Npgsql:NullsDistinct", false);

            migrationBuilder.CreateIndex(
                name: "role_name_index",
                schema: "syki",
                table: "roles",
                column: "normalized_name");

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
                name: "ix_teachers_campi_campus_id",
                schema: "syki",
                table: "teachers_campi",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "ix_teachers_disciplines_discipline_id",
                schema: "syki",
                table: "teachers_disciplines",
                column: "discipline_id");

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
                name: "ix_user_roles_institution_id_user_id",
                schema: "syki",
                table: "user_roles",
                columns: new[] { "institution_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_role_id",
                schema: "syki",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_user_id",
                schema: "syki",
                table: "user_roles",
                column: "user_id");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_trails",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "command_batches",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "commands",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "course_curriculum_discipline",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "course_offerings",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "courses_disciplines",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "enrollment_periods",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "magic_links",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "role_claims",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "students",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "teachers_campi",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "teachers_disciplines",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "user_claims",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "user_logins",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "user_roles",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "user_tokens",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "academic_periods",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "course_curriculums",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "campi",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "disciplines",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "teachers",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "courses",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "users",
                schema: "syki");

            migrationBuilder.DropTable(
                name: "institutions",
                schema: "syki");
        }
    }
}
