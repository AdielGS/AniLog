using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniLog.Migrations
{
    /// <inheritdoc />
    public partial class AtualizaCamposDetalhesAnime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EpisodioAtual",
                table: "animes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Sinopse",
                table: "animes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalEpisodios",
                table: "animes",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EpisodioAtual",
                table: "animes");

            migrationBuilder.DropColumn(
                name: "Sinopse",
                table: "animes");

            migrationBuilder.DropColumn(
                name: "TotalEpisodios",
                table: "animes");
        }
    }
}
