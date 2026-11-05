public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public bool IsAvailable { get; set; } = true;

    public void DisplayInfo()
    {
        System.Console.WriteLine($"Название: {Title}, Автор: {Author}, Год: {Year}, Доступна: {IsAvailable}");
    }

    public void SetTitle(string title)
    {
        Title = title;
    }

    public int CalculateBookAge(int currentYear)
    {
        return currentYear - Year;
    }
}