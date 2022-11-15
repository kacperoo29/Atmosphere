using System.Text;
using Atmosphere.Core.Enums;

namespace Atmosphere.Core.Models;

public class User : BaseUser
{
    protected User()
    {
        Username = string.Empty;
        Password = new byte[0];
        IsActive = false;
        Role = UserRole.User;
    }

    public static User Create(string username, string password)
    {
        var saltedPassword = PasswordUtil.GenerateSaltedHash(Encoding.UTF8.GetBytes(password));

        return new User
        {
            Username = username,
            Password = saltedPassword,
            IsActive = false,
            Role = UserRole.User
        };
    }

    public void MakeAdmin()
    {
        Role = UserRole.Admin;
        Update();
    }
}
