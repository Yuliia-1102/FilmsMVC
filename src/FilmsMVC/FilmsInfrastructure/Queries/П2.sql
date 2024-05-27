SELECT DISTINCT a.Name
FROM Actors a
JOIN ActorsFilms af1 ON a.Id = af1.ActorId
JOIN Films f1 ON af1.FilmId = f1.Id
WHERE f1.Name = @FilmName
AND a.Id IN (
    SELECT af2.ActorId
    FROM ActorsFilms af2
    GROUP BY af2.ActorId
    HAVING COUNT(af2.FilmId) > 1
);


