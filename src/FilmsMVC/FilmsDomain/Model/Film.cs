using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsDomain.Model;

public partial class Film : Entity
{
    [Display(Name = "Жанр")]
    public int GenreId { get; set; }

    [Display(Name = "Режисер")]
    public int DirectorId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name="Назва фільму")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Опис")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Рік виходу")]
    public short ReleaseYear { get; set; }

    [Display(Name = "Трейлер")]
    public string? TrailerLink { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Ціна")]
    public float Price { get; set; }

    [Display(Name = "Касовий збір")]
    public string? BoxOffice { get; set; }

    [Display(Name = "Актори та ролі")]
    public virtual ICollection<ActorsFilm> ActorsFilms { get; set; } = new List<ActorsFilm>();

    [Display(Name = "Країни-виробники")]
    public virtual ICollection<CountriesFilm> CountriesFilms { get; set; } = new List<CountriesFilm>();

    [Display(Name = "Режисер")]
    public virtual Director Director { get; set; } = null!;

    [Display(Name = "Жанр")]
    public virtual Genre Genre { get; set; } = null!;

    public virtual ICollection<Preorder> Preorders { get; set; } = new List<Preorder>();
}
