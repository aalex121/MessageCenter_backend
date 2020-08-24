using AutoMapper;
using MC.DAL.DataModels.UserGroups;
using MC.DAL.Entities;

namespace MC.DAL.Mappers
{
    public class UserGroupMapper : IUserGroupMapper
    {
        public MapperConfiguration GroupInputToJoinConfig
        {
            get => GroupInputToJoin();            
        }

        public MapperConfiguration GroupInputToGroupConfig
        {
            get => GroupInputToGroup();
        }

        private MapperConfiguration GroupInputToJoin()
        {
            MapperConfiguration config = new MapperConfiguration(
                    cfg => cfg.CreateMap<UserGroupInputModel, JoinGroupRequestModel>()
                    .ForMember("UserId", opt => opt.MapFrom(c => c.CreatorId))
                    .ForMember("GroupId", opt => opt.MapFrom(c => c.Id))
            );

            return config;
        }

        private MapperConfiguration GroupInputToGroup()
        {
            MapperConfiguration config = new MapperConfiguration(
                cfg => cfg.CreateMap<UserGroupInputModel, UserGroup>()                
            );

            return config;
        }
    }
}
