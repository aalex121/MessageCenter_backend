using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MC.API.Models;
using MC.DAL.DataModels.Users;
using MessageCenter3.Authentication;
using MessageCenter3.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MessageCenter3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public AuthController(IUserRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        [Route("getUsername")]
        [HttpGet]
        public ActionResult<string> GetUsername()
        {
            return string.Format("Your Username: {0}", User.Identity.Name);
        }

        [HttpPost("/token")]
        public ActionResult<AuthResponse> Token(AuthRequest request)
        {
            UserData user = _repository.GetUserByNameAndPassword(request.Name, request.Password);

            if (user == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            ClaimsIdentity identity = GetIdentity(user);            

            var now = DateTime.UtcNow;
           
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AuthResponse { AccessToken = encodedJwt, UserName = identity.Name, UserId = user.Id };
        }

        private ClaimsIdentity GetIdentity(UserData user)
        { 
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.RoleId)                    
            };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
