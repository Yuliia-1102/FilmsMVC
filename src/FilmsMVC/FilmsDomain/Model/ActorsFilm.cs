using System;
using System.Collections.Generic;

namespace FilmsDomain.Model;

public partial class ActorsFilm : Entity
{
    public int FilmId { get; set; }

    public int ActorId { get; set; }

    public string Role { get; set; } = null!;

    public virtual Actor Actor { get; set; } = null!;

    public virtual Film Film { get; set; } = null!;
}
