SELECT DISTINCT f.Name
FROM Films f
INNER JOIN Preorders p ON f.Id = p.FilmId
INNER JOIN Customers c ON p.CustomerId = c.Id
WHERE c.Name = @CustomerName AND p.Status = 'Куплено';


