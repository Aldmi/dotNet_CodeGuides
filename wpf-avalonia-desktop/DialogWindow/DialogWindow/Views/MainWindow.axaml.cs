using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using DialogWindow.Models;
using DialogWindow.ViewModels;
using ReactiveUI;

namespace DialogWindow.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(action =>
            action(ViewModel!.ShowDialogInteraction.RegisterHandler(DoShowDialogAsync)));
    }
    
    
    private async Task DoShowDialogAsync(InteractionContext<DialogViewModel, Persone?> interaction)
    {
        var dialog = new DialogWindow
        {
            DataContext = interaction.Input  //установили DataContext равным DialogViewModel  
        };
        var result = await dialog.ShowDialog<Persone?>(this);
        interaction.SetOutput(result);
    }
}