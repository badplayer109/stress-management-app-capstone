using System.Collections.ObjectModel;
using System.Windows.Input;
using SchoolStressManagementApp.Models;
using SchoolStressManagementApp.Services;
using SchoolStressManagementApp.Views;

namespace SchoolStressManagementApp.ViewModels;

public partial class ExerciseGuideViewModel : TrackerPageViewModelBase<ExerciseDayModel>
{
    private readonly GlobalStressManagementStatus _status;
    protected override ObservableCollection<ExerciseDayModel> Items => _status.Data.ExerciseDays;
    public ObservableCollection<ExercisePlanModel> Plans => _status.Data.ExercisePlans;
    public IList<ExerciseIntensity> Intensities { get; } = 
        Enum.GetValues<ExerciseIntensity>().Cast<ExerciseIntensity>().ToList();
    
    #region Exercise Plan Draft Properties
    private string? _titleDraft = "";
    public string? TitleDraft { get => _titleDraft; set { _titleDraft = value; OnPropertyChanged(); } }

    private string? _linkDraft = "";
    public string? LinkDraft { get => _linkDraft; set { _linkDraft = value; OnPropertyChanged(); } }

    private ExerciseIntensity _targetIntensityDraft;
    public ExerciseIntensity TargetIntensityDraft { get => _targetIntensityDraft; set { _targetIntensityDraft = value; OnPropertyChanged(); } }

    private TimeSpan targetMinutesDraft = TimeSpan.FromMinutes(30);
    public TimeSpan TargetMinutesDraft { get => targetMinutesDraft; set { targetMinutesDraft = value; OnPropertyChanged();  OnPropertyChanged(nameof(TargetProgress)); } }

    private TimeSpan progressMinutesDraft = TimeSpan.FromMinutes(0);
    public TimeSpan ProgressMinutesDraft { get => progressMinutesDraft; set { progressMinutesDraft = value; OnPropertyChanged(); OnPropertyChanged(nameof(ExerciseProgress)); } }

    private string? _notesDraft = "";
    public string? NotesDraft { get => _notesDraft; set { _notesDraft = value; OnPropertyChanged(); } }

    public double TargetProgress
    {
        get => TargetMinutesDraft.TotalMinutes;
        set => TargetMinutesDraft = TimeSpan.FromMinutes(value);
    }
    
    public double ExerciseProgress
    {
        get => ProgressMinutesDraft.TotalMinutes;
        set => ProgressMinutesDraft = TimeSpan.FromMinutes(value);
    }

    // Selected Plan is connected to the collection view item
    private ExercisePlanModel? _selectedPlan;

    public ExercisePlanModel? SelectedPlan
    {
        get => _selectedPlan;
        set
        {
            if (_selectedPlan != value)
            {
                _selectedPlan = value;
                OnPropertyChanged();
                UpdateCurrentPlan();
            }
        }
    }

    // Current Plan is modifyable through the plan draft section without immediately changing the collectionview item
    private ExercisePlanModel? _currentPlan;

    public ExercisePlanModel? CurrentPlan
    {
        get => _currentPlan;
        set
        {
            _currentPlan = value;
            OnPropertyChanged();
        }
    }
    #endregion


    public string ExerciseGuideMessage => "Select an exercise plan. Then go to the calendar and pick a date. Schedule or record your exercise progress using the controls.";
    public string CurrentDayWalkingMinutes => CurrentDay?.WalkingMinutes.ToString() ?? "No Record";
    public string CurrentDayModerateMinutes => CurrentDay?.ModerateMinutes.ToString() ?? "No Record";
    public string CurrentDayVigorousMinutes => CurrentDay?.VigorousMinutes.ToString() ?? "No Record";

