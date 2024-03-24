using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using Music_Store.Models;
using ReactiveUI;
using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace Music_Store.ViewModels;

public class MusicStoreViewModel : ViewModelBase
{
    private string? _searchText;
    private bool _isBusy;
    private AlbumViewModel? _selectedAlbum;
    private CancellationTokenSource? _cancellationTokenSource;

    
    public MusicStoreViewModel()
    {
        //Каждый раз когда меняется поле SearchText (не чаще чам раз в 400мс), вызывается метод DoSearch
        this.WhenAnyValue(x => x.SearchText)
            .Throttle(TimeSpan.FromMilliseconds(400))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(DoSearch!);
        
        BuyMusicCommand = ReactiveCommand.Create(() => SelectedAlbum);
        
        //RxApp.MainThreadScheduler.Schedule(LoadAsyncTest);//DEBUG для обновления информации в UI
        //RxApp.TaskpoolScheduler.Schedule(LoadAsyncTest); // Для запуска фоновой службы, без обновления UI напрямую из этого потока.
    }
    
    // private async void LoadAsyncTest()
    // {
    // }

    public ObservableCollection<AlbumViewModel> SearchResults { get; } = new();
    
    public string? SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set => this.RaiseAndSetIfChanged(ref _isBusy, value);
    }
    
    public AlbumViewModel? SelectedAlbum
    {
        get => _selectedAlbum;
        set => this.RaiseAndSetIfChanged(ref _selectedAlbum, value);
    }
    
    public ReactiveCommand<Unit, AlbumViewModel?> BuyMusicCommand { get; }
    
    
    private async void DoSearch(string s)
    {
        IsBusy = true;
        SearchResults.Clear();
        
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;
        
        if (!string.IsNullOrWhiteSpace(s))
        {
            var albums = await Album.SearchAsync(s);
            foreach (var album in albums)
            {
                var vm = new AlbumViewModel(album);
                SearchResults.Add(vm);
            }
            
            if (!cancellationToken.IsCancellationRequested)
            {
                LoadCovers(cancellationToken);
            }
        }
        IsBusy = false;
    }
    
    
    /// <summary>
    /// Важное примечание: данный метод выполняет итерации по копии коллекции результатов поиска (создается методом ToList).
    /// Это связано с тем, что он выполняется асинхронно в другом потоке, а исходная коллекция может быть изменена другим поток в любое время.
    /// </summary>
    private async void LoadCovers(CancellationToken cancellationToken)
    {
        foreach (var album in SearchResults.ToList())
        {
            await album.LoadCover();
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
        }
    }
}