using Microsoft.AspNetCore.Identity;

namespace IdentityDb.Pg.Entity;

public class User : IdentityUser
{
    /// <summary>
    /// Инициалы пользователя
    /// </summary>
    public string? Initials { get; set; }
}