using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicDatabase.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "songEntries",
                columns: table => new
                {
                    SongEntryId = table.Column<string>(type: "TEXT", nullable: false),
                    SongName = table.Column<string>(type: "TEXT", nullable: true),
                    SongArtist = table.Column<string>(type: "TEXT", nullable: true),
                    SongAlbum = table.Column<string>(type: "TEXT", nullable: true),
                    SongReleased = table.Column<string>(type: "TEXT", nullable: true),
                    SongURL = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_songEntries", x => x.SongEntryId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "songEntries");
        }
    }
}
