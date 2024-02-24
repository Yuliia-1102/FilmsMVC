using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsDomain.Model;

public partial class Genre : Entity
{
    /*public Genre() 
    {
        Films = new HashSet<Film>();
    
    */
    [Display(Name = "Назва жанру")]
    public string Name { get; set; } = null!;

    public virtual ICollection<Film> Films { get; set; } = new List<Film>();
}
