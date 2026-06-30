using HabitTracker.Core.Models;
using HabitTracker.Core.Data;

namespace HabitTracker.Core.Services;

public class HabitService {
    private List<Habit> _habits;
    private List<HabitFolder> _folders;
    private readonly JsonStorage _storage;
    private int _nextId;
    private int _nextFolderId;
    private UserProfile _profile;

    public StatisticsService Statistics { get; private set; }
    public AchievementService Achievements { get; private set; }
    public UserProfile GetProfile() => _profile;

    
    public HabitService() : this(new JsonStorage()) { }

    public HabitService(JsonStorage storage) {
        _storage = storage;
        var storageData = _storage.LoadHabits();
        _habits = storageData.Habits;
        _folders = storageData.Folders;

        if (!_folders.Any()) {
            _folders.Add(new HabitFolder
                { Id = (int)FolderType.ToDo, Name = "To Do", DisplayOrder = 0, CreatedAt = DateTime.UtcNow });
            _folders.Add(new HabitFolder {
                Id = (int)FolderType.InProgress, Name = "In Progress", DisplayOrder = 1, CreatedAt = DateTime.UtcNow
            });
            _folders.Add(new HabitFolder
                { Id = (int)FolderType.Done, Name = "Done", DisplayOrder = 2, CreatedAt = DateTime.UtcNow });


            foreach (var habit in _habits) {
                if (habit.FolderId == 0) {
                    habit.FolderId = 1;
                }
            }

            SaveAllData();
        }

        _nextId = _habits.Any() ? _habits.Max(h => h.Id) + 1 : 1;
        _nextFolderId = _folders.Any() ? _folders.Max(f => f.Id) + 1 : 1;

        _profile = _storage.LoadUserProfile();

        Statistics = new StatisticsService(_habits, _profile);
        Achievements = new AchievementService(_habits, _profile);
    }

    private void SaveAllData() {
        _storage.SaveHabits(new HabitStorageData {
            Folders = _folders,
            Habits = _habits
        });
    }

    public List<HabitFolder> GetFolders() {
        return _folders.OrderBy(f => f.DisplayOrder).ToList();
    }

    public void CreateFolder(string name) {
        if (string.IsNullOrWhiteSpace(name)) return;

        var folder = new HabitFolder {
            Id = _nextFolderId++,
            Name = name,
            DisplayOrder = _folders.Any() ? _folders.Max(f => f.DisplayOrder) + 1 : 0,
            CreatedAt = DateTime.UtcNow
        };
        _folders.Add(folder);
        SaveAllData();
    }


    public CreateHabitResult CreateHabit(string name, string description = "",
        Difficulty difficulty = Difficulty.Normal,
        Category category = Category.Personal,
        int? folderId = null) {
        if (string.IsNullOrWhiteSpace(name)) {
            return new CreateHabitResult { Success = false, Message = "Habit name cannot be empty!" };
        }

        int targetFolderId = folderId ?? (_folders.FirstOrDefault()?.Id ?? (int)FolderType.ToDo);

        var habit = new Habit {
            Id = _nextId++,
            Name = name,
            Description = description,
            CreatedAt = DateTime.Now,
            CurrentStreak = 0,
            LongestStreak = 0,
            TotalXp = 0,
            Difficulty = difficulty,
            Category = category,
            CompletedDates = new List<DateTime>(),
            FolderId = targetFolderId
        };

        _habits.Add(habit);
        SaveAllData();

        var newAchievements = Achievements.CheckAchievements();

        return new CreateHabitResult {
            Success = true,
            Message = $"Habit '{name}' created successfully! (ID: {habit.Id}).",
            NewAchievements = newAchievements
        };
    }

