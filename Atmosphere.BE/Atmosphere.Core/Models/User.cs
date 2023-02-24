using System.Text;
using Atmosphere.Core.Enums;

namespace Atmosphere.Core.Models;

public class User : BaseUser
{
    public string Email { get; private set; }

    protected User()
    {
        Username = string.Empty;
        Email = string.Empty;
        Password = new byte[0];
        IsActive = true;
        Role = UserRole.User;
    }

    public static User Create(string email, string password)
    {
        var saltedPassword = PasswordUtil.GenerateSaltedHash(Encoding.UTF8.GetBytes(password));

        return new User
        {
            Username = email,
            Email = email,
            Password = saltedPassword,
            IsActive = true,
            Role = UserRole.User
        };
    }

    public void MakeAdmin()
    {
        Role = UserRole.Admin;
        Update();
    }
}
