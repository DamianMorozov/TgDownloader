using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TgStorage.Migrations
{
    /// <inheritdoc />
    public partial class AddedBots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BOTS",
                columns: table => new
                {
                    UID = table.Column<Guid>(type: "CHAR(36)", nullable: false),
                    BOT_TOKEN = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOTS", x => x.UID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BOTS_BOT_TOKEN",
                table: "BOTS",
                column: "BOT_TOKEN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BOTS_UID",
                table: "BOTS",
                column: "UID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BOTS");
        }
    }
}
