using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsDomain.Model;

public partial class Director : Entity
{
	[Required(ErrorMessage = "Поле не повинно бути порожнім")]
	[Display(Name = "Ім'я")]
	public string Name { get; set; } = null!;

    public virtual ICollection<Film> Films { get; set; } = new List<Film>();
}
