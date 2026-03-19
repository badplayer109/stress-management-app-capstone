using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SchoolStressManagementApp.Services;
using SchoolStressManagementApp.Views;

namespace SchoolStressManagementApp.ViewModels;

public class HydrationStatusViewModel  : INotifyPropertyChanged
{
    private readonly GlobalStressManagementStatus _status;
    public double WaterIntake 
    { 
        get => _status.WaterIntake; 
        set => _status.WaterIntake = value; 
    }
    public int WaterIntakeOptimal => _status.WaterIntakeOptimal;
    public int WaterIntakeInit { get; } = 0;
    public ICommand ToMainPageCommand { get; }
    public HydrationStatusViewModel(GlobalStressManagementStatus status)
    {
        _status = status;

        _status.PropertyChanged += OnStatusPropertyChanged;

        ToMainPageCommand = new Command(async () => await ToMainPage());
    }
    
    private async Task ToMainPage()
    {
        await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
    }

    private void OnStatusPropertyChanged(Object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GlobalStressManagementStatus.WaterIntake)) OnPropertyChanged(nameof(WaterIntake));
        // if (e.PropertyName == nameof(GlobalStressManagementStatus.ExerciseProgress)) OnPropertyChanged(nameof(ExerciseProgress));
        // OnPropertyChanged(nameof(StatusOverview));
    }

    public void Dispose()
    {
        _status.PropertyChanged -= OnStatusPropertyChanged;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
