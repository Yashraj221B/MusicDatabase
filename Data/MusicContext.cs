using MusicDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicDatabase.Data;

class MusicContext : DbContext
{
    public DbSet<SongEntry>? songEntries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("DataSource=MusicDatabase.db");
    }
}