using HabitTracker.Models;
using HabitTracker.Data;

namespace HabitTracker.Services;

public class HabitService
{
    private List<Habit> _habits;
    private readonly JsonStorage _storage;
    private int _nextId;

    public HabitService()
    {
        _storage = new JsonStorage();
        _habits = _storage.LoadHabits();
        _nextId = _habits.Any() ? _habits.Max(h => h.Id) + 1 : 1;
    }

    public void CreateHabit(string name, string description = "")
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("✗ Habit name cannot be empty!");
            return;
        }

        var habit = new Habit
        {
            Id = _nextId++,
            Name = name,
            Description = description,
            CreatedAt = DateTime.Now,
            CurrentStreak = 0,
            LongestStreak = 0,
            TotalXP = 0,
            CompletedDates = new List<DateTime>()
        };

        _habits.Add(habit);
        _storage.SaveHabits(_habits);

        Console.WriteLine($"\n✓ Habit '{name}' created successfully! (ID: {habit.Id})");
    }

    public void ListHabits()
    {
        if (!_habits.Any())
        {
            Console.WriteLine("\n📭 No habits yet. Create your first one!");
            return;
        }

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                      YOUR HABITS                           ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

        foreach (var habit in _habits.OrderByDescending(h => h.CurrentStreak))
        {
            string status = habit.IsCompletedToday() ? "✅" : "⭕";
            string streak = habit.CurrentStreak > 0 
                ? $"🔥 {habit.CurrentStreak} days" 
                : "No streak yet";

            Console.WriteLine($"{status} [{habit.Id}] {habit.Name}");
            Console.WriteLine($"   Streak: {streak} | Best: {habit.LongestStreak} days | XP: {habit.TotalXP}");
            
            if (!string.IsNullOrEmpty(habit.Description))
            {
                Console.WriteLine($"   Description: {habit.Description}");
            }

            var lastCompleted = habit.LastCompletedDate();
            if (lastCompleted.HasValue)
            {
                Console.WriteLine($"   Last completed: {lastCompleted.Value:dd/MM/yyyy}");
            }

            Console.WriteLine();
        }
    }

    public void CompleteHabit(int id)
    {
        var habit = _habits.FirstOrDefault(h => h.Id == id);

        if (habit == null)
        {
            Console.WriteLine($"\n✗ Habit with ID {id} not found.");
            return;
        }

        if (habit.IsCompletedToday())
        {
            Console.WriteLine($"\n✗ You already completed '{habit.Name}' today!");
            return;
        }

        // Add today's completion
        habit.CompletedDates.Add(DateTime.Now);

        // Calculate streak
        var yesterday = DateTime.Today.AddDays(-1);
        if (habit.CompletedDates.Any(d => d.Date == yesterday))
        {
            habit.CurrentStreak++;
        }
        else
        {
            habit.CurrentStreak = 1;
        }

        // Update longest streak
        if (habit.CurrentStreak > habit.LongestStreak)
        {
            habit.LongestStreak = habit.CurrentStreak;
        }

        // Award XP (base 10 + streak bonus)
        int xpEarned = 10 + (habit.CurrentStreak - 1);
        habit.TotalXP += xpEarned;

        _storage.SaveHabits(_habits);

        Console.WriteLine($"\n✨ Great job! You completed '{habit.Name}'!");
        Console.WriteLine($"🔥 Current streak: {habit.CurrentStreak} days");
        Console.WriteLine($"⭐ XP earned: +{xpEarned} (Total: {habit.TotalXP})");
    }

    public void DeleteHabit(int id)
    {
        var habit = _habits.FirstOrDefault(h => h.Id == id);

        if (habit == null)
        {
            Console.WriteLine($"\n✗ Habit with ID {id} not found.");
            return;
        }

        _habits.Remove(habit);
        _storage.SaveHabits(_habits);

        Console.WriteLine($"\n✓ Habit '{habit.Name}' deleted successfully!");
    }

    public void ShowStatistics()
    {
        if (!_habits.Any())
        {
            Console.WriteLine("\n📭 No habits to show statistics for.");
            return;
        }

        int totalHabits = _habits.Count;
        int completedToday = _habits.Count(h => h.IsCompletedToday());
        int totalXP = _habits.Sum(h => h.TotalXP);
        int totalCompletions = _habits.Sum(h => h.CompletedDates.Count);
        var bestStreak = _habits.MaxBy(h => h.CurrentStreak);

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                      STATISTICS                            ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
        Console.WriteLine($"\n📊 Total habits:           {totalHabits}");
        Console.WriteLine($"✅ Completed today:        {completedToday}/{totalHabits}");
        Console.WriteLine($"⭐ Total XP:               {totalXP}");
        Console.WriteLine($"🎯 Total completions:      {totalCompletions}");
        
        if (bestStreak != null)
        {
            Console.WriteLine($"🔥 Best current streak:    {bestStreak.Name} ({bestStreak.CurrentStreak} days)");
        }

        Console.WriteLine("\n════════════════════════════════════════════════════════════\n");
    }

    public void ShowHabitHistory(int id)
    {
        var habit = _habits.FirstOrDefault(h => h.Id == id);

        if (habit == null)
        {
            Console.WriteLine($"\n✗ Habit with ID {id} not found.");
            return;
        }

        Console.WriteLine($"\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine($"║  HISTORY: {habit.Name.PadRight(48)} ║");
        Console.WriteLine($"╚════════════════════════════════════════════════════════════╝\n");

        if (!habit.CompletedDates.Any())
        {
            Console.WriteLine("No completions yet. Start today!\n");
            return;
        }

        // Show last 10 completions
        var recentCompletions = habit.CompletedDates
            .OrderByDescending(d => d)
            .Take(10)
            .ToList();

        Console.WriteLine("Recent completions:");
        foreach (var date in recentCompletions)
        {
            Console.WriteLine($"  ✓ {date:dddd, dd/MM/yyyy HH:mm}");
        }

        Console.WriteLine($"\nTotal completions: {habit.CompletedDates.Count}");
        Console.WriteLine();
    }
}