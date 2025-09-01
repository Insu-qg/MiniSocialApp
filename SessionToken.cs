using System;

namespace MiniSocialApp.Security;

public class SessionToken
{
    public Guid Token { get; private set; }
    public string Username { get; private set; }

    public SessionToken(string username)
    {
        Username = username;
        Token = Guid.NewGuid();
    }
}
