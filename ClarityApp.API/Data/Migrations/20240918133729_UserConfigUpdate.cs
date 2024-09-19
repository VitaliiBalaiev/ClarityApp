using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClarityApp.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserConfigUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "AppUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "AppUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
