using System;
using System.Collections.Specialized;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using System.IO;

namespace LicenseManager.Utils
{
    public class Logwriter
    {
        const string eventLog = "Application";
        string eventSource = string.Empty;
        string outFileName = string.Empty;
        StreamWriter fileWriter = null;
        StringCollection fileOutput = new StringCollection();
        StringCollection debugOutput = new StringCollection();

        public Logwriter()
            : this(string.Empty)
        {
        }

        public Logwriter(string eventSourceName)
            : this(eventSourceName, string.Empty)
        {
        }

        public Logwriter(string eventSourceName, string fileName)
        {
            eventSource = eventSourceName;
            outFileName = fileName;
        }

        public void AddFileEventID(int eventID)
        {
            fileOutput.Add(eventID.ToString());
        }

        public void AddDebugEventID(int eventID)
        {
            debugOutput.Add(eventID.ToString());
        }

        public void WriteLogEntry(string message, EventLogEntryType type, int eventID)
        {
            if (fileOutput.Contains(eventID.ToString()))
            {
                if (fileWriter == null)
                {
                    if (outFileName.Trim().Equals(string.Empty))
                    {
                        throw new Exception("Filename can not be null");
                    }
                    fileWriter = new StreamWriter(outFileName, true);
                }
                WriteFileLogEntry(message, type, eventID);
            }
            else if (debugOutput.Contains(eventID.ToString()))
            {
                WriteDebugEntry(message, type, eventID);
            }
            else
            {
                if (!EventLog.SourceExists(eventSource))
                {
                    EventLog.CreateEventSource(eventSource, eventLog);
                }

                WriteEventLogEntry(message, type, eventID);
            }
        }

        private void WriteEventLogEntry(string message, EventLogEntryType type, int eventID)
        {
            EventLog.WriteEntry(eventSource, message, type, eventID);
        }

        private void WriteFileLogEntry(string message, EventLogEntryType type, int eventID)
        {

            fileWriter.WriteLine(string.Format("{0};{1:000000};{2:g};{3}", ConvertEventType(type), eventID, DateTime.Now, message));
            fileWriter.Flush();
        }

        private void WriteDebugEntry(string message, EventLogEntryType type, int eventID)
        {
            Debug.WriteLine(string.Format("{0};{1:000000};{2:g};{3}", ConvertEventType(type), eventID, DateTime.Now, message));
        }

        private string ConvertEventType(EventLogEntryType type)
        {
            string eType = string.Empty;
            switch (type)
            {
                case EventLogEntryType.Error:
                    eType = "ERROR";
                    break;
                case EventLogEntryType.FailureAudit:
                    eType = "FailureAudit";
                    break;
                case EventLogEntryType.Information:
                    eType = "Information";
                    break;
                case EventLogEntryType.SuccessAudit:
                    eType = "SuccessAudit";
                    break;
                case EventLogEntryType.Warning:
                    eType = "Warning";
                    break;
                default:
                    eType = "Not defined";
                    break;
            }
            return eType.PadLeft(20, ' ');
        }
    }
}