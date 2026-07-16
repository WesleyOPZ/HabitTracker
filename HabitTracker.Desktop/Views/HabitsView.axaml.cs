using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using HabitTracker.Desktop.ViewModels;

namespace HabitTracker.Desktop.Views;

public partial class HabitsView : UserControl
{
    private Point _dragStartPoint;
    private KanbanColumnViewModel? _draggedColumn;
    private bool _dragStarted;

    public HabitsView()
    {
        InitializeComponent();
    }

    private void ColumnHeader_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint((Control)sender!).Properties.IsLeftButtonPressed) return;

        _dragStartPoint = e.GetPosition(this);
        _dragStarted = false;

        if (sender is Control { DataContext: KanbanColumnViewModel column })
            _draggedColumn = column;
    }

    private async void ColumnHeader_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (_draggedColumn == null || _dragStarted) return;
        if (!e.GetCurrentPoint((Control)sender!).Properties.IsLeftButtonPressed) return;

        var current = e.GetPosition(this);
        var dx = current.X - _dragStartPoint.X;
        var dy = current.Y - _dragStartPoint.Y;
        if (Math.Sqrt(dx * dx + dy * dy) < 6) return;

        _dragStarted = true;

        // O arrasto é só dentro da mesma janela, então não precisamos
        // serializar a coluna no DataTransfer — é só um "gatilho" pra
        // API; o objeto real já está guardado em _draggedColumn.
        var data = new DataTransfer();
        data.Add(DataTransferItem.CreateText("column-reorder"));

        await DragDrop.DoDragDropAsync(e, data, DragDropEffects.Move);

        _draggedColumn = null;
        _dragStarted = false;
    }

    private void Column_DragOver(object? sender, DragEventArgs e)
    {
        e.DragEffects = _draggedColumn != null ? DragDropEffects.Move : DragDropEffects.None;
    }

    private void Column_Drop(object? sender, DragEventArgs e)
    {
        var draggedColumn = _draggedColumn;
        if (draggedColumn == null) return;
        if (sender is not Control { DataContext: KanbanColumnViewModel targetColumn }) return;
        if (ReferenceEquals(draggedColumn, targetColumn)) return;

        if (DataContext is MainWindowViewModel vm)
        {
            int targetIndex = vm.KanbanColumns.IndexOf(targetColumn);
            vm.MoveColumnTo(draggedColumn, targetIndex);
        }
    }
}