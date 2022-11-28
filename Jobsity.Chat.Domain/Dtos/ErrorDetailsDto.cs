﻿using System.Text.Json;

namespace Jobsity.Chat.Domain.Dtos
{
    public class ErrorDetailsDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
