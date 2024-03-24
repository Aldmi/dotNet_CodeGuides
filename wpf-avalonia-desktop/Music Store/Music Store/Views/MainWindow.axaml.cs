using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using Music_Store.ViewModels;
using ReactiveUI;

namespace Music_Store.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(action =>
            action(ViewModel!.ShowDialogInteraction.RegisterHandler(DoShowDialogAsync))); //регистрация обработчика для Interaction из ViewModel
    }
    
    private async Task DoShowDialogAsync(InteractionContext<MusicStoreViewModel, AlbumViewModel?> interaction)
    {
        var dialog = new MusicStoreWindow
        {
            DataContext = interaction.Input
        };

        var result = await dialog.ShowDialog<AlbumViewModel?>(this);
        interaction.SetOutput(result);
    }
}