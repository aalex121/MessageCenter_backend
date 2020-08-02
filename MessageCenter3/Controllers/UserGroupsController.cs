using System.Collections.Generic;
using MessageCenter3.DAL.Repositories;
using MessageCenter3.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace MessageCenter3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupsController : ControllerBase
    {
        private readonly IUserGroupsRepository _repository;
        private readonly IUserGroupMapper _mapper;

        public UserGroupsController(IUserGroupsRepository repository, IUserGroupMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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

        [Route("[action]/{userId}")]
        [HttpGet]
        public ActionResult<List<UserGroup>> GetAvailableUserGroups(int userId)
        {
            return _repository.GetAvailableUserGroups(userId);
        }

        // POST api/userGroups
        [HttpPost]
        public ActionResult<UserGroup> AddGroup(UserGroupInputModel newGroup)
        {
            newGroup.Id = _repository.AddUserGroup(newGroup).Id;
            Mapper mapper = new Mapper(_mapper.GroupInputToJoinConfig);

            JoinGroupRequestModel joinData = mapper.Map<UserGroupInputModel, JoinGroupRequestModel>(newGroup);
            _repository.AddUserToGroup(joinData);

            mapper = new Mapper(_mapper.GroupInputToGroupConfig);

            return mapper.Map<UserGroupInputModel, UserGroup>(newGroup);
        }

        // POST api/userGroups/AddUserToGroup
        [Route("[action]")]
        [HttpPost]
        public ActionResult<bool> JoinGroup(JoinGroupRequestModel request)
        {
            if (_repository.GetUserGroupById(request.GroupId) == null)
            {
                return BadRequest("The User Group does not exist!");
            }

            request.IsGroupAdmin = false;

            _repository.AddUserToGroup(request);

            return true;
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
        public void ExcludeUserFromGroup(JoinGroupRequestModel request)
        {
            _repository.ExcludeUserFromGroup(request);
        }


    }
}
