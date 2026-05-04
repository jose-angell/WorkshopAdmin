using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopAdmin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class friendlyIdInCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "customer_seq");

            migrationBuilder.AddColumn<int>(
                name: "customer_number",
                table: "customer",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"customer_seq\"')");

            migrationBuilder.AddColumn<string>(
                name: "friendly_id",
                table: "customer",
                type: "text",
                nullable: false,
                computedColumnSql: " 'CUST-' || lpad(customer_number::text, 5, '0') ",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_customer_friendly_id",
                table: "customer",
                column: "friendly_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_customer_friendly_id",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "friendly_id",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "customer_number",
                table: "customer");

            migrationBuilder.DropSequence(
                name: "customer_seq");
        }
    }
}
