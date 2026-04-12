using SchoolStressManagementApp.ViewModels;

namespace SchoolStressManagementApp.Views;

public partial class HydrationStatusPage : ContentPage
{
	public HydrationStatusPage(HydrationStatusViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}