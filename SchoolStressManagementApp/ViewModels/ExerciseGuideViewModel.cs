using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SchoolStressManagementApp.Services;
using SchoolStressManagementApp.Views;

namespace SchoolStressManagementApp.ViewModels;

public class ExerciseGuideViewModel : INotifyPropertyChanged
{
    private readonly GlobalStressManagementStatus _status;
    private string _linkDraft = "";
    public string LinkDraft { get => _linkDraft; set { _linkDraft = value; OnPropertyChanged(); } }
    public ICommand SaveLinkCommand { get; }
    public ICommand DeleteLinkCommand { get; }

    public string ExerciseLink 
    { 
        get => _status.ExerciseLink; 
        set => _status.ExerciseLink = value; 
    }

    public double ExerciseProgress 
    { 
        get => _status.ExerciseProgress; 
        set => _status.ExerciseProgress = value; 
    }

    public string ExerciseGuideMessage => _status.ExerciseGuideMessage;

    public ExerciseGuideViewModel(GlobalStressManagementStatus status)
    {
        _status = status;

        _status.PropertyChanged += OnStatusPropertyChanged;

        SaveLinkCommand = new Command(SaveLink);
        DeleteLinkCommand = new Command(DeleteLink);
        ToMainPageCommand = new Command(async () => await ToMainPage());

        LinkDraft = ExerciseLink;
    }
    public ICommand ToMainPageCommand { get; }
    private async Task ToMainPage()
    {
        await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
    }
    
    public void SaveLink() { if (LinkDraft != ExerciseLink) ExerciseLink = LinkDraft; }

    public void DeleteLink() { if (LinkDraft != ""){ LinkDraft = ""; SaveLink(); }}

    private void OnStatusPropertyChanged(Object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(GlobalStressManagementStatus.ExerciseLink)) OnPropertyChanged(nameof(ExerciseLink));
        if (e.PropertyName == nameof(GlobalStressManagementStatus.ExerciseProgress)) OnPropertyChanged(nameof(ExerciseProgress));
    }

    public void Dispose()
    {
        _status.PropertyChanged -= OnStatusPropertyChanged;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
