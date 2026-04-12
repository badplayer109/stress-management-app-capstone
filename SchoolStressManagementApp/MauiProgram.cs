using Microsoft.Extensions.Logging;
using SchoolStressManagementApp.Services;
using SchoolStressManagementApp.ViewModels;
using SchoolStressManagementApp.Views;

namespace SchoolStressManagementApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		
		builder.Services.AddSingleton<GlobalStressManagementStatus>();

		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddTransient<HydrationStatusViewModel>();
		builder.Services.AddTransient<SleepStatusViewModel>();
		builder.Services.AddSingleton<ExerciseGuideViewModel>();
		// builder.Services.AddTransient<StressCheckInViewModel>();

		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<HydrationStatusPage>();
		builder.Services.AddTransient<SleepStatusPage>();
		builder.Services.AddTransient<ExerciseGuidePage>();
		builder.Services.AddTransient<ExercisePlansPage>();
		// builder.Services.AddTransient<StressCheckInPage>();

		return builder.Build();
	}
}
