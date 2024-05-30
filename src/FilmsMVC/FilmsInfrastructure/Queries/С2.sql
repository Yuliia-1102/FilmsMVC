SELECT f1.Name
FROM Films f1
WHERE NOT EXISTS (
    SELECT *
    FROM ActorsFilms af1
    JOIN Actors a1 ON af1.ActorId = a1.Id
    WHERE af1.FilmId = f1.Id
    AND NOT EXISTS (
        SELECT *
        FROM ActorsFilms af2
        JOIN Actors a2 ON af2.ActorId = a2.Id
        WHERE af2.FilmId = @FilmId
        AND af2.ActorId = af1.ActorId
    )
)
AND NOT EXISTS (
    SELECT *
    FROM ActorsFilms af3
    JOIN Actors a3 ON af3.ActorId = a3.Id
    WHERE af3.FilmId = @FilmId
    AND NOT EXISTS (
        SELECT *
        FROM ActorsFilms af4
        JOIN Actors a4 ON af4.ActorId = a4.Id
        WHERE af4.FilmId = f1.Id
        AND af4.ActorId = af3.ActorId
    )
)
AND f1.Id <> @FilmId;
