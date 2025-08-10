using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MammaMiaDev.Services;

public class DialogWindow : Window
{
	public DialogWindow()
	{
		// Стилизация окна
		 Classes.Add("DialogWindow");
		 SizeToContent = SizeToContent.WidthAndHeight;
		 WindowStartupLocation = WindowStartupLocation.CenterOwner;
		 CanResize = false;

		// Подписка на сообщения
		WeakReferenceMessenger.Default.Register<CloseDialogMessage>(this, (r, m) =>
		{
			Close(m.Value); //возврат true/false из диалогового окна.
			WeakReferenceMessenger.Default.Unregister<CloseDialogMessage>(this);
		});
	}

	protected override void OnClosed(EventArgs e)
	{
		// Очистка подписок при закрытии
		WeakReferenceMessenger.Default.UnregisterAll(this);
		base.OnClosed(e);
	}
}

public class CloseDialogMessage : ValueChangedMessage<bool>
{
	/// <param name="dialogResult">true - подтверждение, false - отмена</param>
	public CloseDialogMessage(bool dialogResult) : base(dialogResult)
	{
	}
}