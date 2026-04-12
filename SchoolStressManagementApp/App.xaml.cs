using SchoolStressManagementApp.Services;

namespace SchoolStressManagementApp;

public partial class App : Application
{
	public App(GlobalStressManagementStatus status)
	{
		InitializeComponent();

		_ = InitializeAsync(status);
	}

	private async Task InitializeAsync(GlobalStressManagementStatus status)
    {
        try
        {
            await status.LoadAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}