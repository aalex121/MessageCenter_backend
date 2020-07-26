using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageCenter3.Models
{
    public class JoinGroupRequestModel
    {
        public int UserId { get; set;}
        public int GroupId { get; set; }
    }
}
