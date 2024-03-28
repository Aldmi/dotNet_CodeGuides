using System;
using System.Reactive.Linq;
using System.Windows.Input;
using DialogWindow.Models;
using ReactiveUI;

namespace DialogWindow.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Варинат с ручным созданием зависимостей
    /// </summary>
    // public MainWindowViewModel()
    // {
    //     ShowDialogInteraction = new Interaction<DialogViewModel, Persone?>();
    //     ShowDialogWindowCommand = ReactiveCommand.CreateFromTask(async () =>
    //     {
    //         var dialogViewModel = new DialogViewModel();
    //         PersoneModel = await ShowDialogInteraction.Handle(dialogViewModel);
    //     });
    // }
    //
    
    /// <summary>
    /// Вариант с DI
    /// </summary>
    public MainWindowViewModel(Interaction<DialogViewModel, Persone?> showDialogInteraction)
    {
        ShowDialogInteraction = showDialogInteraction;
        ShowDialogWindowCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var dialogViewModel = new DialogViewModel();
            PersoneModel = await ShowDialogInteraction.Handle(dialogViewModel);
        });
    }
    
    
    public ICommand ShowDialogWindowCommand { get; }
    public Interaction<DialogViewModel, Persone?> ShowDialogInteraction { get; }
    
    
    private Persone? _personeModel;
    public Persone? PersoneModel
    {
        get => _personeModel;
        set => this.RaiseAndSetIfChanged(ref _personeModel, value);
    }
}