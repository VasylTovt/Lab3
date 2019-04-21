using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CourseWork.Web.Context;
using CourseWork.Web.Models;
using CourseWork.Web.Validator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace CourseWork.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly RailwayContext db;

        public UserController(RailwayContext context)
        {
            db = context;
        }

        [Authorize]
        [HttpGet("getUsers")]
        public async Task GetUsers()
        {
            if (db.Users.FirstOrDefault(u => (u.Email == User.Identity.Name)).Role == "admin")
            {
                Response.StatusCode = 200;
                await Response.WriteAsync(JsonConvert.SerializeObject(db.Users.ToList()));
            }
        }

        [HttpPost("register")]
        public async Task Register([FromBody] RegisterModel model)
        {
            var email = model.Email;
            var password = model.Password;
            var confirmPassword = model.ConfirmPassword;

            if (!EmailValidator.IsValid(email))
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Email is in invalid format");
                return;
            }
            
            if (db.Users.SingleOrDefault(u => u.Email == email) != null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Email is already in database");
                return;
            }

            if (password != confirmPassword)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Confirm password is wrong");
                return;
            }

            db.Users.Add(new User() { Email = email, Password = password });
            db.SaveChanges();

            Response.StatusCode = 200;
            await Response.WriteAsync("Register succeded!");
        }

        [HttpPost("login")]
        public async Task Login([FromBody] LoginModel model)
        {
            var email = model.Email;
            var password = model.Password;

            var user = db.Users.FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync(LoginStatus.WRONG_EMAIL + "");
                return;
            }
            else if (user.Password != password)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync(LoginStatus.WRONG_PASSWORD + "");
                return;
            }

            var identity = GetIdentity(user);

            var now = DateTime.UtcNow;
            
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromDays(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            Response.StatusCode = 200;
            await Response.WriteAsync(encodedJwt);
            /*var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));*/
        }

        private ClaimsIdentity GetIdentity(User user)
        {
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    //new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            
            return null;
        }
    }
}
