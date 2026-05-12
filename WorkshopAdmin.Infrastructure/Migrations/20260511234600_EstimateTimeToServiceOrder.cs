using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopAdmin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EstimateTimeToServiceOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "estimated_time",
                table: "service_order",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "repair_finished_at",
                table: "service_order",
                type: "timestamptz",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "repair_started_at",
                table: "service_order",
                type: "timestamptz",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "estimated_time",
                table: "service_order");

            migrationBuilder.DropColumn(
                name: "repair_finished_at",
                table: "service_order");

            migrationBuilder.DropColumn(
                name: "repair_started_at",
                table: "service_order");
        }
    }
}
