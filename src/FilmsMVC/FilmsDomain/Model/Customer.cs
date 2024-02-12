using System;
using System.Collections.Generic;

namespace FilmsDomain.Model;

public partial class Customer : Entity
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Preorder> Preorders { get; set; } = new List<Preorder>();
}
