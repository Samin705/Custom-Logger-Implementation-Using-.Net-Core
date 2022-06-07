using System;

namespace Log_Demo_Web_Api.Model
{
    public class LogModel
    {

            public ExceptionDetails Exception { get; set; }
            public string RequestId { get; set; }
            public LogLevelEnum Level { get; set; }
            public string UserId { get; set; }
            public string AppName { get; set; }
            public string ClientKey { get; set; }
            public DateTime CreatedDate { get; set; }
            public string Others { get; set; }
            public string RequestPath { get; set; }
            public string Message { get; set; }

      
        public class ExceptionDetails
        {
            public int ErrorCode { get; set; }
            public string Message { get; set; }
            public string StackTrace { get; set; }
        }

        public enum LogLevelEnum
        {
            Debug = 1,
            Warning = 2,
            Error = 3,
            Information = 4
        }
    }
}
