using System;

namespace SchoolStressManagementApp.Models;

public class SleepDayModel : TrackerDayBase
{
    private TimeSpan? bedTime;
    private TimeSpan? wakeTime;
    private TimeSpan? sleepHours;

    public TimeSpan? BedTime
    {
        get => bedTime;
        set
        {
            if (bedTime != value)
            {
                bedTime = value;
                CalculateHours();
                DayPropertyChanged();
            }
        }
    }

    public TimeSpan? WakeTime
    {
        get => wakeTime;
        set
        {
            if (wakeTime != value)
            {
                wakeTime = value;
                CalculateHours();
                DayPropertyChanged();
            }
        }
    }
    
    public TimeSpan? SleepHours
    {
        get => sleepHours;
        set
        {
            if (sleepHours != value)
            {
                sleepHours = value;
                DayPropertyChanged();
            }
        }
    }

    public double GetSleepHours
    {
        get => SleepHours?.TotalHours ?? 0.0;
        set => SleepHours = TimeSpan.FromHours(value);
    }

    private void CalculateHours()
    {
        if (WakeTime == null || BedTime == null) return;
        TimeSpan? sleepDuration = WakeTime - BedTime;

        // If wake time is earlier than bed time, it means they woke up next day
        if (sleepDuration?.TotalSeconds < 0)
        {
            sleepDuration += TimeSpan.FromHours(24);
        }

        SleepHours = sleepDuration;
    }
}
