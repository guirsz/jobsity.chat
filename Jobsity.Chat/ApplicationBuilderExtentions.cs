using Jobsity.Chat.BotQueue;
using Jobsity.Chat.Data.Context;
using Jobsity.Chat.Domain.Entities;

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

        public static IApplicationBuilder DataBaseFeed(this IApplicationBuilder app)
        {
            MyContext context = app.ApplicationServices.GetService<MyContext>();
            context.Users.Add(new UserEntity()
            {
                Name = "Guilherme Souza",
                Email = "guirsz@gmail.com",
                Password = "AB0pKbH5oxV86+35xEZIk66RmdmwEuk8NtO+F6sCumXZDynEpwMs3cRVtMFiQj5SdQ==", //jobsity
            });
            context.Users.Add(new UserEntity()
            {
                Name = "Paola Condor",
                Email = "paola.condor@jobsity.com",
                Password = "AB0pKbH5oxV86+35xEZIk66RmdmwEuk8NtO+F6sCumXZDynEpwMs3cRVtMFiQj5SdQ==", //jobsity
            });
            context.SaveChanges();
            return app;
        }
    }
}
