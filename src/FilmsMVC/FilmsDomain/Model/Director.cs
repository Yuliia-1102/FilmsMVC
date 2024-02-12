using System;
using System.Collections.Generic;

namespace FilmsDomain.Model;

public partial class Director : Entity
{
    public string Name { get; set; } = null!;

    public virtual ICollection<Film> Films { get; set; } = new List<Film>();
}
