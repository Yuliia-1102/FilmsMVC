using System;
using System.Collections.Generic;

namespace FilmsDomain.Model;

public partial class Preorder : Entity
{
    public int FilmId { get; set; }

    public int CustomerId { get; set; }

    public DateOnly PreorderDate { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Film? Film { get; set; }
}
