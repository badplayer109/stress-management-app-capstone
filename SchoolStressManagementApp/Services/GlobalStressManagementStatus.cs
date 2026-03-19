using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SchoolStressManagementApp.Services;

public class GlobalStressManagementStatus : INotifyPropertyChanged
{
    private double _waterIntake;
    public int WaterIntakeOptimal = 8;

    public double WaterIntake 
    {
        get { return _waterIntake; }
        set
        {
            if (value != _waterIntake)
            {
                _waterIntake = value;
                OnPropertyChanged();
            }
        }
    }

    private double _sleepHours;
    public int SleepHoursOptimal = 8;

    public double SleepHours 
    {
        get { return _sleepHours; }
        set
        {
            if (value != _sleepHours)
            {
                _sleepHours = value;
                OnPropertyChanged();
            }
        }
    }

    private string _exerciseLink = "";
    private double _exerciseProgress;

    public string ExerciseLink 
    {
        get { return _exerciseLink; }
        set
        {
            if (value != _exerciseLink)
            {
                _exerciseLink = value;
                OnPropertyChanged();
            }
        }
    }

    public double ExerciseProgress 
    {
        get { return _exerciseProgress; }
        set
        {
            if (value != _exerciseProgress)
            {
                _exerciseProgress = value;
                OnPropertyChanged();
            }
        }
    }
    
    public string ExerciseGuideMessage = "coming soon exercise scheduler...";

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
