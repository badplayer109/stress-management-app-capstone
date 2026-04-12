using System;

namespace SchoolStressManagementApp.Models;

public class IPAQWeekSummary //: INotifyPropertyChanged
{
    public int VigorousMinutes { get; set; }

    public int ModerateMinutes { get; set; }

    public int WalkingMinutes { get; set; }

    public int TotalMET { get; set; }

    public string Status { get; set; } = "";
}
