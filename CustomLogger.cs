using Log_Demo_Web_Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Log_Demo_Web_Api.Model.LogModel;

namespace Log_Demo_Web_Api
{
    public class CustomLogger : ILogger
    {
        private readonly string CategoryName;
        private readonly string _logPrefix;
        private static HttpContext _httpContextAccessor => new HttpContextAccessor().HttpContext;

        public CustomLogger(string categoryName, string logPrefix/*, IHttpContextAccessor httpContextAccessor*/)
        {
            CategoryName = categoryName;
            _logPrefix = logPrefix; 
            //_httpContextAccessor = httpContextAccessor;
        }

        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            ExceptionDetails exceptionD = new ExceptionDetails();

           

            string url = "";

            if (_httpContextAccessor != null)

            {
               // var HttpContext = _httpContextAccessor.HttpContext.;


                var request = _httpContextAccessor.Request;
                UriBuilder uriBuilder = new UriBuilder();
                uriBuilder.Scheme = request.Scheme;
                uriBuilder.Host = request.Host.Host;
                uriBuilder.Path = request.Path.ToString();
                uriBuilder.Query = request.QueryString.ToString();

                 url = uriBuilder.Scheme + "://" + uriBuilder.Host + uriBuilder.Path;
            }
           

            // var url = _httpContextAccessor == null? "empty" : _httpContextAccessor.HttpContext.Request.GetEncodedUrl() ;
          //  string host = _httpContextAccessor == null ? "empty" : _httpContextAccessor.HttpContext.Request.Host.Value;


            string message = _logPrefix;
            if (formatter != null)
            {
                message += formatter(state, exception);
            }
            // Implement log writter as you want. I am using Console
            //  Console.WriteLine($"{logLevel.ToString()} - {eventId.Id} - {CategoryName} - {message}");
            bool remoteApiCallResult = false;
           // remoteApiCallResult = await CustomeLoggerApi(exceptionD,2, "test02", "test02", "test02", "test02", "test02", "test02", "test02");

            Console.Write($"{formatter(state, exception)}");

           // string messageFromOtherClass = $"{formatter(state, exception)}";

            // Console.WriteLine("hello");

           // string details = remoteApiCallResult ? "Operation completed successfully" : "Failed during step transition";
           //return new StringWithBooleanPair { status = remoteApiCallResult, details = details };
        }

   
     

        public async Task<bool> CustomeLoggerApi(ExceptionDetails exception,int level, string requestId,string userId,string appName,string clientKey,
                                                 string others,string requestPath,string message)
        {
            bool returnData = false;
            try
            {
                string url = "";
                url = "https://api.premisehq.co/logs/v2/log";

               // LogLevelEnum eLevel = Enum.Parse<LogLevelEnum>(level);

                LogModel logModel = new LogModel();
                logModel.Exception = exception;
                logModel.Level = (LogLevelEnum)level;
                logModel.RequestId = requestId;
                logModel.UserId = userId;
                logModel.AppName = appName;
                logModel.ClientKey = clientKey;
                logModel.Others = others;
                logModel.RequestPath = requestPath;
                logModel.Message = message;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                // httpWebRequest.Headers.Add("Authorization", headerValue);
                httpWebRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("premise:c0.CxfcodejE9^HrJ_85sdgf7654!!"));
                httpWebRequest.Credentials = new NetworkCredential("premise", "c0.CxfcodejE9^HrJ_85sdgf7654!!");

                using (var streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync()))
                {
                    string json = JsonConvert.SerializeObject(logModel);
                    await streamWriter.WriteAsync(json);
                }

                var httpResponse = (HttpWebResponse)(await (httpWebRequest.GetResponseAsync()));
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = await streamReader.ReadToEndAsync();

                    if ((int)httpResponse.StatusCode == 200)
                    {
                        returnData = true;
                    }
                }
            }
            catch { returnData = false; }
            return returnData;
        }
        private class NoopDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }

        public class StringWithBooleanPair
        {
            public bool status { get; set; }
            public string details { get; set; }
        }
    }
}
