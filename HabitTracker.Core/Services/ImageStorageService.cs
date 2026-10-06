namespace HabitTracker.Core.Services;

public class ImageStorageService {
    private readonly string _imagesFolder;


    public ImageStorageService() : this(
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "HabitTracker", "images")) {
    }

    public ImageStorageService(string imagesFolder) {
        _imagesFolder = imagesFolder;
        Directory.CreateDirectory(_imagesFolder);
    }

    public string SaveImage(string sourceFilePath, string? oldRelativePath = null) {
        if (!File.Exists(sourceFilePath))
            throw new FileNotFoundException("Image file not found.", sourceFilePath);

        string extension = Path.GetExtension(sourceFilePath);
        string newFileName = $"{Guid.NewGuid():N}{extension}";
        string destinationFullPath = Path.Combine(_imagesFolder, newFileName);

        File.Copy(sourceFilePath, destinationFullPath, overwrite: false);

        if (!string.IsNullOrWhiteSpace(oldRelativePath))
            DeleteImage(oldRelativePath);

        return Path.Combine("images", newFileName).Replace('\\', '/');
    }

    public string? GetFullPath(string? relativePath) {
        if (string.IsNullOrWhiteSpace(relativePath)) return null;

        string fileName = Path.GetFileName(relativePath);
        string fullPath = Path.Combine(_imagesFolder, fileName);

        return File.Exists(fullPath) ? fullPath : null;
    }

    public void DeleteImage(string? relativePath) {
        string? fullPath = GetFullPath(relativePath);
        if (fullPath == null) return;

        try {
            File.Delete(fullPath);
        } catch {
            /* melhor esforço */
        }
    }
}