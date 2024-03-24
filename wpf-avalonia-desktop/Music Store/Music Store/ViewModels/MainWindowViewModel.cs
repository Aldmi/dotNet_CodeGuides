using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
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
            }
        });
    }
}