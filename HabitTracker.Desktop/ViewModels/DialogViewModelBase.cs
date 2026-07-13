using System;

namespace HabitTracker.Desktop.ViewModels;

/// <summary>
/// Base para ViewModels de janelas de diálogo. Padroniza como a ViewModel
/// pede pra View fechar a janela, sem a ViewModel conhecer o tipo Window.
/// </summary>
public abstract class DialogViewModelBase : ViewModelBase
{
    public Action? Close { get; set; }
}