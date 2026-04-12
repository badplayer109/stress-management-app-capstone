using System.Collections.ObjectModel;
using System.Windows.Input;
using SchoolStressManagementApp.Services;
using SchoolStressManagementApp.Views;
using SchoolStressManagementApp.Models;

namespace SchoolStressManagementApp.ViewModels;

public partial class SleepStatusViewModel : TrackerPageViewModelBase<SleepDayModel>
{
    private readonly GlobalStressManagementStatus _status;
    protected override ObservableCollection<SleepDayModel> Items => _status.Data.SleepDays;

    public int SleepHoursMax { get; } = 24;
    public int SleepHoursOptimal { get; } = 8;
    public int SleepHoursInit { get; } = 0;

    private TimeSpan bedTimeDraft = new(22, 0, 0);
    private TimeSpan wakeTimeDraft = new(6, 0, 0);
    private TimeSpan? sleepHoursDraft;
    private double getSleepHoursDraft;

    public TimeSpan BedTimeDraft
    {
        get => bedTimeDraft;
        set
        {
            if (bedTimeDraft != value)
            {
                bedTimeDraft = value;
                OnPropertyChanged();
                CalculateHours();
            }
        }
    }

    public TimeSpan WakeTimeDraft
    {
        get => wakeTimeDraft;
        set
        {
            if (wakeTimeDraft != value)
            {
                wakeTimeDraft = value;
                OnPropertyChanged();
                CalculateHours();
            }
        }
    }
    
    public TimeSpan? SleepHoursDraft
    {
        get => sleepHoursDraft;
        set
        {
            if (sleepHoursDraft != value)
            {
                sleepHoursDraft = value;
                OnPropertyChanged();
            }
        }
    }

    public double GetSleepHoursDraft
    {
        get => getSleepHoursDraft;
        set
        {
            if (getSleepHoursDraft != value)
            {
                getSleepHoursDraft = value;
                OnPropertyChanged();
            }
        }
    }
    
    public ICommand RecordToDayCommand { get; }
    public ICommand ClearDayCommand { get; }

    public SleepStatusViewModel(GlobalStressManagementStatus status)
    {
        _status = status;
        
        RecordToDayCommand = new Command(RecordToDay);
        ClearDayCommand = new Command(ClearDay);

        CalculateHours();
        SelectToday();
    }

    private void CalculateHours()
    {
        TimeSpan sleepDuration = WakeTimeDraft - BedTimeDraft;

        // If wake time is earlier than bed time, it means they woke up next day
        if (sleepDuration.TotalSeconds < 0)
        {
            sleepDuration += TimeSpan.FromHours(24);
        }

        SleepHoursDraft = sleepDuration;
        GetSleepHoursDraft = sleepDuration.TotalHours;
    }

    protected override void LoadOrCreateDay()
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
            BedTimeDraft = CurrentDay.BedTime ?? TimeSpan.FromHours(22);
            WakeTimeDraft = CurrentDay.WakeTime ?? TimeSpan.FromHours(6);

        }
        else
        {
            CurrentDay = new SleepDayModel
            {
                Date = SelectedDate.Value,
            };
            BedTimeDraft = TimeSpan.FromHours(22);
            WakeTimeDraft = TimeSpan.FromHours(6);
        }
    }
    
    private async void RecordToDay()
    {
        if (CurrentDay == null ) return;
        
        // Make the DayModel an item in Items if not found.
        if (!Items.Any(d => d.Date.Date == CurrentDay.Date.Date))
            Items.Add(CurrentDay);

        CurrentDay.WakeTime = WakeTimeDraft;
        CurrentDay.BedTime = BedTimeDraft;

        UpdateCurrentDay();
        await _status.SaveAsync();
    }

    private async void ClearDay()
    {
        if (CurrentDay == null) return;

        CurrentDay.WakeTime = WakeTimeDraft;
        CurrentDay.BedTime = BedTimeDraft;
        
        Items.Remove(CurrentDay);
        
        UpdateCurrentDay();
        await _status.SaveAsync();
    }
}
