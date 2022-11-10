using System.Net.Sockets;
using Microsoft.AspNetCore.Mvc;
using MusicDatabase.Data;
using MusicDatabase.Models;
using System.Text;

namespace MusicDatabase.Controllers;

public class PlayController : Controller
{
    private MusicContext database = new MusicContext();
    private readonly ILogger<HomeController> _logger;

    public PlayController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(string? mid)
    {
        if (database.songEntries != null)
        {
            if (mid == null)
            {
                if (database.songEntries.FirstOrDefault() != null)
                {
                    return Redirect("/Play?mid=" + database.songEntries.First().SongEntryId);
                }
                else
                {
                    ViewData["ERROR"] = "Song Database is Empty";
                    return View();
                }
            }
            else
            {
                return View(database.songEntries.Find(mid));
            }
        }
        return View(null);
    }
    public IActionResult Next(string mid)
    {
        int stop = 0;
        if (database.songEntries != null)
        {
            foreach (var song in database.songEntries)
            {
                if (stop == 1)
                {
                    return Redirect("/Play?mid=" + song.SongEntryId);
                }
                if (song.SongEntryId == mid)
                {
                    stop = 1;
                }
            }
        }
        return Redirect("/Play");
    }
    public IActionResult Previous(string mid)
    {
        int index = -1;
        if (database.songEntries != null)
        {
            foreach (var song in database.songEntries)
            {
                if (song.SongEntryId == mid)
                {
                    break;
                }
                index++;
            }
            if (index == -1)
            {
                return Redirect("/Play?mid=" + database.songEntries.ToList().Last().SongEntryId);
            }else{
                return Redirect("/Play?mid=" + database.songEntries.ToList()[index].SongEntryId);
            }
        }
        return Redirect("/Play");
    }
    public IActionResult DownloadMedia()
    {
        try
        {
            if (database.songEntries != null)
            {
                var server = new TcpClient();
                server.Connect("localhost", 8731);
                var stream = server.GetStream();
                foreach (var song in database.songEntries)
                {
                    if (song.SongEntryId != null && song.SongURL != null)
                    {
                        Console.WriteLine("DOWNLOADING :" + song.SongEntryId);
                        stream.Write(Encoding.ASCII.GetBytes(song.SongURL));
                        // stream.Write(Encoding.ASCII.GetBytes("|"));
                        Thread.Sleep(1000);
                        stream.Write(Encoding.ASCII.GetBytes(song.SongEntryId));
                        var data = new Byte[1024];
                        var bytes = stream.Read(data, 0, data.Length);
                        Console.WriteLine(Encoding.ASCII.GetString(data, 0, bytes));
                    }
                }
                stream.Close();
                server.Close();
            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            return Content($"<h1>ERROR OCCURED</h1>\n<h2>{e.Message}</h2>", "text/html");
        }
        return Content("<h1>DOWNLOADED SUCCESSFULLY</h1>", "text/html");
    }
}