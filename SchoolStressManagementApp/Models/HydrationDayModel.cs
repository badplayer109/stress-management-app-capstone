

namespace SchoolStressManagementApp.Models;

public class HydrationDayModel : TrackerDayBase
{
    private double waterIntake;
    public double WaterIntake
    {
        get { return waterIntake; }
        set
        {
            if (waterIntake != value)
            {
                waterIntake = value;
                DayPropertyChanged();
            }
        }
    }
}
