using HabitTracker.Core.Models;

namespace HabitTracker.Core.Services;

public class StatisticsService
{
    private readonly List<Habit> _habits;

    public StatisticsService(List<Habit> habits)
    {
        _habits = habits;
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

    public void ShowStatistics()
    {
        if (HasNoHabits()) return;

        int totalHabits = _habits.Count;
        int completedToday = _habits.Count(h => h.IsCompletedToday());
        int totalXP = _habits.Sum(h => h.TotalXp);
        int currentLevel = LevelSystem.CalculateLevel(totalXP);
        string levelName = LevelSystem.GetLevelName(currentLevel);
        int xpForNext = LevelSystem.GetXpForNextLevel(currentLevel);
        int xpProgress = LevelSystem.GetXpProgressInCurrentLevel(totalXP, currentLevel);
        int totalCompletions = _habits.Sum(h => h.CompletedDates.Count);
        var bestStreak = _habits.MaxBy(h => h.CurrentStreak);

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                      STATISTICS                            ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
        Console.WriteLine($"\nTotal habits:           {totalHabits}");
        Console.WriteLine($"Completed today:        {completedToday}/{totalHabits}");
        Console.WriteLine($"Total XP:               {totalXP}");
        Console.WriteLine($"Current Level:          {currentLevel} - {levelName}");
        if (xpForNext > 0)
        {
            Console.WriteLine($"XP to next level:       {xpProgress}/{xpForNext}");
        }
        else
        {
            Console.WriteLine($"MAX LEVEL REACHED!");
        }

        Console.WriteLine($"Total completions:      {totalCompletions}");

        if (bestStreak != null)
        {
            Console.WriteLine($"Best current streak:    {bestStreak.Name} ({bestStreak.CurrentStreak} days)");
        }

        Console.WriteLine("\n════════════════════════════════════════════════════════════\n");
    }

    public void ShowCategoryStatistics()
    {
        if (HasNoHabits()) return;

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║              STATISTICS BY CATEGORY                        ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

        var categories = new[] { Category.Health, Category.Study, Category.Work, Category.Personal };

        foreach (var category in categories)
        {
            var habitsInCategory = _habits.Where(h => h.Category == category).ToList();

            if (!habitsInCategory.Any())
                continue; // Pula categoria vazia

            int totalHabits = habitsInCategory.Count();
            int totalXp = habitsInCategory.Sum(h => h.TotalXp);
            int totalCompletions = habitsInCategory.Sum(h => h.CompletedDates.Count);
            int completedToday = habitsInCategory.Count(h => h.IsCompletedToday());

            double completionRate = totalHabits > 0
                ? (completedToday * 100.0 / totalHabits)
                : 0;

            Console.WriteLine($"Category: {category} | Habits: {totalHabits} | XP: {totalXp} | Completion rate today: {completionRate:F1}%");
            Console.WriteLine();
        }
    }

    public void ShowWeekdayStatistics()
    {
        if (HasNoHabits()) return;

        // Pega TODAS as datas de completion
        var allCompletions = _habits
            .SelectMany(h => h.CompletedDates)
            .ToList();

        if (!allCompletions.Any())
        {
            Console.WriteLine("\nNo completions yet to analyze.");
            return;
        }

        // Agrupa por dia da semana e conta
        var completionsByWeekday = allCompletions
            .GroupBy(d => d.DayOfWeek)
            .Select(g => new
            {
                DayOfWeek = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .ToList();

        var bestDay = completionsByWeekday.First();
        var worstDay = completionsByWeekday.Last();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║              WEEKDAY STATISTICS                            ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

        Console.WriteLine($"Best day:    {bestDay.DayOfWeek} ({bestDay.Count} completions)");
        Console.WriteLine($"Worst day:   {worstDay.DayOfWeek} ({worstDay.Count} completions)");

        Console.WriteLine("\nAll days:");
        foreach (var day in completionsByWeekday)
        {
            Console.WriteLine($"  {day.DayOfWeek,-10} {day.Count} completions");
        }
    }

    public void ShowWeekActivityGraph()
    {
        if (HasNoHabits()) return;

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║              LAST 7 DAYS ACTIVITY                          ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

        // Últimos 7 dias (hoje até 6 dias atrás)
        for (int i = 6; i >= 0; i--)
        {
            DateTime day = DateTime.Today.AddDays(-i);

            // Conta quantas completions teve nesse dia
            int completionsOnDay = _habits
                .SelectMany(h => h.CompletedDates)
                .Count(d => d.Date == day.Date);

            // Cria barra do gráfico (cada █ = 1 completion)
            string bar = new string('█', completionsOnDay);
            string emptyBar = new string('░', Math.Max(0, 10 - completionsOnDay));

            string dayName = day.ToString("ddd");

            Console.WriteLine($"{dayName} {bar}{emptyBar} {completionsOnDay}");
        }

        Console.WriteLine();
    }
}