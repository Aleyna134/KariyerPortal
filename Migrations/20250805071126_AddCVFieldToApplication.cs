using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KariyerPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddCVFieldToApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CVPath",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CVPath",
                table: "JobApplications");
        }
    }
}
