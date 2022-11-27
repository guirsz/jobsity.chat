using Jobsity.Chat.Domain.Interfaces.Services;
using Jobsity.Chat.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Jobsity.Chat.CrossCutting.DependencyInjection
{
    public static class ConfigureService
    {
        public static void ConfigureDependenciesService(this IServiceCollection services)
        {
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IUserService, UserService>();
        }
    }
}
