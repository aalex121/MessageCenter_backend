using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MessageCenter3.DAL.Repositories;
using MC.DAL.DataModels.Users;

namespace MessageCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        // GET api/user
        [HttpGet]
        [Authorize(Roles = "1")]
        public ActionResult<IEnumerable<UserData>> Get()
        {
            return _repository.GetAllUsers();            
        }

        // GET api/user/5
        [HttpGet("{id}")]
        public ActionResult<UserData> Get(int id)
        {
            return _repository.GetUserById(id);
        }

        // POST api/user
        [HttpPost]
        public ActionResult<UserData> Post(UserData newUser)
        {
            if (_repository.GetUserByName(newUser.Name) != null)
            {
                return BadRequest(new { errorText = "Username already exists!" });
            }
            
            return _repository.CreateUser(newUser); 
        }

        // PUT api/user/5
        [HttpPut("{id}")]
        public ActionResult<UserData> Put(int id, UserData user)
        {
            if (_repository.GetUserByName(user.Name) != null)
            {
                return BadRequest(new { errorText = "Username already exists!" });
            }

            user.Id = id;
            return _repository.UpdateUserData(user);
        }

        // DELETE api/user/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repository.DeleteUser(id);
        }
    }
}
