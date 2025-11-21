namespace YarpGateway.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLogin login)
    {
        if (login.Username == "admin" && login.Password == "pass")
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, login.Username),
                new Claim("password", login.Password)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super_secret_key_123!4567890_secure_long_key"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString });
        }

        return Unauthorized();
    }
}

public class UserLogin
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
