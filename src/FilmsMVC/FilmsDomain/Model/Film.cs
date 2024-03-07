using Microsoft.EntityFrameworkCore;
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
    [Display(Name = "Назва фільму")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Опис")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Рік виходу")]
    [Range(1900, 2024, ErrorMessage = "Рік виходу повинен бути не раніше 1900 і не пізніше поточного року")]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Рік повинен складатися з 4 цифр")]
    public short ReleaseYear { get; set; }

    [Display(Name = "Трейлер")]
    [RegularExpression(@"^https:\/\/.*", ErrorMessage = "Посилання на трейлер повинно починатися з https://")]
    public string? TrailerLink { get; set; }

    private float price;

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Ціна")]
    [Range(0, float.MaxValue, ErrorMessage = "Ціна не може бути від'ємною")]
    public float Price
    {
        get { return price; }
        set { price = value < 0 ? 0 : value; }
    }

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
