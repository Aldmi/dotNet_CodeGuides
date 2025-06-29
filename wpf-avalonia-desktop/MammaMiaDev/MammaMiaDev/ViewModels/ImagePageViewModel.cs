using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using MammaMiaDev.Helpers;

namespace MammaMiaDev.ViewModels;

public class ImagePageViewModel : ViewModelBase
{
	public Bitmap ImageSourceBtmLocal 
		=> ImageHelper.LoadFromResource("/Assets/snowHome.jpg");
	
	public Task<Bitmap?> ImageSourceBtmWeb 
		=> ImageHelper.LoadFromWeb("https://www.kerch.com.ru/image/Pictures/Articles/00103124/b_103124_003.jpg");
}