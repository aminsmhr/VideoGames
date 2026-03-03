namespace VideoGames.Application.Contracts;

public record VideoGameDto(
    Guid Id,
    string Title,
    string Platform,
    int ReleaseYear,
    decimal Price
);

public record UpdateVideoGameRequest(
    string Title,
    string Platform,
    int ReleaseYear,
    decimal Price
);