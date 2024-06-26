﻿SELECT c.Name, c.Email
FROM Customers c
WHERE EXISTS (
    SELECT *
    FROM Films f
    WHERE f.Price = @GivenPrice
)
AND NOT EXISTS (
    SELECT f.Id
    FROM Films f
    WHERE f.Price = @GivenPrice
    AND NOT EXISTS (
        SELECT p.FilmId
        FROM Preorders p
        WHERE p.CustomerId = c.Id
        AND p.FilmId = f.Id AND p.Status = 'Куплено'
    )
);


