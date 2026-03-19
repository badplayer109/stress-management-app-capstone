using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SchoolStressManagementApp.Services;
using SchoolStressManagementApp.Views;

namespace SchoolStressManagementApp.ViewModels;

public class SleepStatusViewModel : INotifyPropertyChanged
{
    private readonly GlobalStressManagementStatus _status;

    public double SleepHours 
    { 
        get => _status.SleepHours; 
        set => _status.SleepHours = value; 
    }
    public int SleepHoursOptimal => _status.SleepHoursOptimal;
    public int SleepHoursInit { get; } = 0;
    public ICommand ToMainPageCommand { get; }

    public SleepStatusViewModel(GlobalStressManagementStatus status)
    {
        _status = status;

        _status.PropertyChanged += OnStatusPropertyChanged;
        
        ToMainPageCommand = new Command(async () => await ToMainPage());
    }
    
    private void OnStatusPropertyChanged(Object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GlobalStressManagementStatus.SleepHours)) OnPropertyChanged(nameof(SleepHours));
        // if (e.PropertyName == nameof(GlobalStressManagementStatus.ExerciseProgress)) OnPropertyChanged(nameof(ExerciseProgress));
        // OnPropertyChanged(nameof(StatusOverview));
    }
    private async Task ToMainPage()
    {
        await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
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
