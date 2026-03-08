# 🎯 Habit Tracker

A fully-featured gamified habit tracking application with progression systems, statistics, and achievements.

## ✨ Features

### Core Functionality
- ✅ **Habit Management** - Create, complete, and track daily habits
- 🏷️ **Categories** - Organize habits by Health, Study, Work, or Personal
- 📖 **History Tracking** - View detailed completion history for each habit
- 💾 **Persistent Storage** - All data saved locally in JSON format

### Gamification Systems
- ⭐ **XP & Levels** - Dynamic progression system (Duolingo-style formula)
  - 8+ levels from Beginner to Legend
  - XP rewards based on difficulty + streak bonuses
- 🎚️ **Difficulty Levels** - Easy (5 XP), Normal (10 XP), Hard (20 XP), Legendary (50 XP)
- 🔥 **Streak Tracking** - Build momentum with daily consistency
- 🏆 **14 Achievements** - Unlock badges for milestones:
  - 🌱 First steps (create habits, first completion)
  - 📅 Consistency (7, 30, 100-day streaks)
  - 🌈 Exploration (diverse categories, category mastery)
  - 🎖️ Progression (reach levels 3, 5, 7)
  - ⚡ XP milestones (100, 500, 1000 total XP)

### Advanced Statistics
- 📊 **Overall Stats** - Total XP, current level, completion rates
- 📚 **Category Breakdown** - Performance metrics per category
- 📅 **Weekday Analysis** - Discover your best and worst days
- 📈 **7-Day Activity Graph** - Visual ASCII chart of recent activity

## 🛠️ Technologies

- **Language:** C# / .NET 8
- **Architecture:** Layered service pattern with separation of concerns
- **Data Storage:** JSON serialization
- **UI:** Console (UTF-8 with emoji support)

## 📁 Project Structure
```
HabitTracker/
├── Models/
│   ├── Habit.cs              # Core habit entity
│   ├── Difficulty.cs         # Difficulty enum
│   ├── Category.cs           # Category enum
│   ├── Achievement.cs        # Achievement model
│   └── AchievementType.cs    # Achievement types enum
├── Services/
│   ├── HabitService.cs       # Habit CRUD operations
│   ├── StatisticsService.cs  # Analytics and visualizations
│   ├── AchievementService.cs # Achievement tracking and unlocking
│   └── LevelSystem.cs        # XP and level calculations
├── Data/
│   └── JsonStorage.cs        # Persistence layer
└── Program.cs                # Console UI and menu system
```

## 🚀 How to Run
```bash
cd HabitTracker
dotnet run
```

## 📸 Features Overview

### Menu Options
1. Create new habit (with difficulty and category selection)
2. List all habits (sorted by streak)
3. Complete habit today (earn XP, check for level ups and achievements)
4. View habit history (last 10 completions)
5. Show overall statistics
6. Filter habits by category
7. Category-specific statistics
8. Weekday performance analysis
9. 7-day activity graph
10. View all achievements
99. Delete habit

### Level Progression Formula
Dynamic XP requirements using Duolingo-style progression:
```
XP for level N = N² × 50 + N × 50
```

### Achievement Categories
- **Beginner** (3) - First habit, first completion, 5 total completions
- **Consistency** (3) - Streaks of 7, 30, and 100 days
- **Explorer** (2) - Diverse habits across categories, category mastery
- **Levels** (3) - Reach levels 3, 5, and 7
- **XP Milestones** (3) - Earn 100, 500, and 1000 total XP

## 🗺️ Roadmap

### ✅ Phase 1: Console Application (COMPLETE)
- [x] Core habit CRUD
- [x] Difficulty and XP system
- [x] Dynamic level progression
- [x] Category organization
- [x] Advanced statistics with separate service
- [x] Achievement system with 14 badges

### 🔄 Phase 2: Desktop Application (IN PROGRESS)
- [ ] Migrate to WPF/Avalonia
- [ ] Modern UI with visual statistics
- [ ] Interactive charts and graphs
- [ ] System tray integration
- [ ] Enhanced achievement visuals

### 🔮 Phase 3: Advanced Features
- [ ] Notifications and reminders
- [ ] SQLite database migration
- [ ] Data export/import
- [ ] Backup and sync capabilities
- [ ] Custom themes

### 🌟 Phase 4: Future Enhancements
- [ ] Cross-platform mobile app
- [ ] Cloud synchronization
- [ ] Social features and challenges
- [ ] Integration with productivity tools

## 📚 Learning Journey

This project demonstrates:
- **Clean Architecture** - Separation of concerns with service layers
- **SOLID Principles** - Single Responsibility, maintainable code
- **OOP Best Practices** - Encapsulation, proper use of enums and models
- **Console UI Design** - User-friendly menu system with visual formatting
- **Data Persistence** - JSON serialization for local storage
- **Gamification Design** - Progression systems, rewards, achievements

---

**Version:** 2.0 (Console Complete)  
**Status:** Production-ready console app, beginning desktop migration  
**Next:** WPF/Avalonia desktop implementation