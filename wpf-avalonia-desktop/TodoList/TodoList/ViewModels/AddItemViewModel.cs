using System;
using System.Reactive;
using ReactiveUI;
using TodoList.Models;

namespace TodoList.ViewModels;

public class AddItemViewModel : ViewModelBase
{
    private string _description;
    public string Description   
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public ReactiveCommand<Unit, ToDoItem> OkCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

    public AddItemViewModel()
    {
        var isValidCommandOk = this.WhenAnyValue(
            x => x.Description,
            x => !string.IsNullOrWhiteSpace(x));
            
        OkCommand = ReactiveCommand.Create(
            () => new ToDoItem {Description = Description},
            isValidCommandOk);
        
        CancelCommand = ReactiveCommand.Create(() => { });
    }
}