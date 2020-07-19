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

        [Route("[action]")]
        [HttpPost]
        public ActionResult<List<MessageOutputModel>> GetByRecipient(MessageRequestModel request)
        {
            RecipientType messageType = (RecipientType)request.MessageType;

            return _repository.GetMessagesByRecipient(request.RecipientId, messageType);
        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult<List<MessageOutputModel>> GetFromTo(MessageRequestModel request)
        {
            RecipientType messageType = (RecipientType)request.MessageType;

            return _repository.GetMessagesFromTo(request.SenderId, request.RecipientId, messageType);
        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult<List<MessageOutputModel>> GetDialogue(MessageRequestModel request)
        {
            return _repository.GetDialogue(request.SenderId, request.RecipientId);
        }

        // POST api/messages
        [HttpPost]
        public ActionResult<int> Post(MessageInputModel message)
        {
            return _repository.AddMessage(message);
        }

    }
}
