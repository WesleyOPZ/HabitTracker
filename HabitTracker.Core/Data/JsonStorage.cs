using System.Text.Json;
using HabitTracker.Core.Models;

namespace HabitTracker.Core.Data;

public class JsonStorage
{
    private readonly string _filePath;

    public JsonStorage()
    {
        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HabitTracker");
        Directory.CreateDirectory(folder);
        _filePath = Path.Combine(folder, "habits.json");
    }

    public List<Habit> LoadHabits()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Habit>();
        }

        try
        {
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Habit>>(json) ?? new List<Habit>();
        }
        catch
        {
            Console.WriteLine("⚠️  Error loading habits. Starting fresh.");
            return new List<Habit>();
        }
    }

    public void SaveHabits(List<Habit> habits)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(habits, options);
        File.WriteAllText(_filePath, json);
    }
}