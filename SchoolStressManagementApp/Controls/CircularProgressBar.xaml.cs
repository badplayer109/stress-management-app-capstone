using System.Diagnostics;

namespace SchoolStressManagementApp.Controls;

public partial class CircularProgressBar : ContentView
{
	public static readonly BindableProperty ProgressProperty = BindableProperty.Create(nameof(Progress), typeof(int), typeof(CircularProgressBar));
    public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(int), typeof(CircularProgressBar));
    public static readonly BindableProperty ThicknessProperty = BindableProperty.Create(nameof(Thickness), typeof(int), typeof(CircularProgressBar));
    public static readonly BindableProperty ProgressColorProperty = BindableProperty.Create(nameof(ProgressColor), typeof(Color), typeof(CircularProgressBar));
    public static readonly BindableProperty ProgressLeftColorProperty = BindableProperty.Create(nameof(ProgressLeftColor), typeof(Color), typeof(CircularProgressBar));
    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CircularProgressBar));
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(CircularProgressBar));
    public static readonly BindableProperty MinProperty = BindableProperty.Create(nameof(Min), typeof(double), typeof(CircularProgressBar));
    public static readonly BindableProperty MaxProperty = BindableProperty.Create(nameof(Max), typeof(double), typeof(CircularProgressBar));

    public int Progress
    {
        get { return (int)GetValue(ProgressProperty); }
        set { SetValue(ProgressProperty, value); }
    }

    public int Size
    {
        get { return (int)GetValue(SizeProperty); }
        set { SetValue(SizeProperty, value); }
    }

    public int Thickness
    {
        get { return (int)GetValue(ThicknessProperty); }
        set { SetValue(ThicknessProperty, value); }
    }

    public Color ProgressColor
    {
        get { return (Color)GetValue(ProgressColorProperty); }
        set { SetValue(ProgressColorProperty, value); }
    }
    
    public Color ProgressLeftColor
    {
        get { return (Color)GetValue(ProgressLeftColorProperty); }
        set { SetValue(ProgressLeftColorProperty, value); }
    }

    public Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { SetValue(TextColorProperty, value); }
    }

    public double Value
    {
        get { return (double)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public double Min
    {
        get { return (double)GetValue(MinProperty); }
        set { SetValue(MinProperty, value); }
    }

    public double Max
    {
        get { return (double)GetValue(MaxProperty); }
        set { SetValue(MaxProperty, value); }
    }

    private bool _isUpdating;
	
	public CircularProgressBar()
	{
		InitializeComponent();

        Max = 100;
        Min = 0;
        Value = 0;
	}

    private void CalculatePercentage()
    {
        if (_isUpdating) return;

        _isUpdating = true;

        double interval = Max - Min;
        if (interval == 0)
            Progress = 0;
        else
            Progress = (int)Math.Round((Value - Min) * 100 / interval);

        _isUpdating = false;
    }

    private void CalculateValue()
    {
        if (_isUpdating) return;

        _isUpdating = true;

        Value = Min + (Progress * (Max - Min) / 100);

        _isUpdating = false;
    }

    protected override void OnPropertyChanged(string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == ProgressProperty.PropertyName)
        {
            CalculateValue();
            graphicsView?.Invalidate();
        }
        else if (propertyName == ValueProperty.PropertyName)
        {
            CalculatePercentage();
            graphicsView?.Invalidate();
        }

		else if (propertyName == SizeProperty.PropertyName)
		{
			HeightRequest = Size;
			WidthRequest = Size;
            graphicsView?.Invalidate();
		}

        else if (propertyName == ThicknessProperty.PropertyName ||
                propertyName == ProgressColorProperty.PropertyName ||
                propertyName == ProgressLeftColorProperty.PropertyName ||
                propertyName == TextColorProperty.PropertyName)
        {
            graphicsView?.Invalidate();
        }
    }
}