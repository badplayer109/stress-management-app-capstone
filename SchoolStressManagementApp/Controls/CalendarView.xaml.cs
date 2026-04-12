using SchoolStressManagementApp.ViewModels;

namespace SchoolStressManagementApp.Controls;

public partial class CalendarView : ContentView
{
	private readonly CalendarViewModel _viewModel;

    public static readonly BindableProperty SelectedDateProperty =
        BindableProperty.Create(
            nameof(SelectedDate),
            typeof(DateTime?),
            typeof(CalendarView),
            null,
            BindingMode.TwoWay,
            propertyChanged: OnSelectedDateChanged);

    public DateTime? SelectedDate
    {
        get => (DateTime?)GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public CalendarView()
    {
        InitializeComponent();

        _viewModel = new CalendarViewModel();

        _viewModel.OnDateSelected = date =>
        {
            SelectedDate = date;
        };

        BindingContext = _viewModel;
    }

    private static void OnSelectedDateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CalendarView)bindable;

        if (newValue is DateTime date)
            control._viewModel.SetSelectedDate(date);
        else
            control._viewModel.SetSelectedDate(null);
    }
}