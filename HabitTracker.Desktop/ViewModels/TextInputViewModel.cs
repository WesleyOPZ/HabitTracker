using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HabitTracker.Desktop.ViewModels;

public partial class TextInputViewModel : DialogViewModelBase {
    public string Title { get; }
    public string Label { get; }

    [ObservableProperty] private string _text;
    
    public bool Confirmed { get; private set; }

    public TextInputViewModel(string title, string label, string initialText = "") {
        Title = title;
        Label = label;
        Text = initialText;
    }

    [RelayCommand]
    private void Confirm() {
        if (string.IsNullOrWhiteSpace(Text)) return;
        Confirmed = true;
        Close?.Invoke();
    }

    [RelayCommand]
    private void Cancel() {
        Confirmed = false;
        Close?.Invoke();
    }

}