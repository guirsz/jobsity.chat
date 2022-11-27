using Jobsity.Chat.BotQueue;

namespace Jobsity.Chat
{
    public static class ApplicationBuilderExtentions
    {
        public static BotQueueOperations? Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<BotQueueOperations>();
            var life = app.ApplicationServices.GetService<Microsoft.AspNetCore.Hosting.IApplicationLifetime>();
            life.ApplicationStarted.Register(OnStarted);
            return app;
        }

        private static void OnStarted()
        {
            Listener.ReceiveMessageFromQueue();
        }
    }
}
