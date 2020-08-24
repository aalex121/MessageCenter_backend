using System;
using System.Collections.Generic;
using System.Text;

namespace MC.DAL.DataModels.Contacts
{
    class NewContactRequest
    {
        public int OwnerId { get; set; }
        public int ContactId { get; set; }
    }
}
