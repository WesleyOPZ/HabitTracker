using HabitTracker.Core.Models;
using HabitTracker.Core.Services;

namespace HabitTracker;

class Program
{
    static Category AskForCategory()
    {
        Console.WriteLine("\nChoose category:");
        Console.WriteLine("1 - Health");
        Console.WriteLine("2 - Study");
        Console.WriteLine("3 - Work");
        Console.WriteLine("4 - Personal");
        Console.Write("Choose: ");

        string? input = Console.ReadLine();

        return input switch
        {
            "1" => Category.Health,
            "2" => Category.Study,
            "3" => Category.Work,
            "4" => Category.Personal,
            _ => Category.Personal
        };
    }

    static Difficulty AskForDifficulty()
    {
        Console.WriteLine("\nChoose difficulty:");
        Console.WriteLine("1 - Easy (5 XP)");
        Console.WriteLine("2 - Normal (10 XP)");
        Console.WriteLine("3 - Hard (20 XP)");
        Console.WriteLine("4 - Legendary (50 XP)");
        Console.Write("Choose: ");

        string? input = Console.ReadLine();

        return input switch
        {
            "1" => Difficulty.Easy,
            "2" => Difficulty.Normal,
            "3" => Difficulty.Hard,
            "4" => Difficulty.Legendary,
            _ => Difficulty.Normal
        };
    }

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var habitService = new HabitService();

        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║            🎯 HABIT TRACKER - Build Better Habits           ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        while (true)
        {
            Console.WriteLine("\n┌────────────────────────────────────────────────────────────┐");
            Console.WriteLine("│  1 - Create new habit                                      │");
            Console.WriteLine("│  2 - List all habits                                       │");
            Console.WriteLine("│  3 - Complete habit today                                  │");
            Console.WriteLine("│  4 - View habit history                                    │");
            Console.WriteLine("│  5 - Show statistics                                       │");
            Console.WriteLine("│  6 - Filter by Category                                    │");
            Console.WriteLine("│  7 - Category statistics                                   │");
            Console.WriteLine("│  8 - Weekday statistics                                    │");
            Console.WriteLine("│  9 - Week activity graph                                   │");
            Console.WriteLine("│  10 - Achievements                                         │");
            Console.WriteLine("│  99 - Delete habit                                         │");
            Console.WriteLine("│  0 - Exit                                                  │");
            Console.WriteLine("└────────────────────────────────────────────────────────────┘");
            Console.Write("\nChoose an option: ");

            string? option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Console.Write("\nHabit name: ");
                    string? name = Console.ReadLine();

                    Console.Write("Description (optional): ");
                    string? description = Console.ReadLine();

                    Difficulty difficulty = AskForDifficulty();
                    Category category = AskForCategory();

                    habitService.CreateHabit(name ?? "", description ?? "", difficulty, category);
                    break;

                case "2":
                    habitService.ListHabits();
                    break;

                case "3":
                    Console.Write("\nEnter habit ID to complete: ");
                    if (int.TryParse(Console.ReadLine(), out int completeId))
                    {
                        var result = habitService.CompleteHabit(completeId);
                        if (result.AlreadyCompleted)
                            Console.WriteLine($"\n✗ {result.Message}");
                        else if (result.Success)
                        {
                            Console.WriteLine($"\n✓ Great job! +{result.XpEarned} XP");
                            if (result.LeveledUp)
                                Console.WriteLine(
                                    $"🎉 LEVEL UP! Level {result.NewLevel} - {LevelSystem.GetLevelName(result.NewLevel)}!");
                            foreach (var a in result.NewAchievements)
                                Console.WriteLine($"🏆 Achievement unlocked: {a.Icon} {a.Name}!");
                        }
                    }

                    break;

                case "4":
                    Console.Write("\nEnter habit ID to view history: ");
                    if (int.TryParse(Console.ReadLine(), out int historyId))
                    {
                        habitService.ShowHabitHistory(historyId);
                    }
                    else
                    {
                        Console.WriteLine("\n✗ Invalid ID!");
                    }

                    break;

                case "5":
                    habitService.Statistics.ShowStatistics();
                    break;

                case "6":
                    Category filterCategory = AskForCategory();
                    habitService.ListHabitCategory(filterCategory);
                    break;

                case "7":
                    habitService.Statistics.ShowCategoryStatistics();
                    break;

                case "8":
                    habitService.Statistics.ShowWeekdayStatistics();
                    break;

                case "9":
                    habitService.Statistics.ShowWeekActivityGraph();
                    break;

                case "10":
                    habitService.Achievements.ShowAchievements();
                    break;

                case "99":
                    Console.Write("\nEnter habit ID to delete: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                    {
                        habitService.DeleteHabit(deleteId);
                    }
                    else
                    {
                        Console.WriteLine("\n✗ Invalid ID!");
                    }

                    break;

                case "0":
                    Console.WriteLine("\n👋 Keep building those habits! Goodbye!");
                    return;

                default:
                    Console.WriteLine("\n✗ Invalid option!");
                    break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║            🎯 HABIT TRACKER - Build Better Habits           ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
        }
    }
}