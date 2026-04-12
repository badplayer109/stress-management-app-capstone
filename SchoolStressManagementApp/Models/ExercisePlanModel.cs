using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SchoolStressManagementApp.Models;

public class ExercisePlanModel : INotifyPropertyChanged
{
    private string title = "";
    public string Title
    {
        get => title;
        set
        {
            if (title != value)
            {
                title = value;
                OnPropertyChanged();
            }
        }
    }
    
    private string exerciseLink = "";
    public string ExerciseLink 
    {
        get => exerciseLink;
        set
        {
            if (exerciseLink != value)
            {
                exerciseLink = value;
                OnPropertyChanged();
            }
        }
    }

    private ExerciseIntensity targetIntensity;
    public ExerciseIntensity TargetIntensity 
    {
        get => targetIntensity;
        set
        {
            if (targetIntensity != value)
            {
                targetIntensity = value;
                OnPropertyChanged();
            }
        }
    }

    private TimeSpan targetMinutes;
    public TimeSpan TargetMinutes 
    {
        get => targetMinutes;
        set
        {
            if (targetMinutes != value)
            {
                targetMinutes = value;
                OnPropertyChanged();
            }
        }
    }

    private TimeSpan progressMinutes;
    public TimeSpan ProgressMinutes 
    {
        get => progressMinutes;
        set
        {
            if (progressMinutes != value)
            {
                progressMinutes = value;
                OnPropertyChanged();
            }
        }
    }

    private int _id;
    public int Id
    {
        get => _id;
        set
        {
            if (_id != value)
            {
                _id = value;
                OnPropertyChanged();
            }
        }
    }

    private string? notes;
    public string? Notes
    {
        get => notes;
        set
        {
            if (notes != value)
            {
                notes = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
