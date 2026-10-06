using HabitTracker.Core.Localization;

namespace HabitTracker.Core.Models;

public static class TagLocalization {
    public static string GetLabel(TagType tag) => LocalizationManager.Instance[$"Tag.{tag}"];
    
    // Tags salvas no Habit são string (List<string>). Se o texto bater
    // com um TagType conhecido, traduz; se for uma tag customizada
    // criada pelo usuário, mostra como digitada, sem tradução.
    public static string GetLabel(string tagValue) =>
        Enum.TryParse<TagType>(tagValue, ignoreCase: true, out var tag) ? GetLabel(tag) : tagValue;
}