using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Log_Demo_Web_Api
{
    public class LoggerProvider : ILoggerProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        //public LoggerProvider(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}

        public ILogger CreateLogger(string categoryName)
        {
            return new CustomLogger(categoryName, "This is prefix: ");
        }

        public void Dispose()
        {
        }
    }
}
