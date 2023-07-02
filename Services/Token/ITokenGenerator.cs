using Jwt.Models;

namespace Jwt.Services.Token
{
    public interface ITokenGenerator
    {
        public string CreateToken(User user, string tokenKey);
    }
}
