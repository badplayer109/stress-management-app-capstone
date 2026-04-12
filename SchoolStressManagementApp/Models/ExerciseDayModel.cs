using System.Collections.ObjectModel;

namespace SchoolStressManagementApp.Models;

public class ExerciseDayModel : TrackerDayBase
{
    private int _walkingMinutes;
    private int _moderateMinutes;
    private int _vigorousMinutes;

    public int WalkingMinutes
    {
        get => _walkingMinutes;
        set
        {
            _walkingMinutes = value;
            DayPropertyChanged(nameof(WalkingMinutes));
        }
    }

    public int ModerateMinutes
    {
        get => _moderateMinutes;
        set
        {
            _moderateMinutes = value;
            DayPropertyChanged(nameof(ModerateMinutes));
        }
    }

    public int VigorousMinutes
    {
        get => _vigorousMinutes;
        set
        {
            _vigorousMinutes = value;
            DayPropertyChanged(nameof(VigorousMinutes));
        }
    }

    private ObservableCollection<ExercisePlanModel> _recordedPlans = new();
    public ObservableCollection<ExercisePlanModel> RecordedPlans
    {
        get => _recordedPlans;
        set
        {
            if (_recordedPlans != value)
            {
                _recordedPlans = value;
                DayPropertyChanged();
            }
        }
    }

    public double GetExerciseProgress()
    {
        double result = 0;
        int count = RecordedPlans.Count;
        if (count > 0)
        {
            foreach (ExercisePlanModel plan in RecordedPlans)
            {
                if (plan.ProgressMinutes.TotalMinutes != 0 && plan.TargetMinutes.TotalMinutes != 0 )
                result += plan.ProgressMinutes / plan.TargetMinutes;
            }
            result /= count;
        }
        return result;
    }
}
