# 🎯 HabitTracker

A gamified habit tracking desktop application with a Kanban workflow, progression systems, statistics, and achievements. Built with Avalonia using MVVM architecture.

## ✨ Features

### Core Functionality
- 📋 **Kanban Board** - Organize habits across To-Do, In-Progress, and Done columns
- 🏷️ **Categories** - Organize habits by Health, Study, Work, or Personal
- 🔍 **Category Filter** - Filter habits by category in real time
- 📖 **History Tracking** - View detailed completion history for each habit
- 💾 **Persistent Storage** - All data saved locally in JSON format (`AppData/Roaming/HabitTracker/`)
- 🔄 **Daily Reset** - Automatic reset at midnight with streak and XP resolution

### Kanban Flow
- **To-Do → In-Progress** - Mark a habit as in progress (partial XP on daily reset)
- **In-Progress → Done** - Complete for full XP and streak
- **To-Do → Done** - Skip directly for full XP and streak
- **Undo** - Move back from Done or In-Progress, reverting XP and completion

### Gamification Systems
- ⭐ **XP & Levels** - Dynamic progression system
  - 8 levels from Beginner to Legend
  - Full XP on Done, half XP on In-Progress at daily reset
  - Streak bonuses on top of difficulty base XP
- 🎚️ **Difficulty Levels** - Easy (5 XP), Normal (10 XP), Hard (20 XP), Legendary (50 XP)
- 🔥 **Streak Tracking** - Build momentum with daily consistency
- 🏆 **14 Achievements** - Unlock badges for milestones:
  - 🌱 First steps (create habits, first completion, 5 total)
  - 📅 Consistency (7, 30, 100-day streaks)
  - 🌈 Exploration (diverse categories, category mastery)
  - 🎖️ Progression (reach levels 3, 5, 7)
  - ⚡ XP milestones (100, 500, 1000 total XP)

### User Profile
- 👤 **Profile Tab** - View name, level, XP, streak, gender, date of birth and member since
- ✏️ **Edit Profile** - Update name, description, gender and date of birth
- 📊 **Statistics Tab** - Overview of total habits, completions, XP, level progress and best streak

## 🛠️ Technologies

- **Language:** C# / .NET 8 (Core) · .NET 10 (Desktop)
- **UI Framework:** Avalonia 11.x
- **MVVM Toolkit:** CommunityToolkit.Mvvm 8.2.1
- **Dialogs:** MessageBox.Avalonia 3.3.1.1
- **Patterns:** MVVM, Layered Architecture, Result Pattern
- **Data Storage:** JSON (SQLite migration planned)

## 📁 Project Structure

```
HabitTracker/
├── HabitTracker.Core/
│   ├── Models/
│   │   ├── Habit.cs
│   │   ├── HabitFolder.cs
│   │   ├── FolderType.cs
│   │   ├── Achievement.cs
│   │   ├── AchievementType.cs
│   │   ├── Category.cs
│   │   ├── Difficulty.cs
│   │   ├── Gender.cs
│   │   ├── UserProfile.cs
│   │   ├── StatisticsResult.cs
│   │   ├── CreateHabitResult.cs
│   │   ├── DeleteHabitResult.cs
│   │   └── MoveHabitResult.cs
│   ├── Services/
│   │   ├── HabitService.cs
│   │   ├── AchievementService.cs
│   │   ├── StatisticsService.cs
│   │   └── LevelSystem.cs
│   └── Data/
│       └── JsonStorage.cs
│
└── HabitTracker.Desktop/
    ├── Converters/
    ├── Models/
    │   ├── ActiveTab.cs
    │   └── SelectableAchievement.cs
    ├── ViewModels/
    │   ├── MainWindowViewModel.cs
    │   ├── KanbanColumnViewModel.cs
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
XP for level N = N² × 50 + N × 50

| Level | Name         | XP Required |
|-------|--------------|-------------|
| 1     | Beginner     | 100         |
| 2     | Apprentice   | 300         |
| 3     | Intermediate | 600         |
| 4     | Advanced     | 1000        |
| 5     | Expert       | 1500        |
| 6     | Master       | 2100        |
| 7     | Grandmaster  | 2800        |
| 8     | Legend       | 3600        |

## 🗺️ Roadmap

### ✅ Phase 1: Console Application
- [x] Core habit CRUD
- [x] Difficulty and XP system
- [x] Dynamic level progression
- [x] Category organization
- [x] Achievement system with 14 badges

### ✅ Phase 2: Core Migration
- [x] Migrate Models/Services/Data to shared Core library
- [x] Result pattern (CreateHabitResult, DeleteHabitResult, MoveHabitResult)
- [x] UserProfile with persistent TotalXp
- [x] GlobalLongestStreak tracking
- [x] JsonStorage saving to AppData/Roaming/HabitTracker/
- [x] Legacy JSON migration support

### 🔄 Phase 3: Desktop Application (IN PROGRESS)
- [x] Avalonia Desktop project with MVVM
- [x] Kanban board (To-Do, In-Progress, Done)
- [x] Kanban flow with XP rules (full on Done, half on In-Progress at reset)
- [x] Daily reset with streak and XP resolution
- [x] Level up and achievement popups on habit completion
- [x] Create habit dialog (Name, Description, Difficulty, Category)
- [x] Delete habit with confirmation
- [x] Habit history dialog
- [x] Category filter
- [x] Tab navigation (Habits, Achievements, Profile, Statistics)
- [x] Achievements tab with locked/unlocked distinction
- [x] Profile tab with user info and stats
- [x] Edit profile dialog (Name, Description, Gender, Date of Birth)
- [x] Statistics tab (overview, level progress, best streak)
- [ ] Profile preview dialog
- [ ] Refined UI/UX (custom popups, animations, themes)

### 🔮 Phase 4: Advanced Features
- [ ] Profile photo with file picker
- [ ] SQLite database migration
- [ ] Notifications and reminders
- [ ] Data export/import

### 🌟 Phase 5: Future
- [ ] Cross-platform mobile (Avalonia)
- [ ] Cloud synchronization
- [ ] Social features and challenges

---

**Version:** 0.3.0-alpha  
**Status:** Desktop in active development