namespace MusicDatabase.Models;
using Microsoft.EntityFrameworkCore;

class SongEntry
{
    public string? SongEntryId { get; set; }
    public string? SongName { get; set; }
    public string? SongArtist { get; set; }
    public string? SongAlbum { get; set; }
    public string? SongReleased { get; set; }
    public string? SongThumbnail {get; set;}
    public string? SongURL { get; set; }
}