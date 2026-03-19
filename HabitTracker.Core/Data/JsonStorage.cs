using System.Text.Json;
using HabitTracker.Core.Models;

namespace HabitTracker.Core.Data;

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

    public List<Habit> LoadHabits()
    {
        if (!File.Exists(_habitsFilePath))
        {
            return new List<Habit>();
        }

        try
        {
            string json = File.ReadAllText(_habitsFilePath);
            return JsonSerializer.Deserialize<List<Habit>>(json) ?? new List<Habit>();
        }
        catch
        {
            return new List<Habit>();
        }
    }

    public void SaveHabits(List<Habit> habits)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(habits, options);
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