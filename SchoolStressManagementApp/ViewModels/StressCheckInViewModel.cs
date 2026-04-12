using System.ComponentModel;
using System.Runtime.CompilerServices;
using SchoolStressManagementApp.Services;

namespace SchoolStressManagementApp.ViewModels;

public partial class StressCheckInViewModel  : INotifyPropertyChanged
{
    private readonly GlobalStressManagementStatus _status;
    
    public StressCheckInViewModel(GlobalStressManagementStatus status)
    {
        _status = status;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
