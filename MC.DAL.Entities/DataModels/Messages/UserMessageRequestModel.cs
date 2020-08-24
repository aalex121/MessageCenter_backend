namespace MC.DAL.DataModels.Messages
{
    public class UserMessageRequestModel
    {
        public int CurrentUserId { get; set; }
        public int CollocutorId { get; set; }
        public int MessageType { get; set; }
        public int Offset { get; set; }
    }
}
