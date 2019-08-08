using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MAPICore.Data;
using MAPICore.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MAPICore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : Controller
    {
        protected readonly ApplicationDbContext Context;
        private User _user;

        public DefaultController(ApplicationDbContext context)
        {
            Context = context;
        }

        protected User Currentuser => GetCurrentUser();

        private User GetCurrentUser()
        {
            if (_user != null)
                return _user;

            var currentUser = HttpContext.User;
            var nameClaims = currentUser.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier);


            if (nameClaims == null)
                return null;

            _user = Context.Users.SingleOrDefault(m => m.UserName == nameClaims.Value);
            return _user;
        }
    }
}