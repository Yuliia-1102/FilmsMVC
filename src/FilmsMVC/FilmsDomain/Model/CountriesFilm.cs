using System;
using System.Collections.Generic;

namespace FilmsDomain.Model;

public partial class CountriesFilm : Entity
{
    public int FilmId { get; set; }

    public int CountryId { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual Film Film { get; set; } = null!;
}
