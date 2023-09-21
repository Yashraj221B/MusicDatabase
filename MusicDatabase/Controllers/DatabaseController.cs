using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MusicDatabase.Models;
using MusicDatabase.Data;
using System.Security.Cryptography;
using System.Text;
using System.Net.Sockets;

namespace MusicDatabase.Controllers;

public class DatabaseController : Controller
{
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
    // [ValidateAntiForgeryToken]
    public IActionResult Add(string SongName, string SongArtist, string SongAlbum, string SongReleased, string SongThumbnail, string SongURL)
    {
        SongEntry song = new SongEntry();
        song.SongName = SongName;
        song.SongArtist = SongArtist;
        song.SongAlbum = SongAlbum;
        song.SongReleased = SongReleased;
        song.SongThumbnail = SongThumbnail;
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
    public IActionResult Edit(string SongID, string SongName, string SongArtist, string SongAlbum, string SongReleased, string SongThumbnail, string SongURL)
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
                song.SongThumbnail = SongThumbnail;
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

    public IActionResult Download(string? Id)
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
        if (Id == null)
        {
            return Content("<h1>Please pass ID</h1>");
        }
        if (database.songEntries != null)
        {
            SongEntry? song = database.songEntries.Find(Id);
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
                    byte[] clientToken = Encoding.ASCII.GetBytes(ToHex(GenerateToken(serverToken), false));
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

                    if (song != null)
                    {

                        if (song.SongEntryId != null && song.SongURL != null)
                        {
                            Console.WriteLine("Name: " + song.SongName);
                            Console.WriteLine("URL: " + song.SongURL);

                            stream.Write(Encoding.ASCII.GetBytes(URL_START));
                            Thread.Sleep(1000);

                            stream.Write(Encoding.ASCII.GetBytes(song.SongURL));
                            Thread.Sleep(1000);

                            stream.Write(Encoding.ASCII.GetBytes(URL_END));
                            Thread.Sleep(1000);

                            stream.Read(code, 0, 3);
                            Thread.Sleep(1000);

                            if (Encoding.ASCII.GetString(code) == URL_OK)
                            {
                                stream.Write(Encoding.ASCII.GetBytes(HASH_START));
                                Thread.Sleep(1000);

                                stream.Write(Encoding.ASCII.GetBytes(song.SongEntryId));
                                Thread.Sleep(1000);

                                stream.Write(Encoding.ASCII.GetBytes(HASH_END));
                                Thread.Sleep(1000);

                                stream.Read(code, 0, 3);
                                Thread.Sleep(1000);

                                if (Encoding.ASCII.GetString(code) == HASH_OK)
                                {
                                    stream.Write(Encoding.ASCII.GetBytes(DOWNLOAD_MEDIA));
                                    Thread.Sleep(1000);
                                    stream.Read(code, 0, 3);
                                    Thread.Sleep(1000);
                                    if (Encoding.ASCII.GetString(code) == DOWNLOAD_COMPLETE)
                                    {
                                        Console.WriteLine("DOWNLOADED: " + song.SongURL);
                                        stream.Write(Encoding.ASCII.GetBytes(END_DOWNLOAD));
                                        return Content("<h1>DOWNLOADED SUCCESSFULLY</h1>", "text/html");
                                    }
                                    else if (Encoding.ASCII.GetString(code) == DOWNLOAD_ERROR)
                                    {
                                        Console.WriteLine("ERROR DOWNLOADING: " + song.SongURL);
                                        stream.Write(Encoding.ASCII.GetBytes(END_DOWNLOAD));
                                        return Content("<h1>DOWNLOAD FAILED</h1>", "text/html");
                                    }
                                }
                            }
                        }
                    }
                    stream.Close();
                    server.Close();
                    server.Dispose();
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                return Content($"<h1>ERROR OCCURED</h1>\n<h2>{e.Message}</h2>", "text/html");
            }
        }
        return Content("ERROR");
    }

    public IActionResult Search(string? searchTerm)
    {
        SongListModel songList = new SongListModel();
        int count = 0;
        SongEntry[] results = { };

        if (searchTerm == "" || searchTerm == null)
        {
            return Content("");
        }else if(searchTerm == "$ALL$")
        {
            songList.SongList = database.songEntries.ToArray();
            return View(songList);
        }

        if (database.songEntries != null)
        {
            foreach (var song in database.songEntries)
            {
                if (song != null)
                {
                    if (song.SongName != null)
                        if (song.SongName.ToLower().Contains(searchTerm.ToLower()))
                        {
                            results = results.Append<SongEntry>(song).ToArray();
                            count++;
                            continue;
                        }
                    if (song.SongAlbum != null)
                    {
                        if (song.SongAlbum.ToLower().Contains(searchTerm.ToLower()))
                        {
                            results = results.Append<SongEntry>(song).ToArray();
                            count++;
                            continue;
                        }
                    }
                    if (song.SongArtist != null)
                    {
                        if (song.SongArtist.ToLower().Contains(searchTerm.ToLower()))
                        {
                            results = results.Append<SongEntry>(song).ToArray();
                            count++;
                            continue;
                        }
                    }
                }
            }
            // songList.SongList = database.songEntries.ToArray();
            songList.SongList = results;
            return View(songList);
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
