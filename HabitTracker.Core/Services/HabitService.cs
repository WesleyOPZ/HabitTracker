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


    public CompleteHabitResult CompleteHabit(int id)
    {
        var habit = _habits.FirstOrDefault(h => h.Id == id);

        if (habit == null)
            return new CompleteHabitResult { Success = false, Message = "Habit not Found." };

        if (habit.IsCompletedToday())
            return new CompleteHabitResult
                { AlreadyCompleted = true, Message = $"You already completed '{habit.Name}' today!" };

        habit.CompletedDates.Add(DateTime.Now);

        var yesterday = DateTime.Today.AddDays(-1);
        if (habit.CompletedDates.Any(d => d.Date == yesterday))
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

    public DeleteHabitResult DeleteHabit(int id)
    {
        var habit = _habits.FirstOrDefault(h => h.Id == id);

        if (habit == null)
            return new DeleteHabitResult { Success = false, Message = "Habit not Found." };
        

        _habits.Remove(habit);
        _storage.SaveHabits(_habits);
        return new DeleteHabitResult {  Success = true, Message = $"Habit '{habit.Name}' Deleted." };
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