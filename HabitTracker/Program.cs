using HabitTracker.Services;

namespace HabitTracker;

class Program
{
    static void Main(string[] args)
    {
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
            Console.WriteLine("│  6 - Delete habit                                          │");
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

                    habitService.CreateHabit(name ?? "", description ?? "");
                    break;

                case "2":
                    habitService.ListHabits();
                    break;

                case "3":
                    Console.Write("\nEnter habit ID to complete: ");
                    if (int.TryParse(Console.ReadLine(), out int completeId))
                    {
                        habitService.CompleteHabit(completeId);
                    }
                    else
                    {
                        Console.WriteLine("\n✗ Invalid ID!");
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
                    habitService.ShowStatistics();
                    break;

                case "6":
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