# 🎯 Habit Tracker

A gamified console application to build and track daily habits with progression systems.

## Features

- ✅ Create and manage habits with customizable settings
- ✅ **Difficulty levels** - Easy, Normal, Hard, Legendary (affects XP rewards)
- ✅ **Dynamic level progression** - Duolingo-style XP system with 8+ levels
- ✅ **Category system** - Organize habits by Health, Study, Work, or Personal
- 🔥 **Streak tracking** - Build momentum with daily consistency
- ⭐ **XP rewards** - Earn XP based on difficulty + streak bonuses
- 📊 **Advanced statistics**:
  - Overall progress (total XP, level, completion rates)
  - Category breakdown with completion rates
  - Weekday performance analysis (best/worst days)
  - 7-day activity visualization (ASCII graph)
- 📖 Habit history with last 10 completions
- 🎯 Filter and view habits by category
- 💾 Persistent JSON storage

## Technologies

- **C# / .NET 8**
- **Architecture**: Layered service pattern (HabitService, StatisticsService, LevelSystem)
- **Data**: JSON serialization for local storage
- **Console**: UTF-8 with emoji support

## Project Structure
```
HabitTracker/
├── Models/
│   ├── Habit.cs           # Habit entity with XP, streaks, categories
│   ├── Difficulty.cs      # Difficulty enum (Easy to Legendary)
│   └── Category.cs        # Category enum (Health, Study, Work, Personal)
├── Services/
│   ├── HabitService.cs    # CRUD operations for habits
│   ├── StatisticsService.cs  # Advanced analytics and visualizations
│   └── LevelSystem.cs     # XP and level progression calculations
├── Data/
│   └── JsonStorage.cs     # Persistence layer
└── Program.cs             # Main console interface
```

## How to Run
```bash
cd HabitTracker
dotnet run
```

## Roadmap

### ✅ Completed (Console Version)
- [x] Difficulty system with XP scaling
- [x] Level progression system (dynamic formula)
- [x] Category organization
- [x] Advanced statistics and visualizations

### 🔜 Next Steps
- [ ] Achievement/badge system
- [ ] Desktop UI (WPF/Avalonia)
- [ ] Enhanced visualizations
- [ ] Notifications and reminders
- [ ] SQLite database migration
- [ ] Cross-platform support

---

**Version:** 1.4  
**Status:** Feature-complete console app, ready for desktop migration