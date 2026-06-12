namespace HabitTracker.Core.Models;

public class HabitFolder
{
    public int Id {  get; set; } 
    public string Name {  get; set; } = string.Empty;
    public int DisplayOrder {  get; set; }
    public DateTime CreatedAt {  get; set; }
} 