using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SchoolStressManagementApp.Services;
using SchoolStressManagementApp.Models;
using SchoolStressManagementApp.Views;
using System.Collections.ObjectModel;

namespace SchoolStressManagementApp.ViewModels;

public partial class MainPageViewModel : INotifyPropertyChanged
{
    private readonly GlobalStressManagementStatus _status;
    
    private ObservableCollection<HydrationDayModel> HydrationDays => _status.Data.HydrationDays;
    private ObservableCollection<SleepDayModel> SleepDays => _status.Data.SleepDays;
    private ObservableCollection<ExerciseDayModel> ExerciseDays => _status.Data.ExerciseDays;
    private HydrationDayModel hydrationStatusToday = new() { WaterIntake = 0 };
    private SleepDayModel sleepStatusToday = new() { SleepHours=TimeSpan.FromMinutes(0) };
    private ExerciseDayModel exerciseStatusToday =  new();
    public HydrationDayModel HydrationStatusToday { get => hydrationStatusToday; set { if (hydrationStatusToday != value) { hydrationStatusToday = value; OnPropertyChanged(); } } }
    public SleepDayModel SleepStatusToday { get => sleepStatusToday; set { if (sleepStatusToday != value) { sleepStatusToday = value; OnPropertyChanged(); } } }
    public ExerciseDayModel ExerciseStatusToday { get => exerciseStatusToday; set { if (exerciseStatusToday != value) { exerciseStatusToday = value; OnPropertyChanged(); } } }

    private DateTime? _selectedDate;

    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (_selectedDate != value)
            {
                _selectedDate = value;
                OnPropertyChanged();
                UpdateJournalDays();
            }
        }
    }

    private double totalExerciseProgress = 0;
    public double TotalExerciseProgress
    {
        get => totalExerciseProgress;
        set
        {
            if (totalExerciseProgress != value)
            {
                totalExerciseProgress = value;
                OnPropertyChanged();
            }
        }
    }

    private int statusOverview;
    public int StatusOverview
    {
        get => statusOverview;
        set
        {
            if (statusOverview != value)
            {
                statusOverview = value;
                OnPropertyChanged();
            } 
        }
    }

    public int WaterIntakeMax { get; } = 16;
    public int WaterIntakeOptimal { get; }  = 8;
    public int WaterIntakeInit { get; } = 0;
    
    public int SleepHoursMax { get; } = 24;
    public int SleepHoursOptimal { get; } = 8;
    public int SleepHoursInit { get; } = 0;

    public ICommand ToHydrationStatusPageCommand { get; }
    public ICommand ToSleepStatusPageCommand { get; }
    public ICommand ToExerciseGuidePageCommand { get; }
    
    public MainPageViewModel(GlobalStressManagementStatus status)
    {
        _status = status;

        ToHydrationStatusPageCommand = new Command(async () => await ToHydrationStatusPage());
        ToSleepStatusPageCommand = new Command(async () => await ToSleepStatusPage());
        ToExerciseGuidePageCommand = new Command(async () => await ToExerciseGuidePage());

        SelectedDate = DateTime.Now.Date;
    }

    public void UpdateTotalProgress()
    {
        TotalExerciseProgress = ExerciseStatusToday.GetExerciseProgress() * 100;
    }

    private void CalculateStatusOverview()
    {
        UpdateTotalProgress();

        double waterStatus = HydrationStatusToday.WaterIntake / WaterIntakeOptimal;
        if (waterStatus >= 1) waterStatus = 1;

        double sleepStatus = SleepStatusToday.GetSleepHours / SleepHoursOptimal;
        if (sleepStatus >= 1) sleepStatus = 1;

        double exerciseStatus = TotalExerciseProgress;
        if (exerciseStatus >= 1) exerciseStatus = 1;

        double statusOverview = (waterStatus + sleepStatus + exerciseStatus) / 3 * 100;
        StatusOverview = (int)Math.Round(statusOverview, MidpointRounding.AwayFromZero);
    }

    private void UpdateJournalDays()
    {
        if (SelectedDate == null) return;
        HydrationStatusToday = HydrationDays.FirstOrDefault(h => h.Date.Date == SelectedDate.Value.Date) ?? new() { WaterIntake = 0 };
        SleepStatusToday = SleepDays.FirstOrDefault(s => s.Date.Date == SelectedDate.Value.Date) ?? new() { SleepHours = TimeSpan.FromMinutes(0) };
        ExerciseStatusToday = ExerciseDays.FirstOrDefault(e => e.Date.Date == SelectedDate.Value.Date) ?? new();

        CalculateStatusOverview();
    }

    private async Task ToHydrationStatusPage()
    {
        await Shell.Current.GoToAsync($"///{nameof(HydrationStatusPage)}");
    }
    
    private async Task  ToSleepStatusPage()
    {
        await Shell.Current.GoToAsync($"///{nameof(SleepStatusPage)}");
    }
    
    private async Task  ToExerciseGuidePage()
    {
        await Shell.Current.GoToAsync($"///{nameof(ExerciseGuidePage)}");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
