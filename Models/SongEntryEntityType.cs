﻿// <auto-generated />
using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable 219, 612, 618
#nullable enable

namespace MusicDatabase.Models
{
    internal partial class SongEntryEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType? baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "MusicDatabase.Models.SongEntry",
                typeof(SongEntry),
                baseEntityType);

            var songEntryId = runtimeEntityType.AddProperty(
                "SongEntryId",
                typeof(string),
                propertyInfo: typeof(SongEntry).GetProperty("SongEntryId", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SongEntry).GetField("<SongEntryId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                afterSaveBehavior: PropertySaveBehavior.Throw);

            var songAlbum = runtimeEntityType.AddProperty(
                "SongAlbum",
                typeof(string),
                propertyInfo: typeof(SongEntry).GetProperty("SongAlbum", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SongEntry).GetField("<SongAlbum>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);

            var songArtist = runtimeEntityType.AddProperty(
                "SongArtist",
                typeof(string),
                propertyInfo: typeof(SongEntry).GetProperty("SongArtist", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SongEntry).GetField("<SongArtist>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);

            var songName = runtimeEntityType.AddProperty(
                "SongName",
                typeof(string),
                propertyInfo: typeof(SongEntry).GetProperty("SongName", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SongEntry).GetField("<SongName>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);

            var songReleased = runtimeEntityType.AddProperty(
                "SongReleased",
                typeof(string),
                propertyInfo: typeof(SongEntry).GetProperty("SongReleased", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SongEntry).GetField("<SongReleased>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);

            var songThumbnail = runtimeEntityType.AddProperty(
                "SongThumbnail",
                typeof(string),
                propertyInfo: typeof(SongEntry).GetProperty("SongThumbnail", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SongEntry).GetField("<SongThumbnail>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);

            var songURL = runtimeEntityType.AddProperty(
                "SongURL",
                typeof(string),
                propertyInfo: typeof(SongEntry).GetProperty("SongURL", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(SongEntry).GetField("<SongURL>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                nullable: true);

            var key = runtimeEntityType.AddKey(
                new[] { songEntryId });
            runtimeEntityType.SetPrimaryKey(key);

            return runtimeEntityType;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "songEntries");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}
