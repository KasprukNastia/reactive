using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

namespace Lesson12.Application.Src.Utils
{
    public class HttpResourceResolver
    {
        private readonly ILogger<HttpResourceResolver> _logger;

        public HttpResourceResolver(ILogger<HttpResourceResolver> logger)
        {
            _logger = logger;
        }

        public string ResourcePath(string fileName)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                TextReader inputStream = new StreamReader(assembly.GetManifestResourceStream(fileName));
                string result = inputStream.ReadToEnd();
                Uri resourceUri = new Uri(result);
                string location = resourceUri.AbsolutePath;
                _logger.LogInformation("File [" + fileName + "] location: " + location);
                return location;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
