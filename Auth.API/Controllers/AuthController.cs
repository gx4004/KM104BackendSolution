using Microsoft.AspNetCore.Mvc;
using Auth.API.Data;
using Auth.API.Models.Entities;
using Auth.API.DTOs;
using BCrypt.Net; // Şifre hashlemek için
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly IConfiguration _config; // appsettings.json'a erişmek için

        public AuthController(AuthDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // Örnek: POST http://localhost:5001/api/Auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            // 1) Aynı kullanıcı var mı?
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == dto.Username);
            if (existingUser != null)
            {
                return BadRequest("Bu kullanıcı adı zaten alınmış.");
            }

            // 2) Şifre hashle
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // 3) Yeni user oluştur
            var newUser = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = hashedPassword
            };

            // 4) Veritabanına kaydet
            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok("Kullanıcı oluşturuldu.");
        }

        // Örnek: POST http://localhost:5001/api/Auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            // 1) Kullanıcı var mı?
            var user = _context.Users.FirstOrDefault(u => u.Username == dto.Username);
            if (user == null)
            {
                return BadRequest("Kullanıcı bulunamadı.");
            }

            // 2) Şifre doğru mu?
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return Unauthorized("Şifre hatalı.");
            }

            // 3) Token oluşturmak için Claim'ler
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("userId", user.Id.ToString())
            };

            // 4) appsettings.json'daki JWT ayarlarını al
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // 5) Token'ı dön
            return Ok(new { token = tokenString });
        }
    }
}
