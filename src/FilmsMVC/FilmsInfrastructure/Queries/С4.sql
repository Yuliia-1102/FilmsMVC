SELECT DISTINCT c2.Name, c2.Email
FROM Customers c2
WHERE c2.Id <> @CustomerId
AND EXISTS (
    SELECT *
    FROM Preorders p1
    JOIN Preorders p2 ON p1.FilmId = p2.FilmId
    WHERE p1.CustomerId = @CustomerId
    AND p2.CustomerId = c2.Id
)
AND NOT EXISTS (
    SELECT *
    FROM Preorders p3
    JOIN Films f1 ON p3.FilmId = f1.Id
    WHERE p3.CustomerId = @CustomerId
    AND NOT EXISTS (
        SELECT *
        FROM Preorders p4
        JOIN Films f2 ON p4.FilmId = f2.Id
        WHERE p4.CustomerId = c2.Id
        AND f2.Id = f1.Id
    )
);
