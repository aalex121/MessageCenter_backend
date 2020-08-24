using MC.DAL.Entities;

namespace MC.DAL.DataModels.UserGroups
{
    public class UserGroupInputModel : UserGroup
    {
        public int CreatorId { get; set; }        
    }
}
