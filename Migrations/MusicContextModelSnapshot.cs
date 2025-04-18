﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicDatabase.Data;

#nullable disable

namespace MusicDatabase.Migrations
{
    [DbContext(typeof(MusicContext))]
    partial class MusicContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.10");

            modelBuilder.Entity("MusicDatabase.Models.SongEntry", b =>
                {
                    b.Property<string>("SongEntryId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SongAlbum")
                        .HasColumnType("TEXT");

                    b.Property<string>("SongArtist")
                        .HasColumnType("TEXT");

                    b.Property<string>("SongName")
                        .HasColumnType("TEXT");

                    b.Property<string>("SongReleased")
                        .HasColumnType("TEXT");

                    b.Property<string>("SongURL")
                        .HasColumnType("TEXT");

                    b.HasKey("SongEntryId");

                    b.ToTable("songEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
