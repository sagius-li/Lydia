using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ExceptionHandler
{
    public class OCGException
    {
        public string Text { get; set; }
        public int ID { get; set; }
        public string Detail { get; set; }
        public ExceptionLevel Level { get; set; }

        public OCGException(string text, ExceptionLevel level)
        {
            this.Text = text;
            this.Level = level;
        }

        public OCGException(int id, string text, ExceptionLevel level)
        {
            this.ID = id;
            this.Text = text;
            this.Level = level;
        }
    }

    public enum ExceptionLevel
    {
        Information, 
        Warning, 
        Error
    }

    /// <summary>
    /// Exception ID is a 64 bit Integer
    ///     - 1-8 bits reserved for system messages
    ///     - 9-24 bits reserved for OCG.ResourceManagement.Searcher
    /// </summary>
    public enum ExceptionID
    {
        // system messages, max. 8 messages
        ImpersonationFailed = 1,
        DBConnectionFailed = 2,

        // OCG.ResourceManagement.Searcher, max. 16 messages
        ObjectKeyNotFound = 256,
        ObjectSIDNotFound = 512,
        ObjectIDNotFound = 1024,
        AttributeNotFound = 2048,
        ObjectTypeNotFound = 4096,
        UnsupportedDataType = 8192,
        UnavailableSearchQuery = 16384,

        NoReadRight = 32768,
        
    }
}
