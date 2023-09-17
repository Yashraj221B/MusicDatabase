using System.Net.Sockets;
using Microsoft.AspNetCore.Mvc;
using MusicDatabase.Data;
using MusicDatabase.Models;
using System.Security.Cryptography;
using System.Text;
using System.Net;

namespace MusicDatabase.Controllers;

public class PlayController : Controller
{
    #pragma warning disable 
    string INVALID_CODE = "100";
    string START_DOWNLOAD = "101";
    string END_DOWNLOAD = "102";
    string URL_START = "103";
    string URL_END = "104";
    string URL_OK = "105";
    string URL_ERROR = "106";
    string HASH_START = "107";
    string HASH_END = "108";
    string HASH_OK = "109";
    string HASH_ERROR = "110";
    string DOWNLOAD_MEDIA = "111";
    string DOWNLOAD_COMPLETE = "112";
    string DOWNLOAD_ERROR = "113";
    string TOKEN_SUCCESS = "114";
    string TOKEN_ERROR = "115";

    #pragma warning restore

    private MusicContext database = new MusicContext();
    private readonly ILogger<HomeController> _logger;

    public PlayController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    private string ToHex(byte[] bytes, bool upperCase)
    {
        StringBuilder result = new StringBuilder(bytes.Length * 2);
        for (int i = 0; i < bytes.Length; i++)
            result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
        return result.ToString();
    }

    private byte[] GenerateToken(byte[] data)
    {
        using (var sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(data);
        }
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
                var song = database.songEntries.Find(mid);
                SongPlay model = new SongPlay();
                if(song != null)
                {
                    model.SongEntryId = song.SongEntryId;
                    model.SongName = song.SongName;
                    model.SongAlbum = song.SongAlbum;
                    model.SongArtist = song.SongArtist;
                    model.SongReleased = song.SongReleased;
                    model.SongThumbnail = song.SongThumbnail;
                    model.SongURL = song.SongURL;
                }
                model.SongList = database.songEntries.ToArray();
                return View(model);
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
                var server = new TcpClient(AddressFamily.InterNetwork);
                server.Connect("localhost", 8731);
                var stream = server.GetStream();

                byte[] code = new byte[3];

                byte[] serverToken = new byte[64];
                stream.Read(serverToken, 0, 64);
                byte[] clientToken = Encoding.ASCII.GetBytes(ToHex(GenerateToken(serverToken),false));
                stream.Write(clientToken);

                stream.Read(code, 0, 3);

                if (Encoding.ASCII.GetString(code) != TOKEN_SUCCESS)
                {
                    stream.Close();
                    server.Close();
                    server.Dispose();
                    return Content("<h1>ERROR OCCURED</h1>\n<h2>Invalid Token</h2>", "text/html");
                }
                stream.Write(Encoding.ASCII.GetBytes(START_DOWNLOAD));
                
                foreach (var song in database.songEntries)
                {
                    if (song.SongEntryId != null && song.SongURL != null)
                    {
                        Console.WriteLine("Name: " + song.SongName);
                        Console.WriteLine("URL: " + song.SongURL);

                        stream.Write(Encoding.ASCII.GetBytes(URL_START));
                        Thread.Sleep(500);

                        stream.Write(Encoding.ASCII.GetBytes(song.SongURL));
                        Thread.Sleep(500);

                        stream.Write(Encoding.ASCII.GetBytes(URL_END));
                        Thread.Sleep(500);

                        stream.Read(code, 0, 3);
                        Thread.Sleep(500);
                        
                        if (Encoding.ASCII.GetString(code) == URL_OK)
                        {
                            stream.Write(Encoding.ASCII.GetBytes(HASH_START));
                            Thread.Sleep(500);

                            stream.Write(Encoding.ASCII.GetBytes(song.SongEntryId));
                            Thread.Sleep(500);

                            stream.Write(Encoding.ASCII.GetBytes(HASH_END));
                            Thread.Sleep(500);

                            stream.Read(code, 0, 3);
                            Thread.Sleep(500);

                            if (Encoding.ASCII.GetString(code) == HASH_OK)
                            {
                                stream.Write(Encoding.ASCII.GetBytes(DOWNLOAD_MEDIA));
                                Thread.Sleep(500);
                                stream.Read(code, 0, 3);
                                Thread.Sleep(500);
                                if (Encoding.ASCII.GetString(code) == DOWNLOAD_COMPLETE)
                                {
                                    Console.WriteLine("DOWNLOADED: " + song.SongURL);
                                }
                                else if(Encoding.ASCII.GetString(code) == DOWNLOAD_ERROR)
                                {
                                    Console.WriteLine("ERROR DOWNLOADING: " + song.SongURL);
                                }
                            }
                        }
                    }
                }
                stream.Write(Encoding.ASCII.GetBytes(END_DOWNLOAD));
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
    
    public IActionResult DownloadThumbnail()
    {
        try
        {
            if (database.songEntries != null)
            {
                WebClient client = new WebClient();
                
                foreach (var song in database.songEntries)
                {
                    if (song.SongEntryId != null && song.SongThumbnail != null)
                    {
                        Console.WriteLine("Name: " + song.SongName);
                        Console.WriteLine("URL: " + song.SongURL);
                        client.DownloadFile(song.SongThumbnail, "Thumbs/" + song.SongEntryId + ".jpeg");
                    }
                }
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
