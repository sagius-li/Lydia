using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.Control
{
    public class ErrorData
    {
        public enum ErrorLevelEnum
        {
            None = 0,
            Warning,
            Error
        }

        public string ErrorText { get; set; }
        public string ErrorType { get; set; }
        public DateTime LogDate { get; set; }
        public ErrorLevelEnum ErrorLevel { get; set; }

        public ErrorData(string errorText)
        {
            ErrorText = errorText;
            ErrorType = string.Empty;
            LogDate = DateTime.Now;
            ErrorLevel = ErrorLevelEnum.Error;
        }

        public ErrorData(string errorText, string errorType)
        {
            ErrorText = errorText;
            ErrorType = errorType;
            LogDate = DateTime.Now;
            ErrorLevel = ErrorLevelEnum.Error;
        }

        public ErrorData(string errorText, string errorType, ErrorLevelEnum errorLevel)
        {
            ErrorText = errorText;
            ErrorType = errorType;
            LogDate = DateTime.Now;
            ErrorLevel = errorLevel;
        }

        public ErrorData(string errorText, string errorType, DateTime logDate)
        {
            ErrorText = errorText;
            ErrorType = errorType;
            LogDate = logDate;
            ErrorLevel = ErrorLevelEnum.Error;
        }

        public ErrorData(string errorText, string errorType, DateTime logDate, ErrorLevelEnum errorLevel)
        {
            ErrorText = errorText;
            ErrorType = errorType;
            LogDate = logDate;
            ErrorLevel = errorLevel;
        }
    }

    public class ErrorControl
    {
        private List<ErrorData> _errorList = new List<ErrorData>();

        public bool HasError
        {
            get
            {
                return _errorList.Count != 0;
            }
        }

        public int ErrorCount { get; set; }

        public List<ErrorData> GetAllError()
        {
            return _errorList;
        }

        public ErrorData GetLastError()
        {
            return _errorList[_errorList.Count - 1];
        }

        public ErrorData PopError()
        {
            ErrorData retVal = _errorList[_errorList.Count - 1];
            _errorList.RemoveAt(_errorList.Count - 1);
            ErrorCount--;

            return retVal;
        }

        public void AddError(ErrorData errorData)
        {
            _errorList.Add(errorData);
            ErrorCount++;
        }

        public void Reset()
        {
            ErrorCount = 0;
            _errorList.Clear();
        }
    }
}
