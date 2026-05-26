using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopAdmin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddControlCollToCustomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "created_by_user_id",
                table: "customer",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql("""
                    UPDATE customer
                    SET created_by_user_id =
                    (
                        SELECT id
                        FROM "user"
                        LIMIT 1
                    )
                    WHERE created_by_user_id = '00000000-0000-0000-0000-000000000000';
                """);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                table: "customer",
                type: "timestamptz",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "updated_by_user_id",
                table: "customer",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_customer_created_by_user_id",
                table: "customer",
                column: "created_by_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_customer_user_created_by_user_id",
                table: "customer",
                column: "created_by_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customer_user_created_by_user_id",
                table: "customer");

            migrationBuilder.DropIndex(
                name: "IX_customer_created_by_user_id",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "updated_by_user_id",
                table: "customer");
        }
    }
}
