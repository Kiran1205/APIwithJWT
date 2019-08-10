using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MAPICore.Data;
using MAPICore.Data.DTO;
using MAPICore.Data.Model;
using MAPICore.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MAPICore.Controllers
{
    [Produces("application/json")]
    [Route("api/token")]
    [ApiController]   
     public class TokenController : DefaultController
    {
        private readonly Encryptor _encryptor;
        private readonly IConfiguration _configuration;

        public TokenController(ApplicationDbContext context,Encryptor encryptor,IConfiguration config):base(context)
        {
            _encryptor = encryptor;
            _configuration = config;
        }

        [HttpPost]
        [Route("createuser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDto)
        {
            try
            {
                if (userDto == null)
                    return BadRequest();
                var user = new Data.Model.User()
                {
                    UserName = userDto.UserName,
                    Password = _encryptor.Encrypt(userDto.Password),
                    EmailId = userDto.EmailId
                };
                await Context.Users.AddAsync(user);
                await Context.SaveChangesAsync();

                return CreatedAtAction("", new { id = user.Id });

            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        // POST: api/Login
        [HttpPost] 
        public async Task<IActionResult> CreateToken([FromBody] LoginDTO login)
        {
            User user;
            try
            {
                user = await Context.Users.FirstOrDefaultAsync(m => m.UserName == login.UserName);


                if (user == null)
                    return Unauthorized();

                var password = _encryptor.Decrypt(user.Password);

                if (login.Password != password)
                {
                    return Unauthorized();
                }

                var token = BuildToken(user);

                await Context.AppInsight.AddAsync(new ApplicationInsight
                {
                    Type = "Login",
                    IpAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                    UserId = user.Id,
                    CreatedDate = DateTime.Now,
                    Message = $"Loin Sucessfully by {user.UserName.ToLower()}"
                });

                await Context.SaveChangesAsync();
                return Ok(new
                {
                    token = token,
                    name = user.UserName,
                    id = user.Id
                });
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private string BuildToken(User users)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,users.UserName),
                new Claim(JwtRegisteredClaimNames.Email,users.EmailId),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
