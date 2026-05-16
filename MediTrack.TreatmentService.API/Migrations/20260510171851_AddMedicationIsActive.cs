using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediTrack.TreatmentService.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicationIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "medications",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "medications");
        }
    }
}
