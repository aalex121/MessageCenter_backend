using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MessageCenter.Models;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<User>> Get()
        {
            return _repository.GetUsers();            
        }

        // GET api/user/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            return _repository.Get(id);
        }

        // POST api/user
        [HttpPost]
        public User Post(User newUser)
        {
            return _repository.Create(newUser); 
        }

        // PUT api/user/5
        [HttpPut("{id}")]
        public User Put(int id, User user)
        {
            user.Id = id;
            return _repository.Update(user);
        }

        // DELETE api/user/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
