using System;
using System.Collections.Generic;

namespace FilmsDomain.Model;

public partial class Film : Entity
{
    public int GenreId { get; set; }

    public int DirectorId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public short ReleaseYear { get; set; }

    public string? TrailerLink { get; set; }

    public float Price { get; set; }

    public string? BoxOffice { get; set; }

    public virtual ICollection<ActorsFilm> ActorsFilms { get; set; } = new List<ActorsFilm>();

    public virtual ICollection<CountriesFilm> CountriesFilms { get; set; } = new List<CountriesFilm>();

    public virtual Director Director { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;

    public virtual ICollection<Preorder> Preorders { get; set; } = new List<Preorder>();
}
