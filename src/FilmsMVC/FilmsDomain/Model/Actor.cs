using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsDomain.Model;

public partial class Actor : Entity
{
    [Display(Name = "Ім'я")]
    public string Name { get; set; } = null!;

    public virtual ICollection<ActorsFilm> ActorsFilms { get; set; } = new List<ActorsFilm>();
}
