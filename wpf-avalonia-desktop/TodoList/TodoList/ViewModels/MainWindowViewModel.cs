using System;
using System.Reactive.Linq;
using ReactiveUI;
using TodoList.Models;
using TodoList.Services;

namespace TodoList.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        var service = new ToDoListService();
        ToDoListVm = new ToDoListViewModel(service.GetItems());
        _contentVm = ToDoListVm;
    }
    
    private ViewModelBase _contentVm;
    public ViewModelBase ContentVm
    {
        get => _contentVm;
        private set => this.RaiseAndSetIfChanged(ref _contentVm, value);
    }
    
    public ToDoListViewModel ToDoListVm { get; }
    
    
    public void AddItem()
    {
        var addItemVm= new AddItemViewModel();

        var commandObs= Observable.Merge(
            addItemVm.CancelCommand.Select(_=> (ToDoItem?)null),
            addItemVm.OkCommand);

        var lifeTime=commandObs
            .Take(1)
            .Subscribe(newItem =>
            {
                if (newItem != null) {
                    ToDoListVm.ListItems.Add(newItem );
                }
                ContentVm = ToDoListVm;
            });

        ContentVm = addItemVm;
    }
}