using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Keepass.DBLib.Migrations
{
    /// <inheritdoc />
    public partial class _0000002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "VaultEntries",
                newName: "HashPassword");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HashPassword",
                table: "VaultEntries",
                newName: "Password");
        }
    }
}
