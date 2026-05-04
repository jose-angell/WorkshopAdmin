using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopAdmin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class friendlyIdInServiceOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "service_order_seq");

            migrationBuilder.AddColumn<int>(
                name: "order_number",
                table: "service_order",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"service_order_seq\"')");

            migrationBuilder.AddColumn<string>(
                name: "friendly_id",
                table: "service_order",
                type: "text",
                nullable: false,
                computedColumnSql: " 'ORD-' || lpad(order_number::text, 5, '0') ",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_service_order_friendly_id",
                table: "service_order",
                column: "friendly_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_service_order_friendly_id",
                table: "service_order");

            migrationBuilder.DropColumn(
                name: "friendly_id",
                table: "service_order");

            migrationBuilder.DropColumn(
                name: "order_number",
                table: "service_order");

            migrationBuilder.DropSequence(
                name: "service_order_seq");
        }
    }
}
