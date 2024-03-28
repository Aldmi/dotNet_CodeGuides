using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using iTunesSearch.Library;

namespace Music_Store.Models;

public class Album
{
    private static readonly iTunesSearchManager SearchManager = new();
    private static HttpClient _httpClient = new();
    private string CachePath => $"./Cache/{Artist} - {Title}";
    
    public string Artist { get; set; }
    public string Title { get; set; }
    public string CoverUrl { get; set; }

    public Album(string artist, string title, string coverUrl)
    {
        Artist = artist;
        Title = title;
        CoverUrl = coverUrl;
    }
    
    
    public static async Task<IEnumerable<Album>> SearchAsync(string searchTerm)
    {
        var query = await SearchManager.GetAlbumsAsync(searchTerm)
            .ConfigureAwait(false);
                
        return query.Albums.Select(x =>
            new Album(x.ArtistName, x.CollectionName, 
                x.ArtworkUrl100.Replace("100x100bb", "600x600bb")));
    }
    
    public async Task<Stream> LoadCoverBitmapAsync()
    {
        if (File.Exists(CachePath + ".bmp"))
        {
            return File.OpenRead(CachePath + ".bmp");
        }
        var data = await _httpClient.GetByteArrayAsync(CoverUrl);
        return new MemoryStream(data);
    }

    
    //-----------SAVE cash to disk----------------------------
    public async Task SaveCashAsync()
    {
        if (!Directory.Exists("./Cache"))
        {
            Directory.CreateDirectory("./Cache");
        }
        await using var fs = File.OpenWrite(CachePath);
        await SaveToStreamAsync(this, fs);
    }
    
    public Stream SaveCoverBitmapStream()
    {
        return File.OpenWrite(CachePath + ".bmp");
    }

    private static async Task SaveToStreamAsync(Album data, Stream stream)
    {
        await JsonSerializer.SerializeAsync(stream, data).ConfigureAwait(false);
    }
    
    
    //-----------LOAD cash from disk----------------------------
    public static async Task<IEnumerable<Album>> LoadCachedAsync()
    {
        if (!Directory.Exists("./Cache"))
        {
            Directory.CreateDirectory("./Cache");
        }

        var results = new List<Album>();

        foreach (var file in Directory.EnumerateFiles("./Cache"))
        {
            if (!string.IsNullOrWhiteSpace(new DirectoryInfo(file).Extension)) continue;

            await using var fs = File.OpenRead(file);
            results.Add(await LoadFromStream(fs).ConfigureAwait(false));
        }

        return results;
    }
    
    
    public static async Task<Album> LoadFromStream(Stream stream)
    {
        return (await JsonSerializer.DeserializeAsync<Album>(stream).ConfigureAwait(false))!;
    }
}