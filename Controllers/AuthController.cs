using Jwt.Models;
using Jwt.Models.Dtos;
using Jwt.Services.Password;
using Jwt.Services.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IPasswordManager _passwordEncryptor;
        private readonly ITokenGenerator _tokenManager;
        private readonly IConfiguration _configuration;
        private static User user = new();

        public AuthController(IConfiguration configuration, IPasswordManager passwordEncryptor, ITokenGenerator tokenManager)
        {
            _configuration = configuration;
            _passwordEncryptor = passwordEncryptor;
            _tokenManager = tokenManager; 
        }

        [HttpPost("register")]
        public ActionResult<User> RegisterUser(UserDto userDto)
        {
            _passwordEncryptor.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = userDto.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<string> Login(UserDto userDto)
        {
            if (!user.Username.Equals(userDto.Username, StringComparison.Ordinal))
            {
                return BadRequest("User not found!");
            }

            if (!_passwordEncryptor.VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password!");
            }

            string jwtToken = _tokenManager.CreateToken(user, _configuration.GetSection("AppSettings:Token").Value);
             
            return Ok(jwtToken);
        }
    }
}
