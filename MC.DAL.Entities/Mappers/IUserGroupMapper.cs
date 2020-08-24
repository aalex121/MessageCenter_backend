using AutoMapper;

namespace MC.DAL.Mappers
{
    public interface IUserGroupMapper
    {
        MapperConfiguration GroupInputToJoinConfig { get; }
        MapperConfiguration GroupInputToGroupConfig { get; }
    }
}
