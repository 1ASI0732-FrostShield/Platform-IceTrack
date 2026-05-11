using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace IceTrackPlatform.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dashboard_configs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    default_site_id = table.Column<int>(type: "int", nullable: true),
                    default_temperature_range_value = table.Column<string>(type: "longtext", nullable: false),
                    default_temperature_range_label = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_dashboard_configs", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    equipment_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    model = table.Column<string>(type: "longtext", nullable: false),
                    type = table.Column<string>(type: "longtext", nullable: false),
                    serial = table.Column<string>(type: "longtext", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "longtext", nullable: false),
                    site_id = table.Column<int>(type: "int", nullable: false),
                    online = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_equipment", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "interventions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    service_request_id = table.Column<int>(type: "int", nullable: false),
                    technician_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false),
                    summary = table.Column<string>(type: "longtext", nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    photo_urls = table.Column<string>(type: "longtext", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_interventions", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    service_request_id = table.Column<int>(type: "int", nullable: false),
                    owner_id = table.Column<int>(type: "int", nullable: false),
                    technician_id = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "longtext", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_reviews", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "service_requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    requester_id = table.Column<int>(type: "int", nullable: false),
                    site_id = table.Column<int>(type: "int", nullable: false),
                    equipment_id = table.Column<int>(type: "int", nullable: false),
                    assigned_to = table.Column<int>(type: "int", nullable: false),
                    origin = table.Column<string>(type: "longtext", nullable: false),
                    type = table.Column<string>(type: "longtext", nullable: false),
                    priority = table.Column<string>(type: "longtext", nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false),
                    completed_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    canceled_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    technician_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_service_requests", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false),
                    address = table.Column<string>(type: "longtext", nullable: false),
                    contact_name = table.Column<string>(type: "longtext", nullable: false),
                    phone = table.Column<string>(type: "longtext", nullable: false),
                    cant_equipment = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    created = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sites", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "technicians",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false),
                    specialty = table.Column<string>(type: "longtext", nullable: false),
                    phone = table.Column<string>(type: "longtext", nullable: true),
                    provider_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_technicians", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "longtext", nullable: false),
                    password_hash = table.Column<string>(type: "longtext", nullable: false),
                    role = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_users", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dashboard_cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DashboardConfigId = table.Column<int>(type: "int", nullable: false),
                    card_type = table.Column<string>(type: "longtext", nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    is_visible = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_dashboard_cards", x => x.Id);
                    table.ForeignKey(
                        name: "f_k_dashboard_cards_dashboard_configs__dashboard_config_id",
                        column: x => x.DashboardConfigId,
                        principalTable: "dashboard_configs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_dashboard_cards__dashboard_config_id",
                table: "dashboard_cards",
                column: "DashboardConfigId");

            migrationBuilder.CreateIndex(
                name: "i_x_dashboard_configs_user_id",
                table: "dashboard_configs",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dashboard_cards");

            migrationBuilder.DropTable(
                name: "equipment");

            migrationBuilder.DropTable(
                name: "interventions");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "service_requests");

            migrationBuilder.DropTable(
                name: "sites");

            migrationBuilder.DropTable(
                name: "technicians");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "dashboard_configs");
        }
    }
}
