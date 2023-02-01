using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MusicDatabase.Models;
using MusicDatabase.Data;
using System.Security.Cryptography;
using System.Text;

namespace MusicDatabase.Controllers;

public class DatabaseController : Controller
{
    private readonly ILogger<HomeController> _logger;

    protected private MusicContext database = new MusicContext();

    public DatabaseController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(string SongName, string SongArtist, string SongAlbum, string SongReleased, string SongURL)
    {
        SongEntry song = new SongEntry();
        song.SongName = SongName;
        song.SongArtist = SongArtist;
        song.SongAlbum = SongAlbum;
        song.SongReleased = SongReleased;
        song.SongURL = SongURL;
        song.SongEntryId = Convert.ToHexString(SHA256.HashData(Encoding.ASCII.GetBytes(SongName)));
        database.Add(song);
        database.SaveChanges();
        return View();
    }

    [HttpGet]
    public IActionResult Edit(string? Id)
    {
        if (Id != null && database.songEntries != null)
        {
            var songEntry = database.songEntries.Find(Id);
            return View(songEntry);
        }
        else
        {
            return Redirect("/Database/List");
        }
    }
    [HttpPost]
    public IActionResult Edit(string SongID, string SongName, string SongArtist, string SongAlbum, string SongReleased, string SongURL)
    {
        if (database.songEntries != null)
        {
            var song = database.songEntries.Find(SongID);
            if (song != null)
            {
                song.SongName = SongName;
                song.SongArtist = SongArtist;
                song.SongAlbum = SongAlbum;
                song.SongReleased = SongReleased;
                song.SongURL = SongURL;
                database.SaveChanges();
            }
            return Redirect("/Database/Edit/" + SongID);
        }
        return Redirect("/Database/Edit/" + SongID);
    }

    public IActionResult List()
    {
        if (database.songEntries != null)
        {
            var songs = database.songEntries.ToArray();
            SongListModel model = new SongListModel();
            model.SongList = songs;

            return View(model);
        }
        return View();
    }

    public IActionResult Delete(string? id)
    {
        if (id != null)
        {
            if (database.songEntries != null)
            {
                var song = database.songEntries.Find(id);
                if (song != null)
                {
                    database.songEntries.Remove(song);
                    database.SaveChanges();                    
                }
            }
        }
        return Redirect("/Database/List");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
