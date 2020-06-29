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

        // GET api/messages
        [HttpGet]        
        public ActionResult<string> Get()
        {
            return "Message Controller here";
        }

        // POST api/messages
        [HttpPost]
        public void Post(MessageInputModel message)
        {
            _repository.AddMessage(message);
        }

    }
}
