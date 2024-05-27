SELECT c2.Name, c2.Email
FROM Customers c2
WHERE c2.Id <> @CustomerId
AND NOT EXISTS (
    SELECT f1.Id
    FROM Preorders p1
    JOIN Films f1 ON p1.FilmId = f1.Id
    WHERE p1.CustomerId = @CustomerId
    AND NOT EXISTS (
        SELECT 1
        FROM Preorders p2
        WHERE p2.CustomerId = c2.Id
        AND p2.FilmId = f1.Id
    )
)
AND EXISTS (
    SELECT 1
    FROM Preorders p3
    JOIN Films f3 ON p3.FilmId = f3.Id
    WHERE p3.CustomerId = c2.Id
    AND NOT EXISTS (
        SELECT 1
        FROM Preorders p4
        WHERE p4.CustomerId = @CustomerId
        AND p4.FilmId = f3.Id
    )
)
AND EXISTS (
    SELECT 1
    FROM Preorders p5
    WHERE p5.CustomerId = c2.Id
    AND p5.FilmId <> ALL (
        SELECT p6.FilmId
        FROM Preorders p6
        WHERE p6.CustomerId = @CustomerId
    )
);
