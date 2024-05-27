using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsDomain.Model;

public partial class Preorder : Entity
{
    [Display(Name = "Фільм")]
    public int FilmId { get; set; }

    [Display(Name = "Пошта клієнта")]
    public int CustomerId { get; set; }

    [Display(Name = "Статус замовлення")]
    public string? Status { get; set; }

    [Display(Name = "Дата замовлення")]
    public DateOnly PreorderDate { get; set; }

    [Display(Name = "Покупець")]
    public virtual Customer? Customer { get; set; }

    [Display(Name = "Фільм")]
    public virtual Film? Film { get; set; }

}
