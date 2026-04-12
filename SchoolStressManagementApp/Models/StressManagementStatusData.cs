using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SchoolStressManagementApp.Models;

public class StressManagementStatusData// : INotifyPropertyChanged
{
    public ObservableCollection<HydrationDayModel> HydrationDays { get; set; } = new();
    public ObservableCollection<SleepDayModel> SleepDays { get; set; } = new();
    public ObservableCollection<ExerciseDayModel> ExerciseDays { get; set; } = new();
    public ObservableCollection<ExercisePlanModel> ExercisePlans { get; set; } = new();
    // private int _exercisePlanId = 0;
    // public int ExercisePlanId
    // {
    //     get { return _exercisePlanId; }
    //     set
    //     {
    //         if (_exercisePlanId != value)
    //         {
    //             _exercisePlanId = value;
    //             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExercisePlanId)));
    //         }
    //     } 
    // }

    // public event PropertyChangedEventHandler? PropertyChanged;
}