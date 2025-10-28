using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Keepass.DBLib.Migrations
{
    /// <inheritdoc />
    public partial class _0000001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Identifier = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntraID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Identifier);
                });

            migrationBuilder.CreateTable(
                name: "Vaults",
                columns: table => new
                {
                    Identifier = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AppUserIdentifier = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaults", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_Vaults_Users_AppUserIdentifier",
                        column: x => x.AppUserIdentifier,
                        principalTable: "Users",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserVault",
                columns: table => new
                {
                    SharedUsersIdentifier = table.Column<int>(type: "INTEGER", nullable: false),
                    SharedVaultsIdentifier = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserVault", x => new { x.SharedUsersIdentifier, x.SharedVaultsIdentifier });
                    table.ForeignKey(
                        name: "FK_AppUserVault_Users_SharedUsersIdentifier",
                        column: x => x.SharedUsersIdentifier,
                        principalTable: "Users",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserVault_Vaults_SharedVaultsIdentifier",
                        column: x => x.SharedVaultsIdentifier,
                        principalTable: "Vaults",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VaultEntries",
                columns: table => new
                {
                    Identifier = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VaultIdentifier = table.Column<int>(type: "INTEGER", nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaultEntries", x => x.Identifier);
                    table.ForeignKey(
                        name: "FK_VaultEntries_Vaults_VaultIdentifier",
                        column: x => x.VaultIdentifier,
                        principalTable: "Vaults",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserVault_SharedVaultsIdentifier",
                table: "AppUserVault",
                column: "SharedVaultsIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_VaultEntries_VaultIdentifier",
                table: "VaultEntries",
                column: "VaultIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Vaults_AppUserIdentifier_Name",
                table: "Vaults",
                columns: new[] { "AppUserIdentifier", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserVault");

            migrationBuilder.DropTable(
                name: "VaultEntries");

            migrationBuilder.DropTable(
                name: "Vaults");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
