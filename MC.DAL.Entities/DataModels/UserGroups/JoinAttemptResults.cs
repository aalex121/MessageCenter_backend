using System;
using System.Collections.Generic;
using System.Text;

namespace MC.DAL.DataModels.UserGroups
{   
    public enum JoinAttemptResults
    {
        None = 0,
        Success = 1,
        AlreadyInGroup = 2,
        GroupNotFound = 3
    }
}
