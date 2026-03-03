namespace VideoGames.Domain;

public class VideoGame
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; } = default!;
    public string Platform { get; private set; } = default!;
    public int ReleaseYear { get; private set; }
    public decimal Price { get; private set; }

    private VideoGame() { } // For EF

    public VideoGame(string title, string platform, int releaseYear, decimal price)
    {
        SetTitle(title);
        SetPlatform(platform);
        SetReleaseYear(releaseYear);
        SetPrice(price);
    }

    public void Update(string title, string platform, int releaseYear, decimal price)
    {
        SetTitle(title);
        SetPlatform(platform);
        SetReleaseYear(releaseYear);
        SetPrice(price);
    }

    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.");
        Title = title.Trim();
    }

    private void SetPlatform(string platform)
    {
        if (string.IsNullOrWhiteSpace(platform)) throw new ArgumentException("Platform is required.");
        Platform = platform.Trim();
    }

    private void SetReleaseYear(int year)
    {
        if (year < 1970 || year > DateTime.UtcNow.Year + 1) throw new ArgumentException("Invalid ReleaseYear.");
        ReleaseYear = year;
    }

    private void SetPrice(decimal price)
    {
        if (price < 0) throw new ArgumentException("Price cannot be negative.");
        Price = price;
    }
}