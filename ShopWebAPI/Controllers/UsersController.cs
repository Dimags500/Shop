using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ShopDAL;

namespace ShopWebAPI
{

    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {

        private ShopContext ShopContext;
        private ILogger<UsersController> logger;
        public UsersController(ShopContext context, ILogger<UsersController> Logger)
        {
            ShopContext = context;
            logger = Logger;
        }


        [HttpGet("")]
        public ActionResult<List<User>> GetAllusers()
        {
            return Ok(ShopContext.Users);

        }

        [HttpGet("{userID}")]
        public ActionResult<User> GetUser(int userID)
        {
            User user = FindUser(userID);
            if (user != null)
                return user;
            else
                return StatusCode(404);
        }

        [HttpPost("add/")]
        public ActionResult AddUser([FromBody] User user)
        {

            logger.LogInformation($"{MethodBase.GetCurrentMethod().Name}");
            if (FindUser(user.UserID) == null)
            {
                ShopContext.Users.Add(user);
                ShopContext.SaveChanges();
                return Created($"{Request.Scheme}//:{Request.Host}/addUser{user.UserID}", user);
            }
            else
                return StatusCode(400);
        }

        [HttpPut("update/")]
        public ActionResult UpdatUser([FromBody] User user)
        {
            User newUser = user;
            User oldUser = FindUser(user.UserID);

            if (oldUser != null)
            {

                oldUser.Name = newUser.Name;
                oldUser.Email = newUser.Email;
                oldUser.PhoneNumber = newUser.PhoneNumber;

                ShopContext.SaveChanges();
                return StatusCode(202, newUser);
            }
            else
                return StatusCode(404);
        }



        [HttpDelete("delete/{userID}")]
        public ActionResult DeleteUser(int userID)
        {
            ShopContext.Users.Remove(FindUser(userID));
            ShopContext.SaveChanges();
            return StatusCode(200);
        }




        private User FindUser(int userID)
        {
            return ShopContext.Users.FirstOrDefault(u=>u.UserID == userID);
        }
    }
}
