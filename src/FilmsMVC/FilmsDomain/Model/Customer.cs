using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsDomain.Model;

public partial class Customer : Entity
{
    [Display(Name = "Ім'я")]
    public string? Name { get; set; }

    [Display(Name = "Пошта")]
    public string? Email { get; set; }

    public string? CardNumber { get; set; }

    public virtual ICollection<Preorder> Preorders { get; set; } = new List<Preorder>();
}
