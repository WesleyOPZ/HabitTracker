using System.Text.Json;
using HabitTracker.Core.Models;

namespace HabitTracker.Core.Data;

public class HabitStorageData
{
    public List<HabitFolder> Folders { get; set; } = new();
    public List<Habit> Habits { get; set; } = new();
}

public class JsonStorage
{
    private readonly string _habitsFilePath;
    private readonly string _profileFilePath;


    public JsonStorage()
    {
        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "HabitTracker");
        Directory.CreateDirectory(folder);
        _habitsFilePath = Path.Combine(folder, "habits.json");
        _profileFilePath = Path.Combine(folder, "profile.json");
    }

    public HabitStorageData LoadHabits()
    {
        if (!File.Exists(_habitsFilePath))
        {
            return new HabitStorageData();
        }

        try
        {
            string json = File.ReadAllText(_habitsFilePath);

            var data = JsonSerializer.Deserialize<HabitStorageData>(json);
            if (data != null && (data.Folders.Count > 0 || data.Habits.Count > 0))
            {
                return data;
            }

            var legacyHabits = JsonSerializer.Deserialize<List<Habit>>(json);
            if (legacyHabits != null)
            {
                return new HabitStorageData
                {
                    Habits = legacyHabits
                };
            }

        }
        catch
        {
            return new HabitStorageData();
        }
        
        return new HabitStorageData();
    }


    public void SaveHabits(HabitStorageData data)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(_habitsFilePath, json);
    }

    public UserProfile LoadUserProfile()
    {
        if (!File.Exists(_profileFilePath))
        {
            return new UserProfile() { DateCreated = DateTime.Now };
        }

        try
        {
            string json = File.ReadAllText(_profileFilePath);
            return JsonSerializer.Deserialize<UserProfile>(json) ?? new UserProfile();
        }
        catch
        {
            return new UserProfile() { DateCreated = DateTime.Now };
        }
    }

    public void SaveProfile(UserProfile profile)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(profile, options);
        File.WriteAllText(_profileFilePath, json);
    }
}