using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniLog.Migrations
{
    /// <inheritdoc />
    public partial class SistemaSegurancaAtualizado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animes_Usuarios_UsuarioId",
                table: "Animes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Animes",
                table: "Animes");

            migrationBuilder.RenameTable(
                name: "Usuarios",
                newName: "usuarios");

            migrationBuilder.RenameTable(
                name: "Animes",
                newName: "animes");

            migrationBuilder.RenameIndex(
                name: "IX_Animes_UsuarioId",
                table: "animes",
                newName: "IX_animes_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_usuarios",
                table: "usuarios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_animes",
                table: "animes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_animes_usuarios_UsuarioId",
                table: "animes",
                column: "UsuarioId",
                principalTable: "usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_animes_usuarios_UsuarioId",
                table: "animes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_usuarios",
                table: "usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_animes",
                table: "animes");

            migrationBuilder.RenameTable(
                name: "usuarios",
                newName: "Usuarios");

            migrationBuilder.RenameTable(
                name: "animes",
                newName: "Animes");

            migrationBuilder.RenameIndex(
                name: "IX_animes_UsuarioId",
                table: "Animes",
                newName: "IX_Animes_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Animes",
                table: "Animes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_Usuarios_UsuarioId",
                table: "Animes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
