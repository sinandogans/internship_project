using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Abstract;

namespace ServiceLayer.Controllers
{
    [Route("api/users")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }



        /// <summary>
        /// Gets all user objects from database and returns as a list.
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">Returns the users</response>
        /// <response code="500">Internal server error</response>

        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll()
        {
            var result = _userService.GetAll();
            return Ok(result);
        }




        /// <summary>
        /// Returns the single user object from database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User object</returns>
        /// <response code="200">Returns the user</response>
        /// <response code="404">If user is not exist</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("getbyid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetById(Guid id)
        {
            var result = _userService.GetById(id);
            return Ok(result);
        }




        /// <summary>
        /// Adds given user object to database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Success message</returns>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     POST
        ///     {
        ///         "username": "sampleusername",
        ///         "password": "samplepassword",
        ///         "email": "sample@sample.com",
        ///         "birthdate": "2000-00-25"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Returns success message</response>
        /// <response code="400">If email or username already taken</response>
        /// <response code="500">Internal server error</response>

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Add([FromBody] User user)
        {
            _userService.Add(user);
            return Ok("User successfully added");
        }




        /// <summary>
        /// Deletes the user object from database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success message</returns>
        /// <response code="200">Deletes the user</response>
        /// <response code="404">If the user is not exist</response>
        /// <response code="500">Internal server error</response>

        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(Guid id)
        {
            _userService.Delete(id);
            return Ok("User successfully deleted");
        }




        /// <summary>
        /// Changes the type of the user object to admin.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success message</returns>
        /// <response code="200">Authorizes the user</response>
        /// <response code="400">If the user is already admin</response>
        /// <response code="500">If token is not found or Authorization denied.</response>

        [HttpPut("auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AuthorizeUser(Guid id)
        {
            _userService.Authorize(id);
            return Ok("User is authorized");
        }




        /// <summary>
        /// Checks the user object if it is admin or not.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Message that indicates whether user is admin or not</returns>
        /// <response code="200">User is admin</response>
        /// <response code="400">User is not admin</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("checkifisadmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CheckIfIsAdmin(Guid id)
        {
            var check = _userService.CheckIfIsAdmin(id);
            if (!check)
                return BadRequest("User is not admin.");
            return Ok("User is admin.");
        }




        /// <summary>
        /// Returns a JWT token if data is true.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Jwt token</returns>
        /// <response code="200">Returns the token</response>
        /// <response code="404">If user is not found</response>
        /// <response code="400">If password is not correct</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login(string email, string password)
        {
            var token = _userService.Login(email, password);
            return Ok(token);
        }
    }
}
