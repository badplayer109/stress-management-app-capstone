using SchoolStressManagementApp.ViewModels;

namespace SchoolStressManagementApp.Views;

public partial class SleepStatusPage : ContentPage
{
	public SleepStatusPage(SleepStatusViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}