using System.Diagnostics;
using System.Text.Json;
using SchoolStressManagementApp.Models;

namespace SchoolStressManagementApp.Services;

public class GlobalStressManagementStatus
{
    private const string FileName = "appdata.json";
    private readonly string _filePath;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public StressManagementStatusData Data { get; private set; } = new();

    public GlobalStressManagementStatus()
    {
        _filePath = Path.Combine(FileSystem.AppDataDirectory, FileName);
    }

    public async Task LoadAsync()
    {
        await _semaphore.WaitAsync();

        try
        {
            if (!File.Exists(_filePath))
                return;

            string json = await File.ReadAllTextAsync(_filePath);

            var loaded = JsonSerializer.Deserialize<StressManagementStatusData>(json);

            if (loaded == null)
                return;

            Data.HydrationDays.Clear();
            foreach (var item in loaded.HydrationDays)
                Data.HydrationDays.Add(item);

            Data.SleepDays.Clear();
            foreach (var item in loaded.SleepDays)
                Data.SleepDays.Add(item);

            Data.ExerciseDays.Clear();
            foreach (var item in loaded.ExerciseDays)
                Data.ExerciseDays.Add(item);

            Data.ExercisePlans.Clear();
            foreach (var item in loaded.ExercisePlans)
                Data.ExercisePlans.Add(item);
        }
        catch (Exception ex) 
        {
            // Catch-all for other issues (e.g., null strings, IO errors)
            Debug.WriteLine($"General Error: {ex.Message}");
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task SaveAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            string json = JsonSerializer.Serialize(Data);
            await File.WriteAllTextAsync(_filePath, json);
        }
        catch (Exception ex) 
        {
            // Catch-all for other issues (e.g., null strings, IO errors)
            Debug.WriteLine($"Save Error: {ex.Message}");
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
