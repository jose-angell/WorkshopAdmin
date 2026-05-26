using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopAdmin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addcontrolcolsToServiceorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                table: "service_order",
                type: "timestamptz",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()");

            migrationBuilder.AddColumn<Guid>(
                name: "created_by_user_id",
                table: "service_order",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql("""
                    UPDATE service_order
                    SET created_by_user_id =
                    (
                        SELECT id
                        FROM "user"
                        LIMIT 1
                    )
                    WHERE created_by_user_id = '00000000-0000-0000-0000-000000000000';
                """);

            migrationBuilder.AddColumn<Guid>(
                name: "technician_id",
                table: "service_order",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql("""
                    UPDATE service_order
                    SET technician_id =
                    (
                        SELECT id
                        FROM "user"
                        LIMIT 1
                    )
                    WHERE technician_id = '00000000-0000-0000-0000-000000000000';
                """);

            migrationBuilder.AddColumn<Guid>(
                name: "updated_by_user_id",
                table: "service_order",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_service_order_created_by_user_id",
                table: "service_order",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_order_technician_id",
                table: "service_order",
                column: "technician_id");

            migrationBuilder.AddForeignKey(
                name: "FK_service_order_user_created_by_user_id",
                table: "service_order",
                column: "created_by_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_service_order_user_technician_id",
                table: "service_order",
                column: "technician_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_service_order_user_created_by_user_id",
                table: "service_order");

            migrationBuilder.DropForeignKey(
                name: "FK_service_order_user_technician_id",
                table: "service_order");

            migrationBuilder.DropIndex(
                name: "IX_service_order_created_by_user_id",
                table: "service_order");

            migrationBuilder.DropIndex(
                name: "IX_service_order_technician_id",
                table: "service_order");

            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                table: "service_order");

            migrationBuilder.DropColumn(
                name: "technician_id",
                table: "service_order");

            migrationBuilder.DropColumn(
                name: "updated_by_user_id",
                table: "service_order");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                table: "service_order",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldNullable: true);
        }
    }
}