    private int exercisePlanId;
    public int ExercisePlanId
    {
        get { return exercisePlanId; }
        set
        {
            if (exercisePlanId != value)
            {
                exercisePlanId = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SavePlanCommand { get; }
    public ICommand ClearPlanCommand { get; }
    public ICommand DeletePlanCommand { get; }
    public ICommand RecordPlanToDayCommand { get; }
    public ICommand UnrecordPlanToDayCommand { get; }
    public ICommand ClearDayCommand { get; }
    public ICommand DeletePlaninDayCommand { get; }
    public ICommand ToMainPageCommand { get; }
    public ICommand ToExercisePlansPageCommand { get; }
    public ICommand ToExerciseGuidePageCommand { get; }

    public ExerciseGuideViewModel(GlobalStressManagementStatus status)
    {
        _status = status;

        ToMainPageCommand = new Command(async () => await ToMainPage());
        ToExercisePlansPageCommand = new Command(async () => await ToPlansPage());
        ToExerciseGuidePageCommand = new Command(async () => await ToGuidePage());
        SavePlanCommand = new Command(SavePlan);
        ClearPlanCommand = new Command(ClearPlan);
        DeletePlanCommand = new Command<ExercisePlanModel>(DeletePlan);
        RecordPlanToDayCommand = new Command(RecordPlanToDay);
        UnrecordPlanToDayCommand = new Command(UnrecordPlanToDay);
        ClearDayCommand = new Command(ClearDay);
        DeletePlaninDayCommand = new Command<ExercisePlanModel>(DeletePlaninDay);

        SelectToday();
    }
    
    private void GetPlanId()
    {
        int _exercisePlanId = 1;
        foreach (ExercisePlanModel plan in Plans)
        {
            if (plan.Id == _exercisePlanId) _exercisePlanId++;
        }
        if (exercisePlanId != _exercisePlanId)
        ExercisePlanId = _exercisePlanId;
    }

    private void UpdateCurrentPlan()
    {
        if (SelectedPlan == null)
        {
            ClearPlan();
            return;
        }

        var existing = Plans.FirstOrDefault(p => p.Id == SelectedPlan.Id);

        if (existing != null)
        {
            CurrentPlan = new ExercisePlanModel
            {
                Title = existing.Title,
                ExerciseLink = existing.ExerciseLink,
                TargetIntensity = existing.TargetIntensity,
                TargetMinutes = existing.TargetMinutes,
                Id = existing.Id,
                Notes = existing.Notes
            };
            TitleDraft = CurrentPlan.Title;
            LinkDraft = CurrentPlan.ExerciseLink;
            TargetIntensityDraft = CurrentPlan.TargetIntensity;
            TargetMinutesDraft = CurrentPlan.TargetMinutes;
            if (CurrentPlan.ProgressMinutes != TimeSpan.FromMinutes(0))
            ProgressMinutesDraft = CurrentPlan.ProgressMinutes;
            else
            {
                TimeSpan old = ProgressMinutesDraft;
                ProgressMinutesDraft = old;
            }
            NotesDraft = CurrentPlan.Notes;
        }
        else
        {
            CurrentPlan = new ExercisePlanModel
            {
                Title = TitleDraft ?? "",
                ExerciseLink = LinkDraft ?? "",
                TargetIntensity = TargetIntensityDraft,
                TargetMinutes = TargetMinutesDraft
            };
        }
    }

    private async void SavePlan()
    {
        if (TargetIntensityDraft == ExerciseIntensity.None || TitleDraft == "" || TitleDraft == null)
        {
            return;
        }

        if (SelectedPlan != null && Plans.Any(p => p.Id == SelectedPlan.Id))
        {
            SelectedPlan.Title = TitleDraft;
            SelectedPlan.ExerciseLink = LinkDraft ?? "";
            SelectedPlan.TargetIntensity = TargetIntensityDraft;
            SelectedPlan.TargetMinutes = TargetMinutesDraft;
            SelectedPlan.Notes = NotesDraft;
            UpdateCurrentPlan();
            await _status.SaveAsync();
        }
        else 
        {
            GetPlanId();
            CurrentPlan = new ExercisePlanModel
            {
                Title = TitleDraft ?? "",
                ExerciseLink = LinkDraft ?? "",
                TargetIntensity = TargetIntensityDraft,
                TargetMinutes = TargetMinutesDraft,
                Id = ExercisePlanId,
                Notes = NotesDraft
            };
            Plans.Add(CurrentPlan);
            _selectedPlan = Plans.FirstOrDefault(p => p.Id == CurrentPlan.Id);
            OnPropertyChanged(nameof(SelectedPlan));
            await _status.SaveAsync();
            ClearPlan();
        }
    }

    private void ClearPlan()
    {
        if (SelectedPlan != null && Plans.Any(p => p.Id == SelectedPlan.Id))
        {
            CurrentPlan = null;
            SelectedPlan = null;
            TitleDraft = "";
            LinkDraft = "";
            TargetIntensityDraft = ExerciseIntensity.None;
            TargetMinutesDraft = TimeSpan.FromMinutes(0);
            ProgressMinutesDraft = TimeSpan.FromMinutes(0);
            NotesDraft = null;
        }
    }

    private async void DeletePlan(ExercisePlanModel plan)
    {
        ClearPlan();
        
        if (Plans.Any(p => p.Id == plan.Id))
        {
            Plans.Remove(plan);
            await _status.SaveAsync();
        }
    }

    protected override void LoadOrCreateDay()
    {
        if (SelectedDate == null)
        {
            CurrentDay = null;
            return;
        }

        var existing = Items.FirstOrDefault(x => x.Date.Date == SelectedDate.Value.Date);

        if (existing != null)
        {
            CurrentDay = existing;
            var topPlan = CurrentDay.RecordedPlans.FirstOrDefault();
            if (topPlan == null) return;
            SelectedPlan = Plans.FirstOrDefault(p => p.Id == topPlan.Id);
            ProgressMinutesDraft = topPlan.ProgressMinutes;
            NotesDraft = topPlan.Notes;
        }
        else
        {
            CurrentDay = new ExerciseDayModel
            {
                Date = SelectedDate.Value,
            };
        }
    }
    
    protected override void UpdateCurrentDay()
    {
        // Check if CurrentDay and CurrentPlan is not null.
        ExerciseIntensity currentIntensity = CurrentPlan!.TargetIntensity;

        switch (currentIntensity)
        {
            case ExerciseIntensity.Walking:
                    CurrentDay!.WalkingMinutes = 0;
                    break;
                case ExerciseIntensity.Moderate:
                    CurrentDay!.ModerateMinutes = 0;
                    break;
                case ExerciseIntensity.Vigorous:
                    CurrentDay!.VigorousMinutes = 0;
                    break;
        }
        
        foreach (ExercisePlanModel plan in CurrentDay!.RecordedPlans)
        {
            ExerciseIntensity intensity = plan.TargetIntensity;

            if (intensity != currentIntensity) continue;
            
            switch (intensity)
            {
                case ExerciseIntensity.Walking:
                    CurrentDay.WalkingMinutes += (int)plan.ProgressMinutes.TotalMinutes;
                    break;
                case ExerciseIntensity.Moderate:
                    CurrentDay.ModerateMinutes += (int)plan.ProgressMinutes.TotalMinutes;
                    break;
                case ExerciseIntensity.Vigorous:
                    CurrentDay.VigorousMinutes += (int)plan.ProgressMinutes.TotalMinutes;
                    break;
            }
        }
        
        base.UpdateCurrentDay();
    }
    
    private async void RecordPlanToDay()
    {
        if (CurrentPlan == null || CurrentDay == null ) return;
        
        // Make the DayModel an item in Items if not found.
        if (!Items.Any(d => d.Date.Date == CurrentDay.Date.Date))
            Items.Add(CurrentDay);

        CurrentPlan.ProgressMinutes = ProgressMinutesDraft;
        CurrentPlan.Notes = NotesDraft;

        // Update the the DayModel's Plan Items.
        var existingPlan = CurrentDay.RecordedPlans.FirstOrDefault(p => p.Id == CurrentPlan.Id);

        if (existingPlan != null)
        {
            existingPlan.ProgressMinutes = CurrentPlan.ProgressMinutes;
            existingPlan.Notes = CurrentPlan.Notes ?? NotesDraft;
        }
        else
            CurrentDay.RecordedPlans.Add(CurrentPlan);

        UpdateCurrentDay();
        await _status.SaveAsync();
    }

    private async void UnrecordPlanToDay()
    {
        if (CurrentPlan == null || CurrentDay == null) return;
        
        if (CurrentDay.RecordedPlans.Count <= 1)
        {
            if (!CurrentDay.RecordedPlans.Any(p => p.Id == CurrentPlan.Id)) return;
            Items.Remove(CurrentDay);
        }
        else 
        {
            var existingPlan = CurrentDay.RecordedPlans.FirstOrDefault(p => p.Id == CurrentPlan.Id);
            if (existingPlan == null) return;
            CurrentDay.RecordedPlans.Remove(existingPlan);
        }

        UpdateCurrentDay();
        await _status.SaveAsync();
    }

    private async void ClearDay()
    {
        if (CurrentDay == null) return;

        CurrentDay.RecordedPlans.Clear();
        CurrentDay.WalkingMinutes = 0;
        CurrentDay.ModerateMinutes = 0;
        CurrentDay.VigorousMinutes = 0;
        
        Items.Remove(CurrentDay);
        
        await _status.SaveAsync();
    }

    private async void DeletePlaninDay(ExercisePlanModel plan)
    {
        if (CurrentDay == null) return;
        
        var existingPlan = CurrentDay.RecordedPlans.FirstOrDefault(p => p.Id == plan.Id);

        if (existingPlan == null) return;

        if (CurrentDay.RecordedPlans.Count > 1)
        CurrentDay.RecordedPlans.Remove(existingPlan);
        else
        Items.Remove(CurrentDay);

        UpdateCurrentDay();
        await _status.SaveAsync();
    }
    
    private async Task ToMainPage()
    {
        await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
    }

    private async Task ToPlansPage()
    {
        await Shell.Current.GoToAsync($"///{nameof(ExercisePlansPage)}");
    }

    private async Task ToGuidePage()
    {
        await Shell.Current.GoToAsync($"///{nameof(ExerciseGuidePage)}");
    }
}
