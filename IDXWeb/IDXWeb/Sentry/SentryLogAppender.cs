using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Util;
using SharpRaven;
using SharpRaven.Data;

namespace IDXWeb
{
    public class SentryLogAppender : AppenderSkeleton
    {

        static string dsnKey = System.Configuration.ConfigurationManager.AppSettings["dsnKeySentry"];
        static string environmentSentry = System.Configuration.ConfigurationManager.AppSettings["environmentSentry"];

        protected override void Append(LoggingEvent loggingEvent)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            string someText = "Try To Sent to sentry";
            someText += Environment.NewLine + "DSN Key : " + dsnKey;
            someText += Environment.NewLine + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++";
            someText += Environment.NewLine + "Environment : " + environmentSentry;
            someText += Environment.NewLine + "Enabled protocols:   " + ServicePointManager.SecurityProtocol;
            someText += Environment.NewLine +"Available protocols: ";
            Boolean platformSupportsTls12 = false;
            foreach (SecurityProtocolType protocol in Enum.GetValues(typeof(SecurityProtocolType))) {
                someText += Environment.NewLine + protocol.GetHashCode();
                if (protocol.GetHashCode() == 3072) {
                    platformSupportsTls12 = true;
                }
            }

            try {
                var ravenClient = new RavenClient(dsnKey);
                ravenClient.Environment = environmentSentry;
                var sentryEvent = new SentryEvent(loggingEvent.ExceptionObject);
                ravenClient.Capture(sentryEvent);
                someText += Environment.NewLine + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++";
                someText += Environment.NewLine + "In test client change!";
                someText += Environment.NewLine + "Sentry Uri: " + ravenClient.CurrentDsn.SentryUri;
                someText += Environment.NewLine + "Port: " + ravenClient.CurrentDsn.Port;
                someText += Environment.NewLine + "Public Key: " + ravenClient.CurrentDsn.PublicKey;
                someText += Environment.NewLine + "Private Key: " + ravenClient.CurrentDsn.PrivateKey;
                someText += Environment.NewLine + "Project ID: " + ravenClient.CurrentDsn.ProjectID;
                someText += Environment.NewLine + "Success Send To Sentry";
                someText += Environment.NewLine + loggingEvent.Level.Name;
                File.WriteAllText(@"C:\Temp\sentryLog.txt", someText);
            } catch (Exception excp) {
                someText += "Error Message : " + Environment.NewLine + excp.Message;
                someText += "Stack Trace : " + Environment.NewLine + excp.StackTrace;
                try {
                    File.WriteAllText(@"C:\Temp\sentryLog.txt", someText);
                }
                catch (Exception e) { }
            }
        }
    }
}