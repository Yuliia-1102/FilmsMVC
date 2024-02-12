using System;
using System.Collections.Generic;

namespace FilmsDomain.Model;

public partial class Actor : Entity
{
    public string Name { get; set; } = null!;

    public virtual ICollection<ActorsFilm> ActorsFilms { get; set; } = new List<ActorsFilm>();
}
