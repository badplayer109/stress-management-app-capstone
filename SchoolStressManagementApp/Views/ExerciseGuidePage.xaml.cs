using SchoolStressManagementApp.ViewModels;

namespace SchoolStressManagementApp.Views;

public partial class ExerciseGuidePage : ContentPage
{
	public ExerciseGuidePage(ExerciseGuideViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}