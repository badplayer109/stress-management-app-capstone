using System.Collections.ObjectModel;
using System.Windows.Input;
using SchoolStressManagementApp.Models;
using SchoolStressManagementApp.Services;
using SchoolStressManagementApp.Views;

namespace SchoolStressManagementApp.ViewModels;

public partial class HydrationStatusViewModel  : TrackerPageViewModelBase<HydrationDayModel>
{
    private readonly GlobalStressManagementStatus _status;
    protected override ObservableCollection<HydrationDayModel> Items => _status.Data.HydrationDays;
    
    private string waterIntakeDraftText = "0";
    public string WaterIntakeDraftText
    {
        get => waterIntakeDraftText;
        set
        {
            if (waterIntakeDraftText != value)
            {
                waterIntakeDraftText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(WaterIntakeDraft));
            }
        }
    }

    public double WaterIntakeDraft
    {
        get
        {
            if (double.TryParse(waterIntakeDraftText, out var parsed))
                return parsed;
            return 0;
        }
        set
        {
            if (Math.Abs(WaterIntakeDraft - value) > double.Epsilon)
            {
                waterIntakeDraftText = value.ToString("0");
                OnPropertyChanged(nameof(WaterIntakeDraftText));
                OnPropertyChanged();
            }
        }
    }

    public int WaterIntakeOptimal { get; } = 8;
    public int WaterIntakeInit { get; } = 0;
    public int WaterIntakeMax { get; } = 16;

    public ICommand RecordToDayCommand { get; }
    public ICommand ClearDayCommand { get; }
    
    public HydrationStatusViewModel(GlobalStressManagementStatus status)
    {
        _status = status;

        RecordToDayCommand = new Command(RecordToDay);
        ClearDayCommand = new Command(ClearDay);
        
        SelectToday();
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
            WaterIntakeDraft = CurrentDay.WaterIntake;
        }
        else
        {
            CurrentDay = new HydrationDayModel
            {
                Date = SelectedDate.Value,
            };
            WaterIntakeDraft = 0;
        }
    }

    private async void RecordToDay()
    {
        if (CurrentDay == null) return;

        if (WaterIntakeDraft != 0)
        {
            // Make the DayModel an item in Items if not found.
            if (!Items.Any(d => d.Date.Date == CurrentDay.Date.Date))
                Items.Add(CurrentDay);

            var existing = Items.FirstOrDefault(d => d.Date.Date == CurrentDay.Date.Date);

            existing?.WaterIntake = WaterIntakeDraft;
        }
        else
        {
            // if 0, then Remove item in Items if found.
            if (Items.Any(d => d.Date.Date == CurrentDay.Date.Date))
                Items.Remove(CurrentDay);
        }

        UpdateCurrentDay();
        await _status.SaveAsync();
    }

    private async void ClearDay()
    {
        if (CurrentDay == null) return;

        CurrentDay.WaterIntake = 0;
        
        Items.Remove(CurrentDay);
        
        UpdateCurrentDay();
        await _status.SaveAsync();
    }
}
