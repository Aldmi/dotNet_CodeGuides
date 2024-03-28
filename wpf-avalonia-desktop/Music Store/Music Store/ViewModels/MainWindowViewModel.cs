using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Windows.Input;
using Music_Store.Models;
using ReactiveUI;

namespace Music_Store.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ICommand BuyMusicCommand  { get; }
    public Interaction<MusicStoreViewModel, AlbumViewModel?> ShowDialogInteraction { get; }
    public ObservableCollection<AlbumViewModel> Albums { get; } = new();

    public MainWindowViewModel()
    {
        ShowDialogInteraction = new Interaction<MusicStoreViewModel, AlbumViewModel?>();
        BuyMusicCommand = ReactiveCommand.CreateFromTask(async () => {
            var store = new MusicStoreViewModel();
            var result = await ShowDialogInteraction.Handle(store);
            if (result != null)
            {
                Albums.Add(result);
                await result.SaveToDiskAsync();
            }
        });
        RxApp.MainThreadScheduler.Schedule(LoadAlbums);
    }
    
    private async void LoadAlbums()
    {
        var albums = (await Album.LoadCachedAsync()).Select(x => new AlbumViewModel(x));

        foreach (var album in albums)
        {
            Albums.Add(album);
        }

        foreach (var album in Albums.ToList())
        {
            await album.LoadCover();
        }
    }
}