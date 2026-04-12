using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using SchoolStressManagementApp.Models;

namespace SchoolStressManagementApp.ViewModels;

public class CalendarViewModel : INotifyPropertyChanged
{

    public ObservableCollection<CalendarWeek> Weeks { get; set; } = new();

    private DateTime _currentMonth = DateTime.Today;
    public DateTime CurrentMonth
    {
        get => _currentMonth;
        set
        {
            if (_currentMonth != value)
            {
                _currentMonth = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentMonth)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MonthYear)));
            }
        }
    }

    private CalendarDay? _selectedDay;
    public CalendarDay? SelectedDay
    {
        get => _selectedDay;
        set
        {
            if (_selectedDay != value)
            {
                _selectedDay = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDay)));
            }
        }
    }

    public string MonthYear => CurrentMonth.ToString("MMMM yyyy");

    public ICommand PreviousMonthCommand { get; }
    public ICommand NextMonthCommand { get; }
    public ICommand SelectDayCommand { get; }

    public Action<DateTime?>? OnDateSelected;

    public CalendarViewModel()
    {
        PreviousMonthCommand = new Command(() =>
        {
            CurrentMonth = CurrentMonth.AddMonths(-1);
            GenerateCalendar();
        });

        NextMonthCommand = new Command(() =>
        {
            CurrentMonth = CurrentMonth.AddMonths(1);
            GenerateCalendar();
        });

        SelectDayCommand = new Command<CalendarDay>(SelectDay);

        GenerateCalendar();
    }

    private void SelectDay(CalendarDay day)
    {
        if (SelectedDay == day && day.IsSelected)
        {
            day.IsSelected = false;
            SelectedDay = null;
            OnDateSelected?.Invoke(null);
            return;
        }

        if (SelectedDay != null)
            SelectedDay.IsSelected = false;

        day.IsSelected = true;
        SelectedDay = day;

        OnDateSelected?.Invoke(day.Date);
    }

    public void SetSelectedDate(DateTime? date)
    {
        foreach (var week in Weeks)
        {
            foreach (var day in week.Days)
            {
                day.IsSelected = false;

                if (date != null && day.Date.Date == date.Value.Date)
                {
                    day.IsSelected = true;
                    SelectedDay = day;
                }
            }
        }
    }

    private void GenerateCalendar()
    {
        Weeks.Clear();

        DateTime firstDay = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 1);
        int firstDayOfWeek = (int)firstDay.DayOfWeek;
        DateTime start = firstDay.AddDays(-firstDayOfWeek);
        int daysInMonth = DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.Month);
        int weeksInMonth = (int)Math.Ceiling((firstDayOfWeek + daysInMonth) / 7.0);

        for (int week = 0; week < weeksInMonth; week++)
        {
            var calendarWeek = new CalendarWeek();

            for (int day = 0; day < 7; day++)
            {
                calendarWeek.Days.Add(new CalendarDay
                {
                    Date = start,
                    IsCurrentMonth = start.Month == CurrentMonth.Month
                });

                start = start.AddDays(1);
            }

            Weeks.Add(calendarWeek);
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MonthYear)));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
