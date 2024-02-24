using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsDomain.Model;

public partial class ActorsFilm : Entity
{

    [Display(Name = "Фільм")]
    public int FilmId { get; set; }

    [Display(Name = "Актор")]
    public int ActorId { get; set; }

    [Display(Name = "Роль")]
    public string? Role { get; set; }

    [Display(Name = "Актор")]
    public virtual Actor Actor { get; set; } = null!;

    [Display(Name = "Фільм")]
    public virtual Film Film { get; set; } = null!;
}
