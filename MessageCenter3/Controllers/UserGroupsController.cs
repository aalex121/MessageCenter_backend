using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageCenter3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessageCenter3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupsController : ControllerBase
    {
        private readonly IUserGroupsRepository _repository;

        public UserGroupsController(IUserGroupsRepository repository)
        {
            _repository = repository;
        }

        // GET api/userGroups/1
        [HttpGet("{id}")]        
        public ActionResult<UserGroup> Get(int id)
        {
            return _repository.GetUserGroupById(id);
        }

        // GET api/userGroups/SearchByName/myGroup
        [Route("[action]/{searchName}")]
        [HttpGet]
        public ActionResult<List<UserGroup>> SearchByName(string name)
        {
            return _repository.GetUserGroupsByName(name);
        }

        // GET api/userGroups/SearchByUserId/1
        [Route("[action]/{userId}")]
        [HttpGet]
        public ActionResult<List<UserGroup>> SearchByUserId(int userId)
        {
            return _repository.GetUserGroupsByUserId(userId);
        }

        // POST api/userGroups
        [HttpPost]
        public ActionResult<UserGroup> AddGroup(UserGroupInputModel newGroup)
        {
            UserGroup group = _repository.AddUserGroup(newGroup);
            _repository.AddUserToGroup(group.Id, newGroup.CreatorId);

            return group;
        }

        // POST api/userGroups/AddUserToGroup/1/1
        [Route("[action]/{userId}/{groupId}")]
        [HttpPost]
        public ActionResult<bool> AddUserToGroup(int userId, int groupId)
        {
            return _repository.AddUserToGroup(userId, groupId);
        }

        [HttpPut]
        public ActionResult<UserGroup> RenameGroup(UserGroup renamedGroup)
        {
            return _repository.RenameUserGroup(renamedGroup);
        }

        // DELETE api/userGroups/1        
        [HttpDelete("{id}")]
        public void DeleteGroup(int groupId)
        {
            _repository.DeleteUserGroup(groupId);
        }

        // DELETE api/userGroups/ExcludeUserFromGroup/1/1
        [Route("[action]/{userId}/{groupId}")]
        [HttpDelete]
        public void ExcludeUserFromGroup(int userId, int groupId)
        {
            _repository.ExcludeUserFromGroup(userId, groupId);
        }


    }
}
