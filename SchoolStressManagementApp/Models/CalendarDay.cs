using System.ComponentModel;

namespace SchoolStressManagementApp.Models;

public class CalendarDay : INotifyPropertyChanged
{
    private bool _isSelected;

    public DateTime Date { get; set; }

    public int Day => Date.Day;

    public bool IsCurrentMonth { get; set; }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColorSelected)));
        }
    }

    public Color ColorSelected => IsSelected
    ? Color.FromRgb(180,180,30)
    : Colors.Purple;

    public event PropertyChangedEventHandler? PropertyChanged;
}
