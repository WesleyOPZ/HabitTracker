using HabitTracker.Core.Models;

namespace HabitTracker.Core.Services;

public class AchievementService
{
    private List<Achievement> _achievements;
    private readonly List<Habit> _habits;
    private readonly UserProfile _profile;

    public AchievementService(List<Habit> habits, UserProfile profile)
    {
        _habits = habits;
        _profile = profile;
        _achievements = InitializeAchievements();
        RecheckAllAchievements();
    }

    private List<Achievement> InitializeAchievements()
    {
        return new List<Achievement>
        {
            // Iniciante
            new Achievement
            {
                Type = AchievementType.FirstStep,
                Name = "First Step",
                Description = "Create your first habit",
                Icon = "🌱",
                IsUnlocked = false,
            },
            new Achievement
            {
                Type = AchievementType.Beginner,
                Name = "Beginner",
                Description = "Complete a habit for the first time",
                Icon = "⭐",
                IsUnlocked = false,
            },
            new Achievement
            {
                Type = AchievementType.GettingStarted,
                Name = "Getting Started",
                Description = "Complete 5 habits total",
                Icon = "🔥",
                IsUnlocked = false
            },

            // Consistência
            new Achievement
            {
                Type = AchievementType.Dedicated,
                Name = "Dedicated",
                Description = "Maintain a 7-day streak",
                Icon = "📅",
                IsUnlocked = false
            },
            new Achievement
            {
                Type = AchievementType.Committed,
                Name = "Committed",
                Description = "Maintain a 30-day streak",
                Icon = "🎯",
                IsUnlocked = false
            },
            new Achievement
            {
                Type = AchievementType.Unstoppable,
                Name = "Unstoppable",
                Description = "Maintain a 100-day streak",
                Icon = "💪",
                IsUnlocked = false
            },

            // Exploração
            new Achievement
            {
                Type = AchievementType.Diverse,
                Name = "Diverse",
                Description = "Have at least 1 habit in each category",
                Icon = "🌈",
                IsUnlocked = false
            },
            new Achievement
            {
                Type = AchievementType.CategoryMaster,
                Name = "Category Master",
                Description = "Complete 10 habits in a single category",
                Icon = "📚",
                IsUnlocked = false
            },

            // Níveis
            new Achievement
            {
                Type = AchievementType.Level3,
                Name = "Rising Star",
                Description = "Reach level 3",
                Icon = "🎖️",
                IsUnlocked = false
            },
            new Achievement
            {
                Type = AchievementType.Level5,
                Name = "Expert",
                Description = "Reach level 5",
                Icon = "🏆",
                IsUnlocked = false
            },
            new Achievement
            {
                Type = AchievementType.Level7,
                Name = "Legend",
                Description = "Reach level 7",
                Icon = "👑",
                IsUnlocked = false
            },

            // XP
            new Achievement
            {
                Type = AchievementType.Century,
                Name = "Century",
                Description = "Earn 100 total XP",
                Icon = "⚡",
                IsUnlocked = false
            },
            new Achievement
            {
                Type = AchievementType.HalfK,
                Name = "Half K",
                Description = "Earn 500 total XP",
                Icon = "💎",
                IsUnlocked = false
            },
            new Achievement
            {
                Type = AchievementType.Millennium,
                Name = "Millennium",
                Description = "Earn 1000 total XP",
                Icon = "🔱",
                IsUnlocked = false
            }
        };
    }
    // Checa TODAS as conquistas e desbloqueia as que foram atingidas

    public List<Achievement> CheckAchievements()
    {
        var newlyUnlocked = new List<Achievement>();

        // Checa cada conquista
        foreach (var achievement in _achievements.Where(a => !a.IsUnlocked))
        {
            if (ShouldUnlockAchievements(achievement))
            {
                achievement.IsUnlocked = true;
                achievement.UnlockedAt = DateTime.Now;
                newlyUnlocked.Add(achievement);
            }
        }

        return newlyUnlocked;
    }

    private bool CheckDiverseAchievement()
    {
        var categories = new[] { Category.Health, Category.Study, Category.Work, Category.Personal };
        return categories.All(cat => _habits.Any(h => h.Category == cat));
    }

    private bool CheckCategoryMasterAchievement()
    {
        var categories = new[] { Category.Health, Category.Study, Category.Work, Category.Personal };
        foreach (var category in categories)
        {
            int completionsInCategory = _habits
                .Where(h => h.Category == category)
                .Sum(h => h.CompletedDates.Count);

            if (completionsInCategory >= 10)
                return true;
        }

        return false;
    }

    private void RecheckAllAchievements()
    {
        foreach (var achievement in _achievements)
        {
            if (ShouldUnlockAchievements(achievement) && !achievement.IsUnlocked)
            {
                achievement.IsUnlocked = true;
                achievement.UnlockedAt = DateTime.Now;
            }
        }
    }

    private bool ShouldUnlockAchievements(Achievement achievement)
    {
        return achievement.Type switch
        {
            AchievementType.FirstStep => _habits.Count >= 1,
            AchievementType.Beginner => _habits.Sum(h => h.CompletedDates.Count) >= 1,
            AchievementType.GettingStarted => _habits.Sum(h => h.CompletedDates.Count) >= 5,
            AchievementType.Dedicated => _habits.Any(h => h.LongestStreak >= 7),
            AchievementType.Committed => _habits.Any(h => h.LongestStreak >= 30),
            AchievementType.Unstoppable => _habits.Any(h => h.LongestStreak >= 100),
            AchievementType.Diverse => CheckDiverseAchievement(),
            AchievementType.CategoryMaster => CheckCategoryMasterAchievement(),
            AchievementType.Level3 => LevelSystem.CalculateLevel(_profile.TotalXp) >= 3,
            AchievementType.Level5 => LevelSystem.CalculateLevel(_profile.TotalXp) >= 5,
            AchievementType.Level7 => LevelSystem.CalculateLevel(_profile.TotalXp) >= 7,
            AchievementType.Century => _profile.TotalXp >= 100,
            AchievementType.HalfK => _profile.TotalXp >= 500,
            AchievementType.Millennium => _profile.TotalXp >= 1000,
            _ => false
        };
    }
}