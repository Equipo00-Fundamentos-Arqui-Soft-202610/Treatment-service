using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediTrack.TreatmentService.API.Migrations
{
    /// <inheritdoc />
    public partial class FixGuidColumnStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "EventId",
                table: "processed_events",
                type: "binary(16)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Id",
                table: "outbox_message",
                type: "binary(16)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "EventId",
                table: "processed_events",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "binary(16)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "outbox_message",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "binary(16)");
        }
    }
}
