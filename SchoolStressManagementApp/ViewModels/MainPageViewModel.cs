using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SchoolStressManagementApp.Services;
using SchoolStressManagementApp.Views;

namespace SchoolStressManagementApp.ViewModels;

public class MainPageViewModel : INotifyPropertyChanged, IDisposable
{
    private readonly GlobalStressManagementStatus _status;
    public double WaterIntake => _status.WaterIntake;
    public int WaterIntakeOptimal => _status.WaterIntakeOptimal;
    public int WaterIntakeInit { get; } = 0;
    
    public double SleepHours => _status.SleepHours;
    public int SleepHoursOptimal => _status.SleepHoursOptimal;
    public int SleepHoursInit { get; } = 0;

    public string ExerciseLink => _status.ExerciseLink;
    public string ExerciseGuideMessage => _status.ExerciseGuideMessage;
    public double ExerciseProgress => _status.ExerciseProgress;

    public int StatusOverview => CalculateStatusOverview();
    public ICommand ToHydrationStatusPageCommand { get; }
    public ICommand ToSleepStatusPageCommand { get; }
    public ICommand ToExerciseGuidePageCommand { get; }
    
    public MainPageViewModel(GlobalStressManagementStatus status)
    {
        _status = status;

        _status.PropertyChanged += OnStatusPropertyChanged;
        
        ToHydrationStatusPageCommand = new Command(async () => await ToHydrationStatusPage());
        ToSleepStatusPageCommand = new Command(async () => await ToSleepStatusPage());
        ToExerciseGuidePageCommand = new Command(async () => await ToExerciseGuidePage());
    }
    
    private void OnStatusPropertyChanged(Object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GlobalStressManagementStatus.WaterIntake)) OnPropertyChanged(nameof(WaterIntake));
        if (e.PropertyName == nameof(GlobalStressManagementStatus.SleepHours)) OnPropertyChanged(nameof(SleepHours));
        if (e.PropertyName == nameof(GlobalStressManagementStatus.ExerciseLink)) OnPropertyChanged(nameof(ExerciseLink));
        if (e.PropertyName == nameof(GlobalStressManagementStatus.ExerciseProgress)) OnPropertyChanged(nameof(ExerciseProgress));
        OnPropertyChanged(nameof(StatusOverview));
    }

    private int CalculateStatusOverview()
    {
        double waterStatus = WaterIntake / WaterIntakeOptimal;
        if (waterStatus >= 1) waterStatus = 1;
        double sleepStatus = SleepHours / SleepHoursOptimal;
        if (sleepStatus >= 1) sleepStatus = 1;
        double exerciseStatus = ExerciseProgress / 100;
        // if (exerciseStatus >= 1) exerciseStatus = 1;
        double statusOverview = (waterStatus + sleepStatus + exerciseStatus) / 3 * 100;
        return (int)Math.Round(statusOverview, MidpointRounding.AwayFromZero);
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

    public void Dispose()
    {
        _status.PropertyChanged -= OnStatusPropertyChanged;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
