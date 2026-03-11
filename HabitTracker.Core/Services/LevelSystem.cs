namespace HabitTracker.Core.Services;

public static class LevelSystem
{
    /// Calcula quanto XP é necessário para COMPLETAR um nível específico
    /// Exemplo: Nível 1 precisa de 100 XP, Nível 2 precisa de 300 XP
    public static int GetXpRequiredForLevel(int level)
    {
        return (level * level * 50) + (level * 50);
    }

    /// Calcula o XP TOTAL acumulado necessário para CHEGAR em um nível
    /// Exemplo: Para chegar no nível 3, precisa de 400 XP total (100 + 300)
    public static int GetTotalXpForLevel(int level)
    {
        int totalXp = 0;

        for (int i = 0; i < level; i++)
        {
            totalXp += GetXpRequiredForLevel(i);
        }

        return totalXp;
    }

    /// Descobre em qual NÍVEL/FASE você está baseado no seu XP total
    /// Exemplo: Com 450 XP, você está no nível 3
    public static int CalculateLevel(int totalXp)
    {
        int level = 1;
        int xpAccumulated = 0;

        while (true)
        {
            int XpForNextLevel = GetXpRequiredForLevel(level);

            if (xpAccumulated + XpForNextLevel > totalXp)
            {
                return level;
            }

            xpAccumulated += XpForNextLevel;
            level++;
        }
    }

    public static readonly string[] LevelNames =
    {
        "Beginner", // Nível 1
        "Apprentice", // Nível 2
        "Intermediate", // Nível 3
        "Advanced", // Nível 4
        "Expert", // Nível 5
        "Master", // Nível 6
        "Grandmaster", // Nível 7
        "Legend" // Nível 8
    };

    public static string GetLevelName(int level)
    {
        if (level <= 0)
        {
            return "None";
        }

        if (level > LevelNames.Length)
        {
            return "Beyond Legend";
        }

        return LevelNames[level - 1];
    }

    /// Retorna quanto XP é necessário para completar o nível atual
    /// e avançar para o próximo
    public static int GetXpForNextLevel(int currentLevel)
    {
        return GetXpRequiredForLevel(currentLevel);
    }

    /// Retorna quanto XP você já ganhou DENTRO do nível atual
    /// Exemplo: 450 XP total no nível 3 = 50 XP de progresso (450 - 400)
    public static int GetXpProgressInCurrentLevel(int totalXp, int currentLevel)
    {
        int xpAtStartOfLevel = GetTotalXpForLevel(currentLevel);

        return totalXp - xpAtStartOfLevel;
    }
}