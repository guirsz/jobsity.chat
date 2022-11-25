using Jobsity.Chat.Data.Repository;
using Jobsity.Chat.Domain.Interfaces;
using Jobsity.Chat.Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Jobsity.Chat.CrossCutting.DependencyInjection
{
    public static class ConfigureRepository
    {
        public static void ConfigureDependenciesRepository(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
        }
    }
}
