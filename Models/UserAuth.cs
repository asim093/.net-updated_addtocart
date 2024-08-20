using System;
using System.Collections.Generic;

namespace landingpage.Models;

public partial class UserAuth
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Roleid { get; set; }

    public int Status { get; set; }
}
