using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TgStorage.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "SOURCES",
                type: "LONG(20)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INT(20)");

            migrationBuilder.AlterColumn<long>(
                name: "SOURCE_ID",
                table: "MESSAGES",
                type: "LONG(20)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INT(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FILE_NAME",
                table: "DOCUMENTS",
                type: "NVARCHAR(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(100)",
                oldMaxLength: 256);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "SOURCES",
                type: "INT(20)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "LONG(20)");

            migrationBuilder.AlterColumn<long>(
                name: "SOURCE_ID",
                table: "MESSAGES",
                type: "INT(20)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "LONG(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FILE_NAME",
                table: "DOCUMENTS",
                type: "NVARCHAR(100)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(256)",
                oldMaxLength: 256);
        }
    }
}
