using System;
using System.Collections.Generic;
using MessageCenter3.DAL.Repositories;
using MessageCenter3.Enums;
using MessageCenter3.Models;
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
        public ActionResult<List<MessageOutputModel>> GetByRecipient(UserMessageRequestModel request)
        {
            RecipientType messageType = (RecipientType)request.MessageType;

            return _repository.GetMessagesByRecipient(request.CollocutorId, messageType);
        }        

        [Route("[action]")]
        [HttpPost]
        public ActionResult<List<MessageOutputModel>> GetUserDialogue(UserMessageRequestModel request)
        {
            bool isMessageTypeValid = Enum.IsDefined(typeof(MessageTypes), request.MessageType);

            if (!isMessageTypeValid)
            {
                return BadRequest("Invalid Message Type!");
            }
            
            return _repository.GetDialogue(request);
        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult<List<MessageOutputModel>> GetGroupMessages(GroupMessageRequestModel request)
        {
            bool isMessageTypeValid = Enum.IsDefined(typeof(MessageTypes), request.MessageType);

            if (!isMessageTypeValid)
            {
                return BadRequest("Invalid Message Type!");
            }

            return _repository.GetGroupMessages(request);
        }

        // POST api/messages
        [HttpPost]
        public ActionResult<int> Post(MessageInputModel message)
        {
            return _repository.AddMessage(message);
        }

    }
}
