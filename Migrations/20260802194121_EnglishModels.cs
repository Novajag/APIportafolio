using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portafolio.Migrations
{
    /// <inheritdoc />
    public partial class EnglishModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorreoElectronico",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Mensaje",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Messages",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "FechaDelMensaje",
                table: "Messages",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Messages",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Messages",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Messages",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Messages",
                newName: "FechaDelMensaje");

            migrationBuilder.AddColumn<string>(
                name: "CorreoElectronico",
                table: "Messages",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Mensaje",
                table: "Messages",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
