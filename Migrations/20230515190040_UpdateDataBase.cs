using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieSystemTMDB.Migrations
{
    public partial class UpdateDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Persons_PersonId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_PersonId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Genres");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Persons",
                newName: "PersonId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Genres",
                newName: "GenreId");

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MovieLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.MovieId);
                    table.ForeignKey(
                        name: "FK_Movies_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movies_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieRatings_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MovieRatings_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieRatings_MovieId",
                table: "MovieRatings",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieRatings_PersonId",
                table: "MovieRatings",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_GenreId",
                table: "Movies",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_PersonId",
                table: "Movies",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieRatings");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Persons",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "GenreId",
                table: "Genres",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "Genres",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_PersonId",
                table: "Genres",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Persons_PersonId",
                table: "Genres",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");
        }
    }
}
