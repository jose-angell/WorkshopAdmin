using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopAdmin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(150)", nullable: false),
                    phone = table.Column<string>(type: "varchar(20)", nullable: false),
                    email = table.Column<string>(type: "varchar(150)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "equipment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "varchar(50)", nullable: false),
                    brand = table.Column<string>(type: "varchar(100)", nullable: false),
                    model = table.Column<string>(type: "varchar(100)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "part",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(150)", nullable: false),
                    price = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    stock = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_part", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "service_order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    equipment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    failure_description = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: false),
                    labor_cost = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_service_order_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_service_order_equipment_equipment_id",
                        column: x => x.equipment_id,
                        principalTable: "equipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_part",
                columns: table => new
                {
                    service_order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    part_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_part", x => new { x.service_order_id, x.part_id });
                    table.ForeignKey(
                        name: "FK_order_part_part_part_id",
                        column: x => x.part_id,
                        principalTable: "part",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_part_service_order_service_order_id",
                        column: x => x.service_order_id,
                        principalTable: "service_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_customer_name",
                table: "customer",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_brand_model",
                table: "equipment",
                columns: new[] { "brand", "model" });

            migrationBuilder.CreateIndex(
                name: "IX_order_part_part_id",
                table: "order_part",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "IX_part_name",
                table: "part",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_service_order_customer_id",
                table: "service_order",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_order_equipment_id",
                table: "service_order",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_order_status",
                table: "service_order",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_part");

            migrationBuilder.DropTable(
                name: "part");

            migrationBuilder.DropTable(
                name: "service_order");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "equipment");
        }
    }
}
