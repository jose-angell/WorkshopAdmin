using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopAdmin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateEquipmetControlcol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_part_user_created_by_user_id",
                table: "part");

            migrationBuilder.AddColumn<Guid>(
                name: "created_by_user_id",
                table: "equipment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql("""
                    UPDATE equipment
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
                table: "equipment",
                type: "timestamptz",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "updated_by_user_id",
                table: "equipment",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_equipment_created_by_user_id",
                table: "equipment",
                column: "created_by_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_equipment_user_created_by_user_id",
                table: "equipment",
                column: "created_by_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_part_user_created_by_user_id",
                table: "part",
                column: "created_by_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_equipment_user_created_by_user_id",
                table: "equipment");

            migrationBuilder.DropForeignKey(
                name: "FK_part_user_created_by_user_id",
                table: "part");

            migrationBuilder.DropIndex(
                name: "IX_equipment_created_by_user_id",
                table: "equipment");

            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                table: "equipment");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "equipment");

            migrationBuilder.DropColumn(
                name: "updated_by_user_id",
                table: "equipment");

            migrationBuilder.AddForeignKey(
                name: "FK_part_user_created_by_user_id",
                table: "part",
                column: "created_by_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
