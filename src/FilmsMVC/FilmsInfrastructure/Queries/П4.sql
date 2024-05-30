SELECT SUM(f.Price) AS TotalPrice
FROM Films f
WHERE f.Id NOT IN (
    SELECT p.FilmId
    FROM Preorders p
    JOIN Customers c ON p.CustomerId = c.Id
    WHERE c.Name = @CustomerName AND p.Status = 'Куплено'
)
AND EXISTS (
    SELECT *
    FROM Customers c
    WHERE c.Name = @CustomerName
);

