using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsDomain.Model;

public partial class Country : Entity
{
    [Display(Name = "Назва країни")]
    public string Name { get; set; } = null!;

    public virtual ICollection<CountriesFilm> CountriesFilms { get; set; } = new List<CountriesFilm>();
}
