using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopAdmin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class friendlyIdInEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "equipment_seq");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "equipment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql("UPDATE equipment SET \"CustomerId\" = (SELECT id FROM customer LIMIT 1)");
            
            migrationBuilder.AddColumn<int>(
                name: "equipment_number",
                table: "equipment",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"equipment_seq\"')");

            migrationBuilder.AddColumn<string>(
                name: "friendly_id",
                table: "equipment",
                type: "text",
                nullable: false,
                computedColumnSql: "'EQ-' || lpad(equipment_number::text, 5, '0')",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_equipment_CustomerId",
                table: "equipment",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_equipment_customer_CustomerId",
                table: "equipment",
                column: "CustomerId",
                principalTable: "customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_equipment_customer_CustomerId",
                table: "equipment");

            migrationBuilder.DropIndex(
                name: "IX_equipment_CustomerId",
                table: "equipment");

            migrationBuilder.DropColumn(
                name: "friendly_id",
                table: "equipment");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "equipment");

            migrationBuilder.DropColumn(
                name: "equipment_number",
                table: "equipment");

            migrationBuilder.DropSequence(
                name: "equipment_seq");
        }
    }
}
