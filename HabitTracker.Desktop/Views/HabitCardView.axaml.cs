using Avalonia.Controls;
using Avalonia.Interactivity;

namespace HabitTracker.Desktop.Views;

public partial class HabitCardView : UserControl
{
    public HabitCardView()
    {
        InitializeComponent();
    }

    private void ImageIcon_Click(object? sender, RoutedEventArgs e)
    {
        ImagePreviewPopup.PlacementTarget = ImageIconButton;
        ImagePreviewPopup.IsOpen = !ImagePreviewPopup.IsOpen;
    }
}