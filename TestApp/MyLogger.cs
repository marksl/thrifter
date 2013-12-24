using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace TestApp
{
    public class MyLogger : ILogger
    {
        private readonly List<string> errors = new List<string>();

        public string FailureMessage
        {
            get { return string.Join("\n", errors); }
        }

        public LoggerVerbosity Verbosity { get; set; }
        public string Parameters { get; set; }

        public void Initialize(IEventSource eventSource)
        {
            eventSource.ErrorRaised += eventSource_ErrorRaised;
        }

        public void Shutdown()
        {
        }

        private void eventSource_ErrorRaised(object sender, BuildErrorEventArgs e)
        {
            errors.Add(e.Message);
        }
    }
}