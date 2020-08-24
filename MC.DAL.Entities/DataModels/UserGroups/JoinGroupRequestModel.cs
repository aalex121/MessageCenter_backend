namespace MC.DAL.DataModels.UserGroups
{
    public class JoinGroupRequestModel
    {
        public int UserId { get; set;}
        public int GroupId { get; set; }
        public bool IsGroupAdmin { get; set; }
    }
}
