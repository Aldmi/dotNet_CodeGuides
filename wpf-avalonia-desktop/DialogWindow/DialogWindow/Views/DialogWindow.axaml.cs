using System;
using Avalonia.ReactiveUI;
using DialogWindow.ViewModels;
using ReactiveUI;

namespace DialogWindow.Views;

public partial class DialogWindow : ReactiveWindow<DialogViewModel>
{
    public DialogWindow()
    {
        InitializeComponent();
        //Закрываем диалоговое окно при вызове команды GetResultVmCommand
        this.WhenActivated(action =>
            action(ViewModel!.GetResultVmCommand.Subscribe(Close)));
    }
}