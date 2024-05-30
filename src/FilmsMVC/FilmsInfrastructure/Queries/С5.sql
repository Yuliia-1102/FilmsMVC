SELECT DISTINCT a1.Name
FROM Actors a1
WHERE EXISTS (
    SELECT *
    FROM ActorsFilms af1
    JOIN ActorsFilms af2 ON af1.FilmId = af2.FilmId
    WHERE af2.ActorId = a1.Id
    AND af1.ActorId = (SELECT Id 
                       FROM Actors 
                       WHERE Name = @ActorName)
)
AND a1.Name <> @ActorName;
