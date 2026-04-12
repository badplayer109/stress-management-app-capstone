using SchoolStressManagementApp.ViewModels;

namespace SchoolStressManagementApp.Views;

public partial class ExercisePlansPage : ContentPage
{
	public ExercisePlansPage(ExerciseGuideViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}