using SchoolStressManagementApp.Views;

namespace SchoolStressManagementApp.Controls;

public partial class DefaultFrame : ContentView
{
	public DefaultFrame()
	{
		InitializeComponent();
	}

	private async void BackButton_Clicked(object sender, EventArgs e)
	{
		// Navigation.PopAsync();
		await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
	} 
}