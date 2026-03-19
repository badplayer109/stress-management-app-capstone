using SchoolStressManagementApp.ViewModels;

namespace SchoolStressManagementApp.Views;

public partial class ExerciseGuidePage : ContentPage
{
	public ExerciseGuidePage(ExerciseGuideViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

		Unloaded += ContentPage_Unloaded;
	}

	void ContentPage_Unloaded(object? sender, EventArgs e)
	{
		if (BindingContext is IDisposable viewModel)
		{
			viewModel.Dispose();
		}
		Unloaded -= ContentPage_Unloaded;
	}
}