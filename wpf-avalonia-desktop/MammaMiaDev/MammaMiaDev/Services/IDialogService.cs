using System.Threading.Tasks;

namespace MammaMiaDev.Services;

public interface IDialogService
{
	/// <summary>
	/// Открыть  диалоговое окно
	/// </summary>
	/// <param name="viewModel">View model окна</param>
	/// <typeparam name="TViewModel"></typeparam>
	/// <returns>true - CloseDialogMessage подтверждение; false- CloseDialogMessage отмена; null - закрыли окно без передачи CloseDialogMessage </returns>
	Task<bool?> ShowDialogAsync<TViewModel>(TViewModel viewModel) 
		where TViewModel : class;
}