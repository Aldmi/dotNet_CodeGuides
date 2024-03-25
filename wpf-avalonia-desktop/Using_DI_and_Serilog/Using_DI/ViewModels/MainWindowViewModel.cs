using System.Reactive;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Using_DI.Services;

namespace Using_DI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IFooService _fooService;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MainWindowViewModel(IFooService fooService, IServiceScopeFactory serviceScopeFactory)
    {
        _fooService = fooService;
        _serviceScopeFactory = serviceScopeFactory;
        IncrementCounterCommand = ReactiveCommand.Create(() =>
        {
            using var scope= _serviceScopeFactory.CreateScope();
            var fooServiceLocal= scope.ServiceProvider.GetRequiredService<IFooService>();
            Counter = fooServiceLocal.GetCounterValue();
            
            
            //Counter = _fooService.GetCounterValue();
        });
    }


    private int _counter;
    public int Counter
    {
        get => _counter;
        set => this.RaiseAndSetIfChanged(ref _counter, value);
    }
    
    
    public ICommand IncrementCounterCommand  { get; }
}