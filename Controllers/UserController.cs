using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAPICore.Data;
using MAPICore.Data.DTO;
using MAPICore.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MAPICore.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : DefaultController
    {
        private readonly Encryptor _encryptor;
        public UserController(ApplicationDbContext context, Encryptor encryptor): base(context)
        {
            _encryptor = encryptor;
            
        }

        // GET: api/User
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await Context.Users.SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
                return NotFound();

            return Ok(new
            {
                Id = user.Id,
                Email = user.EmailId,
                UserName = user.UserName
            });
        }

        // POST: api/User
        

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
