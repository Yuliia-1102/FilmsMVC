SELECT f.Name, f.TrailerLink
FROM Films f
JOIN Genres g ON f.GenreId = g.Id
WHERE g.Name = @GenreName AND f.ReleaseYear > @ReleaseYear;


