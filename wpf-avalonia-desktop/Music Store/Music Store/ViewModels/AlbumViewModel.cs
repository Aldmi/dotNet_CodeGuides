using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Music_Store.Models;
using ReactiveUI;

namespace Music_Store.ViewModels;

public class AlbumViewModel : ViewModelBase
{
    private readonly Album _album;
    
    public AlbumViewModel(Album album)
    {
        _album = album;
    }

    public string Artist => _album.Artist;

    public string Title => _album.Title;
    
    private Bitmap? _cover;
    public Bitmap? Cover
    {
        get => _cover;
        private set => this.RaiseAndSetIfChanged(ref _cover, value);
    }
    
    
    public async Task LoadCover()
    {
        await using var imageStream = await _album.LoadCoverBitmapAsync();
        Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
    }
}

