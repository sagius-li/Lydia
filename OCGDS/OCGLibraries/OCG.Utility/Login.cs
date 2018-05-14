using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace OCG.Utility
{
    public enum LoginType
    {
        None = 0,
        DebugView = 1,
        File = 2,
        Database = 3,
        EventView = 4,
        Console = 5
    }

    public enum MessageType
    {
        Error = 1,
        Info = 2,
        Warn = 4
    }

    public class LoginMessage
    {
        public MessageType Type { get; set; }
        public string Message { get; set; }
    }

    [Serializable()]
    public class Login
    {
        // General Properties
        private List<string> errorList = new List<string>();

        private StringBuilder fileLogBuilder = new StringBuilder();

        public List<string> ErrorList
        {
            get
            {
                return errorList;
            }
        }

        public const int StackTraceMinLevel = 1;
        public const int StackTraceMaxLevel = 10;

        public LoginType LoginType { get; set; }

        public int LoginMessageType { get; set; }

        public string Separator { get; set; }

        public int StackTraceLevel { get; set; }

        public bool LogStackTrace { get; set; }

        public bool LogDate { get; set; }

        public string LogFileName { get; set; }

        // Properties for EventView Login
        public string EventViewAppName { get; set; }

        public Login()
        {
            LoginType = Utility.LoginType.DebugView;
            LoginMessageType = (int)MessageType.Error | (int)MessageType.Info | (int)MessageType.Warn;
            Separator = "\t";
            StackTraceLevel = 1;
            LogStackTrace = true;
            LogDate = true;
            LogFileName = string.Empty;

            EventViewAppName = "OCG Login";
        }

        public void Error(int? code, string message)
        {
            Log(code, new LoginMessage() { Type = MessageType.Error, Message = message }, 1);
        }

        public void Info(int? code, string message)
        {
            Log(code, new LoginMessage() { Type = MessageType.Info, Message = message }, 1);
        }

        public void Warn(int? code, string message)
        {
            Log(code, new LoginMessage() { Type = MessageType.Warn, Message = message }, 1);
        }

        public void Log(int? code, LoginMessage message, int skipFrames)
        {
            if (LoginType == Utility.LoginType.None)
            {
                return;
            }

            if (message.Type == MessageType.Error)
            {
                errorList.Add(message.Message);
            }

            string logDate = LogDate ? DateTime.Now.ToString("HH:mm:ss") : string.Empty;

            string logCode = code == null ? string.Empty : code.ToString();

            string logStack = string.Empty;
            if (LogStackTrace && StackTraceLevel > 0)
            {
                StackTrace st = new StackTrace(skipFrames);

                int level = st.FrameCount < StackTraceLevel ? st.FrameCount : StackTraceLevel;

                for (int i = level; i >0; i--)
                {
                    logStack = string.Format("{0}->{1}", logStack, st.GetFrame(i).GetMethod());
                }

                if (!string.IsNullOrEmpty(logStack))
                {
                    logStack = logStack.Substring(2);
                }
            }

            writeLog(logDate, logCode, logStack, message);
        }

        public void SaveLogFile(bool append = false)
        {
            if (string.IsNullOrEmpty(this.LogFileName) || this.fileLogBuilder.Length == 0)
            {
                return;
            }

            StreamWriter sw = new StreamWriter(this.LogFileName, append);

            sw.Write(this.fileLogBuilder);

            sw.Close();
        }

        private void writeLog(string logDate, string logCode, string logStack, LoginMessage logMessage)
        {
            switch (LoginType)
            {
                case LoginType.None:
                    break;
                case LoginType.DebugView:
                    writeDebugViewLog(logDate, logCode, logStack, logMessage);
                    break;
                case LoginType.File:
                    writeFileLog(logDate, logCode, logStack, logMessage);
                    break;
                case LoginType.Database:
                    break;
                case LoginType.EventView:
                    writeEventViewLog(logDate, logCode, logStack, logMessage);
                    break;
                case LoginType.Console:
                    writeConsoleLog(logDate, logCode, logStack, logMessage);
                    break;
                default:
                    break;
            }
        }

        private void writeFileLog(string logDate, string logCode, string logStack, LoginMessage logMessage)
        {
            if (string.IsNullOrEmpty(this.LogFileName))
            {
                return;
            }

            if (((int)LoginMessageType & (int)logMessage.Type) != (int)logMessage.Type)
            {
                return;
            }

            string outputString = string.Empty;

            if (!string.IsNullOrEmpty(logDate))
            {
                outputString = string.Format("{0}{1}", logDate, Separator);
            }

            if (!string.IsNullOrEmpty(logCode))
            {
                outputString = string.Format("{0}{1}{2}", outputString, logCode, Separator);
            }

            if (!string.IsNullOrEmpty(logStack))
            {
                outputString = string.Format("{0}{1}{2}", outputString, logStack, Separator);
            }

            outputString = string.Format("{0}{1}: {2}", outputString, logMessage.Type.ToString(), logMessage.Message);

            fileLogBuilder = fileLogBuilder.AppendLine(outputString);
        }

        private void writeConsoleLog(string logDate, string logCode, string logStack, LoginMessage logMessage)
        {
            if (((int)LoginMessageType & (int)logMessage.Type) != (int)logMessage.Type)
            {
                return;
            }

            string outputString = string.Empty;

            if (!string.IsNullOrEmpty(logDate))
            {
                outputString = string.Format("{0}{1}", logDate, Separator);
            }

            if (!string.IsNullOrEmpty(logCode))
            {
                outputString = string.Format("{0}{1}{2}", outputString, logCode, Separator);
            }

            if (!string.IsNullOrEmpty(logStack))
            {
                outputString = string.Format("{0}{1}{2}", outputString, logStack, Separator);
            }

            outputString = string.Format("{0}{1}: {2}", outputString, logMessage.Type.ToString(), logMessage.Message);

            Console.WriteLine(outputString);
        }

        private void writeDebugViewLog(string logDate, string logCode, string logStack, LoginMessage logMessage)
        {
            if (((int)LoginMessageType & (int)logMessage.Type) != (int)logMessage.Type)
            {
                return;
            }

            string outputString = string.Empty;

            if (!string.IsNullOrEmpty(logDate))
            {
                outputString = string.Format("{0}{1}", logDate, Separator);
            }

            if (!string.IsNullOrEmpty(logCode))
            {
                outputString = string.Format("{0}{1}{2}", outputString, logCode, Separator);
            }

            if (!string.IsNullOrEmpty(logStack))
            {
                outputString = string.Format("{0}{1}{2}", outputString, logStack, Separator);
            }

            outputString = string.Format("{0}{1}: {2}", outputString, logMessage.Type.ToString(), logMessage.Message);

            Debug.WriteLine(outputString);
        }

        private void writeEventViewLog(string logDate, string logCode, string logStack, LoginMessage logMessage)
        {
            if (((int)LoginMessageType & (int)logMessage.Type) != (int)logMessage.Type)
            {
                return;
            }

            int eventCode = 0;
            if (!string.IsNullOrEmpty(logCode))
            {
                int.TryParse(logCode, out eventCode);
            }

            string outputString = string.Format("Log Message: {0}\n", logMessage.Message);

            if (!string.IsNullOrEmpty(logStack))
            {
                outputString = string.Format("{0}Log Stack: {1}", outputString, logStack);
            }

            EventLog eventLog = new EventLog("Application", ".", EventViewAppName);

            EventLogEntryType logType;
            switch (logMessage.Type)
            {
                case MessageType.Error:
                    logType = EventLogEntryType.Error;
                    break;
                case MessageType.Info:
                    logType = EventLogEntryType.Information;
                    break;
                case MessageType.Warn:
                    logType = EventLogEntryType.Warning;
                    break;
                default:
                    logType = EventLogEntryType.Information;
                    break;
            }

            if (eventCode != 0)
            {
                eventLog.WriteEntry(outputString, logType, eventCode);
            }
            else
            {
                eventLog.WriteEntry(outputString, logType);
            }
        }
    }
}
