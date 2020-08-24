using MC.DAL.Mappers;
using MessageCenter3.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MC.DI.Initialize
{
    public class DALInitializer
    {
        private const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ADO_Test;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static void InitRepositories(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>(provider => new UserRepository(connectionString));
            services.AddTransient<IMessageRepository, MessageRepository>(provider => new MessageRepository(connectionString));
            services.AddTransient<IUserGroupsRepository, UserGroupsRepository>(provider => new UserGroupsRepository(connectionString));
            services.AddTransient<IUserGroupMapper, UserGroupMapper>(provider => new UserGroupMapper());
        }
    }
}
