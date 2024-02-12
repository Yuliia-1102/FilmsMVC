using System;
using System.Collections.Generic;

namespace FilmsDomain.Model;

public partial class Country : Entity
{
    public string Name { get; set; } = null!;

    public virtual ICollection<CountriesFilm> CountriesFilms { get; set; } = new List<CountriesFilm>();
}