    public MoveHabitResult MoveHabitToFolder(int habitId, int targetFolderId) {
        var habit = _habits.FirstOrDefault(h => h.Id == habitId);
        if (habit == null || habit.FolderId == targetFolderId) return new MoveHabitResult();

        int oldFolderId = habit.FolderId;
        int xpBefore = _profile.TotalXp;
        int levelBefore = LevelSystem.CalculateLevel(xpBefore);
        // --- TO-DO → IN-PROGRESS ---
        if (oldFolderId == (int)FolderType.ToDo && targetFolderId == (int)FolderType.InProgress) {
            habit.IsInProgress = true;
        }
        // --- IN-PROGRESS → TO-DO ---
        else if (oldFolderId == (int)FolderType.InProgress && targetFolderId == (int)FolderType.ToDo) {
            habit.IsInProgress = false;
        }
        // --- QUALQUER → DONE ---
        else if (targetFolderId == (int)FolderType.Done) {
            if (!habit.CompletedDates.Any(d => d.Date == DateTime.Today)) {
                habit.CompletedDates.Add(DateTime.Now);

                var yesterday = DateTime.Today.AddDays(-1);
                if (habit.CompletedDates.Any(d => d.Date == yesterday))
                    habit.CurrentStreak++;
                else
                    habit.CurrentStreak = 1;

                if (habit.CurrentStreak > habit.LongestStreak)
                    habit.LongestStreak = habit.CurrentStreak;

                if (habit.LongestStreak > _profile.GlobalLongestStreak)
                    _profile.GlobalLongestStreak = habit.LongestStreak;

                ApplyHabitCompletion(habit);
            }

            habit.IsInProgress = false;
        }

        // --- DONE → QUALQUER ---
        else if (oldFolderId == (int)FolderType.Done) {
            var today = habit.CompletedDates.FirstOrDefault(d => d.Date == DateTime.Today);
            if (today != default) {
                habit.CompletedDates.Remove(today);
                _profile.TotalXp -= habit.XpGainedToday;
                habit.TotalXp -= habit.XpGainedToday;
                habit.XpGainedToday = 0;
                habit.CurrentStreak = Math.Max(0, habit.CurrentStreak - 1);
            }

            habit.IsInProgress = targetFolderId == (int)FolderType.InProgress;
        }

        habit.FolderId = targetFolderId;
        SaveAllData();
        _storage.SaveProfile(_profile);

        int levelAfter = LevelSystem.CalculateLevel(_profile.TotalXp);
        var newAchievements = targetFolderId == (int)FolderType.Done
            ? Achievements.CheckAchievements()
            : new List<Achievement>();

        return new MoveHabitResult {
            LeveledUp = levelAfter > levelBefore,
            NewLevel = levelAfter,
            NewAchievements = newAchievements
        };
    }

    public bool ProcessDailyReset() {
        if (_profile.LastResetDate.Date >= DateTime.Today) return false;

        foreach (var habit in _habits) {
            if (habit.FolderId == (int)FolderType.Done) {
                if (habit.CurrentStreak > habit.LongestStreak)
                    habit.LongestStreak = habit.CurrentStreak;

                if (habit.LongestStreak > _profile.GlobalLongestStreak)
                    _profile.GlobalLongestStreak = habit.LongestStreak;
            } else {
                if (habit.IsInProgress) {
                    ApplyHabitCompletion(habit, halfXp: true);
                }

                habit.CurrentStreak = 0;
            }
            
            habit.FolderId = (int)FolderType.ToDo;
            habit.IsInProgress = false;
            habit.XpGainedToday = 0;
        }
        _profile.LastResetDate = DateTime.Today;
        SaveAllData();
        _storage.SaveProfile(_profile);
        return true;
    }

    public DeleteHabitResult DeleteHabit(int id) {
        var habit = _habits.FirstOrDefault(h => h.Id == id);

        if (habit == null)
            return new DeleteHabitResult { Success = false, Message = "Habit not Found." };


        _habits.Remove(habit);
        SaveAllData();
        return new DeleteHabitResult { Success = true, Message = $"Habit '{habit.Name}' Deleted." };
    }


    public List<Habit> GetHabits() {
        return _habits.OrderByDescending(h => h.CurrentStreak).ToList();
    }

    public void UpdateProfile(UserProfile profile) {
        _profile = profile;
        _storage.SaveProfile(_profile);
    }

    private void ApplyHabitCompletion(Habit habit, bool halfXp = false) {
        int xpEarned = (int)habit.Difficulty + Math.Max(0, habit.CurrentStreak - 1);
        if (halfXp)
            xpEarned = ((int)habit.Difficulty + habit.CurrentStreak) / 2;

        habit.XpGainedToday = xpEarned;
        habit.TotalXp += xpEarned;
        _profile.TotalXp += xpEarned;
    }
}