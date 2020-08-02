using AutoMapper;

namespace MessageCenter3.Models
{
    public interface IUserGroupMapper
    {
        MapperConfiguration GroupInputToJoinConfig { get; }
        MapperConfiguration GroupInputToGroupConfig { get; }
    }
}
