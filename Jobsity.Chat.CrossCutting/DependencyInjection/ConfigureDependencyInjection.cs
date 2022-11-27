using Jobsity.Chat.Domain.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Jobsity.Chat.CrossCutting.DependencyInjection
{
    public static class ConfigureDependencyInjection
    {
        public static void ConfigureQueue(this IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
            var queueConfigurations = new QueueConfigurations();
            new ConfigureFromConfigurationOptions<QueueConfigurations>(configuration.GetSection("QueueConfigurations")).Configure(queueConfigurations);
            services.AddSingleton(queueConfigurations);
        }
    }
}
