using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopAdmin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class controlColPart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "created_by_user_id",
                table: "part",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql("""
            UPDATE part
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
                table: "part",
                type: "timestamptz",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "updated_by_user_id",
                table: "part",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_part_created_by_user_id",
                table: "part",
                column: "created_by_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_part_user_created_by_user_id",
                table: "part",
                column: "created_by_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_part_user_created_by_user_id",
                table: "part");

            migrationBuilder.DropIndex(
                name: "IX_part_created_by_user_id",
                table: "part");

            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                table: "part");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "part");

            migrationBuilder.DropColumn(
                name: "updated_by_user_id",
                table: "part");
        }
    }
}
