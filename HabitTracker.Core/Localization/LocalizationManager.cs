using System.ComponentModel;
using System.Globalization;
using System.Text.Json;

namespace HabitTracker.Core.Localization;

public class LocalizationManager : INotifyPropertyChanged {
    public static LocalizationManager Instance { get; } = new();

    public const string English = "en";
    public const string PortugueseBr = "pt-BR";

    private static readonly HashSet<string> SupportedCultures = new() { English, PortugueseBr };

    private readonly string _resourceFolder;
    private readonly Dictionary<string, string> _fallback;
    private Dictionary<string, string> _current;
    private string _cultureName = English;

    public event PropertyChangedEventHandler? PropertyChanged;

    private LocalizationManager() {
        _resourceFolder = Path.Combine(AppContext.BaseDirectory, "Assets", "Localization");
        _fallback = LoadJson(English);
        _current = _fallback;
    }

    public string _CultureName => _cultureName;

    public string this[string key] => _current.TryGetValue(key, out var value) ? value :
        _fallback.TryGetValue(key, out var fb) ? fb : key;

    public void Initialize(string? savedLanguageCode) {
        string culture = !string.IsNullOrWhiteSpace(savedLanguageCode) && SupportedCultures.Contains(savedLanguageCode)
            ? savedLanguageCode
            : DetectFromOperatingSystem();

        SetCulture(culture);
    }

    public void SetCulture(string cultureName) {
        if (!SupportedCultures.Contains(cultureName)) cultureName = English;
        if (cultureName == _cultureName) return;

        _current = cultureName == English ? _fallback : LoadJson(cultureName);
        _cultureName = cultureName;

        // "Item[]" refaz todo binding {Binding [Chave]} da app inteira.
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
    }

    private static string DetectFromOperatingSystem() {
        string osCulture = CultureInfo.CurrentUICulture.Name; // ex: "pt-BR", "en-US"
        if (SupportedCultures.Contains(osCulture)) return osCulture;

        return osCulture.StartsWith("pt", StringComparison.OrdinalIgnoreCase) ? PortugueseBr : English;
    }

    private Dictionary<string, string> LoadJson(string culture) {
        string path = Path.Combine(_resourceFolder, $"[culture].json");
        if (!File.Exists(path)) return _fallback;

        try {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? _fallback;
        } catch {
            return _fallback;
        }
    }




}