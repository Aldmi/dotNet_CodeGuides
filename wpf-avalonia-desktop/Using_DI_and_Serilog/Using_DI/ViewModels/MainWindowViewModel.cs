using Using_DI.Services;

namespace Using_DI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IRepository _repository;

    public MainWindowViewModel(IRepository repository)
    {
        _repository = repository;
    }

    public int GetEntityId => _repository.GetId;

}