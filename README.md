# 🎯 Habit Tracker

A fully-featured gamified habit tracking application with progression systems, statistics, and achievements. Built with Avalonia Desktop using MVVM architecture.

## ✨ Features

### Core Functionality
- ✅ **Habit Management** - Create, complete, and delete daily habits
- 🏷️ **Categories** - Organize habits by Health, Study, Work, or Personal
- 🔍 **Category Filter** - Filter habit list by category in real time
- 📖 **History Tracking** - View detailed completion history for each habit
- 💾 **Persistent Storage** - All data saved locally in JSON format (`AppData/Roaming/HabitTracker/`)

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

### User Profile
- 👤 **Profile Tab** - View name, level, XP, streak, gender, date of birth and member since
- ✏️ **Edit Profile** - Update name, description, gender and date of birth
- 🔒 **Permanent XP** - XP is never lost when deleting a habit

## 🛠️ Technologies

- **Language:** C# / .NET 8 (Core) and .NET 10 (Desktop)
- **UI Framework:** Avalonia 11.x
- **MVVM Toolkit:** CommunityToolkit.Mvvm 8.2.1
- **Dialogs:** MessageBox.Avalonia 3.3.1.1
- **Patterns:** MVVM, Layered Architecture, Result Pattern
- **Data Storage:** JSON → SQLite (planned)

## 📁 Project Structure

```
HabitTracker/ (Solution)
├── HabitTracker.Core/             # Class library (shared logic)
│   ├── Models/
│   │   ├── Habit.cs
│   │   ├── Achievement.cs
│   │   ├── AchievementType.cs
│   │   ├── Category.cs
│   │   ├── Difficulty.cs
│   │   ├── Gender.cs
│   │   ├── CompleteHabitResult.cs
│   │   ├── DeleteHabitResult.cs
│   │   ├── CreateHabitResult.cs
│   │   ├── StatisticsResult.cs
│   │   └── UserProfile.cs
│   ├── Services/
│   │   ├── HabitService.cs
│   │   ├── AchievementService.cs
│   │   ├── StatisticsService.cs
│   │   └── LevelSystem.cs
│   └── Data/
│       └── JsonStorage.cs
│
└── HabitTracker.Desktop/          # Avalonia Desktop app
    ├── Converters/
    │   ├── CategoryDisplayConverter.cs
    │   ├── TabVisibilityConverter.cs
    │   └── BoolToOpacityConverter.cs
    ├── Models/
    │   └── ActiveTab.cs
    ├── ViewModels/
    │   ├── MainWindowViewModel.cs
    │   ├── CreateHabitViewModel.cs
    │   ├── EditProfileViewModel.cs
    │   └── ViewModelBase.cs
    ├── Views/
    │   ├── MainWindow.axaml
    │   ├── CreateHabitDialog.axaml
    │   ├── HabitHistoryDialog.axaml
    │   └── EditProfileDialog.axaml
    ├── App.axaml
    └── Program.cs
```

## 🚀 How to Run

```bash
dotnet run --project HabitTracker.Desktop
```

## 📐 Level Progression Formula

Dynamic XP requirements using Duolingo-style progression:

```
XP for level N = N² × 50 + N × 50
```

| Level | Name        | XP Required |
|-------|-------------|-------------|
| 1     | Beginner    | 100         |
| 2     | Apprentice  | 300         |
| 3     | Intermediate| 600         |
| 4     | Advanced    | 1000        |
| 5     | Expert      | 1500        |
| 6     | Master      | 2100        |
| 7     | Grandmaster | 2800        |
| 8     | Legend      | 3600        |

## 🏆 Achievement Categories

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
- [x] Achievement system with 14 badges

### ✅ Phase 2: Core Migration (COMPLETE)
- [x] Migrate Models/Services/Data to shared Core library
- [x] Result pattern (CreateHabitResult, CompleteHabitResult, DeleteHabitResult)
- [x] UserProfile with persistent TotalXp
- [x] GlobalLongestStreak tracking
- [x] JsonStorage saving to AppData/Roaming/HabitTracker/

### 🔄 Phase 3: Desktop Application (IN PROGRESS)
- [x] Avalonia Desktop project setup with MVVM
- [x] Main window with habit list and Level/XP/ProgressBar header
- [x] Complete habit with XP, level up and achievement popups
- [x] Create habit dialog (Name, Description, Difficulty, Category)
- [x] Delete habit with confirmation
- [x] Habit history dialog
- [x] Category filter with nullable enum and ValueConverter
- [x] Tab navigation system with scalable ActiveTab enum
- [x] Achievements tab with locked/unlocked visual distinction
- [x] Profile tab with user info and stats
- [x] Edit profile dialog (Name, Description, Gender, Date of Birth)
- [ ] Profile preview dialog (public card view)
- [ ] Statistics tab
- [ ] Refined UI/UX (custom popups, animations, themes)

### 🔮 Phase 4: Advanced Features
- [ ] Profile photo with file picker
- [ ] Custom fonts and backgrounds in profile
- [ ] SQLite database migration (with soft delete)
- [ ] Notifications and reminders
- [ ] Data export/import

### 🌟 Phase 5: Future Enhancements
- [ ] Cross-platform mobile app (Avalonia)
- [ ] Cloud synchronization
- [ ] Social features and challenges

## 📚 Learning Journey

This project demonstrates:
- **Clean Architecture** - Separation of concerns with service layers
- **SOLID Principles** - Single Responsibility, maintainable code
- **OOP Best Practices** - Encapsulation, proper use of enums and models
- **MVVM Pattern** - ViewModel, data binding, ObservableCollection
- **ValueConverters** - IValueConverter for display logic in Avalonia
- **Data Persistence** - JSON serialization for local storage
- **Gamification Design** - Progression systems, rewards, achievements
- **Cross-platform UI** - Avalonia for Desktop (and future Mobile)

---

**Version:** 4.0  
**Status:** Desktop in active development  
**Next:** Profile preview dialog and statistics tab
