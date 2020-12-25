using AuthAPI.Models;
using AuthAPI.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace AuthAPI.Controllers
{
    [ApiController,
    OpenApiTag(nameof(AuthController), Description = "Registration of new user"),
    Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRegistrator _userRegistrator;

        public AuthController(IUserRegistrator userRegistrator)
        {
            _userRegistrator = userRegistrator ?? throw new ArgumentNullException(nameof(userRegistrator));
        }

        /// <summary>
        /// Реєстрація користувача
        /// </summary>
        [HttpPost,
        Produces(MediaTypeNames.Application.Json),
        Consumes(MediaTypeNames.Application.Json),
        ProducesResponseType(typeof(RegisteredUser), StatusCodes.Status200OK)]
        public async Task<ObjectResult> Register([FromBody]NewUser newUser)
        {
            return new ObjectResult(await _userRegistrator.RegisterUserAsync(newUser));
        }
    }
}
