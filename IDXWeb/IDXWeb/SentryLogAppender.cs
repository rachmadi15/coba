using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Util;
using SharpRaven;
using SharpRaven.Data;
using SharpRaven.Log4Net.Extra;

namespace IDXWeb {
    public class SentryLogAppender : AppenderSkeleton {

        static string dsnKey = System.Configuration.ConfigurationManager.AppSettings["dsnKeySentry"];
        static string environmentSentry = System.Configuration.ConfigurationManager.AppSettings["environmentSentry"];

        protected override void Append(LoggingEvent loggingEvent) {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            var ravenClient = new RavenClient(dsnKey);
            ravenClient.Environment = environmentSentry;
            ravenClient.Capture(new SentryEvent(loggingEvent.ExceptionObject));
        }
    }
}