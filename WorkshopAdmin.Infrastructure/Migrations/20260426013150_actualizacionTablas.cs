using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopAdmin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class actualizacionTablas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "equipment");

            migrationBuilder.AddColumn<short>(
                name: "service_type_id",
                table: "service_order",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "brand",
                table: "part",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "part",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "min_stock",
                table: "part",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "part_category_id",
                table: "part",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "sku",
                table: "part",
                type: "varchar(150)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "unit_of_measure",
                table: "part",
                type: "varchar(10)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "warehouse_location",
                table: "part",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description_type",
                table: "equipment",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "equipment_type_id",
                table: "equipment",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "equipment",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "service_type_id",
                table: "service_order");

            migrationBuilder.DropColumn(
                name: "brand",
                table: "part");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "part");

            migrationBuilder.DropColumn(
                name: "min_stock",
                table: "part");

            migrationBuilder.DropColumn(
                name: "part_category_id",
                table: "part");

            migrationBuilder.DropColumn(
                name: "sku",
                table: "part");

            migrationBuilder.DropColumn(
                name: "unit_of_measure",
                table: "part");

            migrationBuilder.DropColumn(
                name: "warehouse_location",
                table: "part");

            migrationBuilder.DropColumn(
                name: "description_type",
                table: "equipment");

            migrationBuilder.DropColumn(
                name: "equipment_type_id",
                table: "equipment");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "equipment");

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "equipment",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");
        }
    }
}
