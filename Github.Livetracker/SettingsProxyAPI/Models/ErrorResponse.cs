using System;

namespace SettingsProxyAPI.Models
{
    public class ErrorResponse
    {
        public string Message { get; set; }

        public ErrorResponse(string message)
        {
            Message = message;
        }

        public ErrorResponse(Exception ex)
        {
            Message = ex.Message;
        }
    }
}
