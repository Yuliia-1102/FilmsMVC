SELECT f.Name
FROM Films f
JOIN CountriesFilms cf ON f.Id = cf.FilmId
JOIN Countries c ON cf.CountryId = c.Id
JOIN Directors d ON f.DirectorId = d.Id
WHERE c.Name = 'США'
AND d.Name = @DirectorName;


