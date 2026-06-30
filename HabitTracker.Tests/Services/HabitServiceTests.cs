using HabitTracker.Core.Data;
using HabitTracker.Core.Models;
using HabitTracker.Core.Services;

namespace HabitTracker.Tests.Services;

public class HabitServiceTests
{
    private HabitService CreateService()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);
        var storage = new JsonStorage(tempPath);
        return new HabitService(storage);
    }

    // ─── CreateHabit ───────────────────────────────────────────

    [Fact]
    public void CreateHabit_WithValidName_ShouldSucceed()
    {
        var service = CreateService();
        var result = service.CreateHabit("Exercício");
        Assert.True(result.Success);
    }

    [Fact]
    public void CreateHabit_WithEmptyName_ShouldFail()
    {
        var service = CreateService();
        var result = service.CreateHabit("");
        Assert.False(result.Success);
    }

    [Fact]
    public void CreateHabit_ShouldAppearInGetHabits()
    {
        var service = CreateService();
        var result = service.CreateHabit("Leitura");
        Assert.Single(service.GetHabits());
    }

    // ─── MoveHabitToFolder ─────────────────────────────────────
    
    [Fact]
    public void MoveHabitToDone_ShouldGrantXp()
    {
        var service = CreateService();
        service.CreateHabit("Teste", difficulty: Difficulty.Normal);
        var habit = service.GetHabits().First();

        service.MoveHabitToFolder(habit.Id, (int)FolderType.Done);

        Assert.True(service.GetProfile().TotalXp > 0);
    }

    [Fact]
    public void MoveHabitToDone_ShouldSetStreakToOne()
    {
        var service = CreateService();
        service.CreateHabit("Teste");
        var habit = service.GetHabits().First();

        service.MoveHabitToFolder(habit.Id, (int)FolderType.Done);

        Assert.Equal(1, service.GetHabits().First().CurrentStreak);
    }

    [Fact]
    public void MoveHabitToDone_Twice_ShouldNotDoubleXp()
    {
        var service = CreateService();
        service.CreateHabit("Teste");
        var habit = service.GetHabits().First();

        service.MoveHabitToFolder(habit.Id, (int)FolderType.Done);
        int xpAfterFirst = service.GetProfile().TotalXp;

        service.MoveHabitToFolder(habit.Id, (int)FolderType.Done);
        int xpAfterSecond = service.GetProfile().TotalXp;

        Assert.Equal(xpAfterFirst, xpAfterSecond);
    }

    [Fact]
    public void MoveHabitBackFromDone_ShouldRemoveXp()
    {
        var service = CreateService();
        service.CreateHabit("Teste");
        var habit = service.GetHabits().First();

        service.MoveHabitToFolder(habit.Id, (int)FolderType.Done);
        int xpAfterDone = service.GetProfile().TotalXp;

        service.MoveHabitToFolder(habit.Id, (int)FolderType.ToDo);

        Assert.True(service.GetProfile().TotalXp < xpAfterDone);
    }
    
    // ─── ApplyHabitCompletion ──────────────────────────────────

    [Fact]
    public void MoveHabitToDone_EasyDifficulty_ShouldGrantMinimumFiveXp()
    {
        var service = CreateService();
        service.CreateHabit("Teste", difficulty: Difficulty.Easy);
        var habit = service.GetHabits().First();

        service.MoveHabitToFolder(habit.Id, (int)FolderType.Done);

        Assert.True(service.GetProfile().TotalXp >= 5);
    }

    [Fact]
    public void MoveHabitToDone_XpShouldNeverBeNegative()
    {
        var service = CreateService();
        service.CreateHabit("Teste", difficulty: Difficulty.Easy);
        var habit = service.GetHabits().First();

        service.MoveHabitToFolder(habit.Id, (int)FolderType.Done);

        Assert.True(service.GetProfile().TotalXp >= 0);
    }
    
    // ─── DeleteHabit ───────────────────────────────────────────

    [Fact]
    public void DeleteHabit_ShouldRemoveFromList()
    {
        var service = CreateService();
        service.CreateHabit("Teste");
        var habit = service.GetHabits().First();

        service.DeleteHabit(habit.Id);

        Assert.Empty(service.GetHabits());
    }

    [Fact]
    public void DeleteHabit_WithInvalidId_ShouldFail()
    {
        var service = CreateService();
        var result = service.DeleteHabit(999);
        Assert.False(result.Success);
    }
}