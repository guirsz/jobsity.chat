using Newtonsoft.Json;
using System;

namespace Jobsity.Chat.Bot
{
    public class BotException
    {
        public BotException(Exception err, string message)
        {
            Message = message;
            ErrorMessage = err.Message;
            Error = JsonConvert.SerializeObject(err);
        }

        public string Message { get; private set; }
        public string ErrorMessage { get; private set; }
        public string Error { get; private set; }
    }
}
