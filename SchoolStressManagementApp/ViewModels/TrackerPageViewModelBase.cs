using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SchoolStressManagementApp.Models;
using System.Runtime.CompilerServices;

namespace SchoolStressManagementApp.ViewModels;

public abstract class TrackerPageViewModelBase<T> : INotifyPropertyChanged
    where T : TrackerDayBase, new()
{
    protected DateTime? _selectedDate;

    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (_selectedDate != value)
            {
                _selectedDate = value;
                OnPropertyChanged();
                LoadOrCreateDay();
            }
        }
    }

    protected T? _currentDay;

    public T? CurrentDay
    {
        get => _currentDay;
        set
        {
            _currentDay = value;
            OnPropertyChanged();
        }
    }

    protected abstract ObservableCollection<T> Items { get; }

    public ICommand SaveCommand { get; }

    protected TrackerPageViewModelBase()
    {
        SaveCommand = new Command(SaveDay);
    }
    protected virtual void SelectToday()
    {
        if (Items != null)
        SelectedDate = DateTime.Now.Date;
    }

    protected virtual void LoadOrCreateDay()
    {
        if (SelectedDate == null)
        {
            CurrentDay = null;
            return;
        }

        var existing = Items.FirstOrDefault(x => x.Date.Date == SelectedDate.Value.Date);

        if (existing != null)
        {
            CurrentDay = existing;
        }
        else
        {
            CurrentDay = new T
            {
                Date = SelectedDate.Value
            };
        }
    }

    protected virtual void UpdateCurrentDay()
    {
        try
        {
            DateTime? date = SelectedDate?.Date;
            SelectedDate = null;
            if (date != null)
            SelectedDate = date;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }

    private void SaveDay()
    {
        if (CurrentDay == null) return;

        if (!Items.Contains(CurrentDay))
        {
            Items.Add(CurrentDay);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}