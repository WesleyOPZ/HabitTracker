using System;
using CommunityToolkit.Mvvm.ComponentModel;
using HabitTracker.Core.Models;

namespace HabitTracker.Desktop.Models;

public partial class SelectableAchievement : ObservableObject
{
    public Achievement Achievement { get; }
    
    [ObservableProperty] private bool _isSelected;
    
    private readonly Func<bool> _canSelectMore;

    public SelectableAchievement(Achievement achievement, bool isSelected, Func<bool> canSelectMore)
    {
        Achievement = achievement;
        _isSelected = isSelected;
        _canSelectMore = canSelectMore;
    }
    
    /// Pode ser clicado se JÁ está selecionado (para desmarcar)
    /// OU se o limite ainda não foi atingido (para marcar).
    public bool CanToggle => _isSelected || _canSelectMore();
    
    /// Chamado pelo ViewModel após cada toggle para avisar a UI.
    public void NotifyCanToggleChanged()  => OnPropertyChanged(nameof(CanToggle));
    
}