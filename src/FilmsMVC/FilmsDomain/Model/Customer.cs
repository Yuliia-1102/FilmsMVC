using System;
using System.Collections.Generic;

namespace FilmsDomain.Model;

public partial class Customer : Entity
{
    public string? Name { get; set; } 

    public string? Email { get; set; } 

    public virtual ICollection<Preorder> Preorders { get; set; } = new List<Preorder>();
}
