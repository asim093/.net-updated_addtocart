using System;
using System.Collections.Generic;

namespace landingpage.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Pname { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Price { get; set; }
}
