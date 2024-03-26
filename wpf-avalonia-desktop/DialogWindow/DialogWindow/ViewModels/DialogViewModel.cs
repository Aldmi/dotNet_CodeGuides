using System.Reactive;
using DialogWindow.Models;
using ReactiveUI;

namespace DialogWindow.ViewModels;

/// <summary>
/// Диалоговое окно должно возвращать Persone
/// </summary>
public class DialogViewModel : ViewModelBase
{
    public DialogViewModel()
    {
        PersoneModel = new Persone();
        GetResultVmCommand = ReactiveCommand.Create(() =>
        {
            return PersoneModel;
        });
    }
    
    
    public ReactiveCommand<Unit, Persone> GetResultVmCommand { get; }
    
    
    /// <summary>
    /// ResultViewModel задает сразу модель для отображения, хотя можно вернуть и просто класс Model
    /// </summary>
    private Persone _personeModel;
    public Persone PersoneModel
    {
        get => _personeModel;
        set => this.RaiseAndSetIfChanged(ref _personeModel, value);
    }
}