using HabitTracker.Core.Models;
using HabitTracker.Core.Data;

namespace HabitTracker.Core.Services;

public class HabitService
{
    private List<Habit> _habits;
    private readonly JsonStorage _storage;
    private int _nextId;

    public StatisticsService Statistics { get; private set; }
    public AchievementService Achievements { get; private set; }

    public HabitService()
    {
        _storage = new JsonStorage();
        _habits = _storage.LoadHabits();
        _nextId = _habits.Any() ? _habits.Max(h => h.Id) + 1 : 1;

        Statistics = new StatisticsService(_habits);
        Achievements = new AchievementService(_habits);
    }

    private void DisplayHabit(Habit habit)
    {
        string status = habit.IsCompletedToday() ? "✅" : "⭕";
        string streak = habit.CurrentStreak > 0
            ? $"{habit.CurrentStreak} days"
            : "No streak yet";
        Console.WriteLine($"{status} [{habit.Id}] {habit.Name}");
        Console.WriteLine(
            $"   Category: {habit.Category} | Difficulty: {habit.Difficulty} | " +
            $"Streak: {streak} | Best: {habit.LongestStreak} days | XP: {habit.TotalXp}");

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

    private bool HasNoHabits()
    {
        if (!_habits.Any())
        {
            Console.WriteLine("\n📭 No habits to show statistics for.");
            return true;
        }

        return false;
    }

    public void CreateHabit(string name, string description = "", Difficulty difficulty = Difficulty.Normal,
        Category category = Category.Personal)
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
            TotalXp = 0,
            Difficulty = difficulty,
            Category = category,
            CompletedDates = new List<DateTime>()
        };

        _habits.Add(habit);
        _storage.SaveHabits(_habits);

        Console.WriteLine($"\n✓ Habit '{name}' created successfully! (ID: {habit.Id})");

        var newAchievements = Achievements.CheckAchievements();
        if (newAchievements.Any())
        {
            Console.WriteLine(
                $"\n🏆 Achievement unlocked: {newAchievements.First().Icon} {newAchievements.First().Name}!");
        }
    }

    public void ListHabits()
    {
        if (HasNoHabits()) return;

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                      YOUR HABITS                           ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

        foreach (var habit in _habits.OrderByDescending(h => h.CurrentStreak))
        {
            DisplayHabit(habit);
        }
    }

    public CompleteHabitResult CompleteHabit (int id)
    {
        var habit = _habits.FirstOrDefault(h => h.Id == id);

        if (habit == null)
            return new CompleteHabitResult { Success = false, Message = "Habit not Found." };
        
        if (habit.IsCompletedToday())
            return new CompleteHabitResult{AlreadyCompleted = true, Message = $"You already completed '{habit.Name}' today!" };
        
        habit.CompletedDates.Add(DateTime.Now);
        
        var yesterday = DateTime.Now.AddDays(-1);
        if (habit.CompletedDates.Any(d => d.Date == DateTime.Today.AddDays(-1)))
            habit.CurrentStreak++;
        else 
            habit.CurrentStreak = 1;

        if (habit.CurrentStreak > habit.LongestStreak)
            habit.LongestStreak = habit.CurrentStreak;

        int xpEarned = (int)habit.Difficulty + (habit.CurrentStreak - 1);
        habit.TotalXp += xpEarned;
        
        _storage.SaveHabits(_habits);
        
        int totalXp = _habits.Sum(h => h.TotalXp);
        int oldLevel = LevelSystem.CalculateLevel(totalXp - xpEarned);
        int newLevel = LevelSystem.CalculateLevel(totalXp);
        
        var newAchievements = Achievements.CheckAchievements();

        return new CompleteHabitResult
        {
            Success = true,
            XpEarned = xpEarned,
            LeveledUp = newLevel > oldLevel,
            NewLevel = newLevel,
            NewAchievements = newAchievements
        };


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

    public void ListHabitCategory(Category category)
    {
        var filtered = _habits.Where(h => h.Category == category).ToList();
        if (!filtered.Any())
        {
            Console.WriteLine($"\n📭 No habits in category {category}.");
            return;
        }

        Console.WriteLine($"\n╔════════════════════════════════════════════════╗");
        Console.WriteLine($"║  HABITS - {category.ToString().ToUpper().PadRight(38)} ║");
        Console.WriteLine($"╚════════════════════════════════════════════════╝\n");

        foreach (var habit in filtered)
        {
            DisplayHabit(habit);
        }
    }

    public List<Habit> GetHabits()
    {
        return _habits.OrderByDescending(h => h.CurrentStreak).ToList();
    }

    public int GetTotalXp()
    {
        return _habits.Sum(h => h.TotalXp);
    }
}