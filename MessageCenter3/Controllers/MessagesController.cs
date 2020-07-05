using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageCenter3.Enums;
using MessageCenter3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessageCenter3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _repository;

        public MessagesController(IMessageRepository repository)
        {
            _repository = repository;
        }

        // GET api/messages/GetFromUser
        [Route("[action]/{userId}")]
        [HttpGet]        
        public ActionResult<List<Message>> GetFromUser(int userId)
        {
            return _repository.GetMessagesByAuthor(userId);
        }

        [Route("[action]/{userId}")]
        [HttpGet]
        public ActionResult<List<Message>> GetToUser(int userId)
        {
            return _repository.GetMessagesToUser(userId);
        }

        [Route("[action]/{userId}")]
        [HttpGet]
        public ActionResult<List<Message>> GetToGroup(int groupId)
        {
            return _repository.GetMessagesToGroup(groupId);
        }

        // POST api/messages
        [HttpPost]
        public void Post(MessageInputModel message)
        {
            _repository.AddMessage(message);
        }

    }
}
