SELECT DISTINCT g.Name
FROM Genres g
JOIN Films f ON g.Id = f.GenreId
JOIN Preorders p ON f.Id = p.FilmId
JOIN Customers c ON p.CustomerId = c.Id
WHERE c.Name = @CustomerName AND p.Status = 'Куплено';
