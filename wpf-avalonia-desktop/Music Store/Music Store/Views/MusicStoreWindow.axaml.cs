using System;
using Avalonia.ReactiveUI;
using Music_Store.ViewModels;
using ReactiveUI;

namespace Music_Store.Views;

public partial class MusicStoreWindow : ReactiveWindow<MusicStoreViewModel>
{
    public MusicStoreWindow()
    {
        InitializeComponent();
        this.WhenActivated(action => action(ViewModel!.BuyMusicCommand.Subscribe(Close))); //Команда BuyMusicCommand должна закрыть окно (аналогично нажатию на крестик.)
    }
}