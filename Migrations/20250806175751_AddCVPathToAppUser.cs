using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KariyerPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddCVPathToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CVPath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CVPath",
                table: "AspNetUsers");
        }
    }
}
