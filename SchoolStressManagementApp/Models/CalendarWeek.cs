using System.Collections.ObjectModel;

namespace SchoolStressManagementApp.Models;

public class CalendarWeek
{
    public ObservableCollection<CalendarDay> Days { get; set; } = new();
}
