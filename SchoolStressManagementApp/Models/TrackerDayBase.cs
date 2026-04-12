using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SchoolStressManagementApp.Models;

public abstract class TrackerDayBase : INotifyPropertyChanged
{
    public DateTime Date { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void DayPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
