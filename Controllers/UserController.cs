using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using StoreAPI.Model;
using StoreAPI.Services;
using Org.BouncyCastle.Crypto.Generators;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUservices _uservices;
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration, IUservices uservices)
        {
            _configuration = configuration;
            _uservices = uservices;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*"); // Cho phép tất cả nguồn gốc, hãy cân nhắc chỉ định nguồn gốc cụ thể
            var users = await _uservices.GetAllAsynsc();
            return Ok(users);
        }
      
      

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost("register")]
        public async Task<IActionResult> Post(User user)
        {
            if (Regex.IsMatch(user.Email, @"@yahoo\.com|@gmail\.com", RegexOptions.IgnoreCase))
            {
                // Kiểm tra xem email đã tồn tại hay chưa
                bool isEmailTaken = await _uservices.IsEmailExistsAsync(user.Email);

                if (!isEmailTaken)
                {
                    // Mã hóa mật khẩu trước khi lưu trữ
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                    user.PasswordHash = hashedPassword;

                    // Thực hiện thêm tài khoản người dùng vào dịch vụ
                    await _uservices.CreateAsync(user);
                    return Ok("created successfully");

                }
                else
                {
                    // Nếu email đã tồn tại, ném một ngoại lệ hoặc trả về thông báo lỗi
                    return Ok("Email đã tồn tại");
                }

            }
            else
            {
                // Nếu địa chỉ email không hợp lệ, trả về thông báo lỗi
                return BadRequest("Địa chỉ email phải chứa @yahoo.com hoặc @gmail.com.");
            }
             
           
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {
            // Thực hiện kiểm tra người dùng có tồn tại
            bool isEmailTaken = await _uservices.IsEmailExistsAsync(user.Email);

            if (!isEmailTaken)
            {
                return BadRequest("User not found.");
            }

            // Truy xuất thông tin người dùng từ cơ sở dữ liệu
            User storedUser = await _uservices.GetUserByEmailAsync(user.Email);

            // Kiểm tra mật khẩu
            if (!BCrypt.Net.BCrypt.Verify(user.PasswordHash, storedUser.PasswordHash))
            {
                return BadRequest("Wrong Password");
            }

            // Người dùng hợp lệ, tạo và trả về mã thông báo
            string token = CreateToken(storedUser);
            return Ok(new { Message = "Login successful", Token = token });
        }


        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),


            };

            if (user.Email.EndsWith("Admin@Admin.vip.com", StringComparison.OrdinalIgnoreCase))
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSetting:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
     
        [HttpGet("profile"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserProfile()
        {
            // Lấy thông tin người dùng hiện tại (có thể dựa trên mã thông báo hoặc phiên đăng nhập)
            // Thường, bạn cần lấy thông tin từ phiên đăng nhập hoặc mã thông báo JWT.
            // Trong ví dụ này, tôi giả sử bạn có một mã thông báo JWT và có thể lấy thông tin từ mã thông báo.

            // Lấy thông tin từ mã thông báo (Ví dụ: username và email được lưu trong mã thông báo JWT)
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            
            var user = await _uservices.GetByEmailAsync(email);

            if (user == null)
            {
                return NotFound("Không tìm thấy thông tin người dùng.");
            }

            // Kiểm tra xem email từ mã thông báo và email từ cơ sở dữ liệu có khớp không
            if (user.Email != email)
            {
                return Unauthorized("Không có quyền truy cập thông tin người dùng.");
            }

            // Trả về thông tin người dùng
            return Ok(user);
        }
    }
}
