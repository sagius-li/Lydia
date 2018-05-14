using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ExceptionHandler
{
    public class ExceptionHandler
    {
        private List<OCGException> exceptionList = new List<OCGException>();

        public void AddException(OCGException ex)
        {
            exceptionList.Insert(0, ex);
        }

        public void AddRangeExceptions(List<OCGException> exList)
        {
            exceptionList.InsertRange(0, exList);
        }

        public bool HasException
        {
            get
            {
                return exceptionList.Count != 0;
            }
        }

        public bool HasInformation
        {
            get
            {
                return exceptionList.Exists(e => e.Level == ExceptionLevel.Information);
            }
        }

        public bool HasWarning
        {
            get
            {
                return exceptionList.Exists(e => e.Level == ExceptionLevel.Warning);
            }
        }

        public bool HasError
        {
            get
            {
                return exceptionList.Exists(e => e.Level == ExceptionLevel.Error);
            }
        }

        public OCGException PopFirstInformation()
        {
            OCGException retVal = exceptionList.First(e => e.Level == ExceptionLevel.Information);
            exceptionList.Remove(retVal);

            return retVal;
        }

        public List<OCGException> PopAllInformation()
        {
            List<OCGException> retVal = exceptionList.FindAll(e => e.Level == ExceptionLevel.Information);

            exceptionList.RemoveAll(e => e.Level == ExceptionLevel.Information);

            return retVal;
        }

        public OCGException PopFirstWarning()
        {
            OCGException retVal = exceptionList.First(e => e.Level == ExceptionLevel.Warning);
            exceptionList.Remove(retVal);

            return retVal;
        }

        public List<OCGException> PopAllWarning()
        {
            List<OCGException> retVal = exceptionList.FindAll(e => e.Level == ExceptionLevel.Warning);

            exceptionList.RemoveAll(e => e.Level == ExceptionLevel.Warning);

            return retVal;
        }

        public OCGException PopFirstError()
        {
            OCGException retVal = exceptionList.First(e => e.Level == ExceptionLevel.Error);
            exceptionList.Remove(retVal);

            return retVal;
        }

        public List<OCGException> PopAllError()
        {
            List<OCGException> retVal = exceptionList.FindAll(e => e.Level == ExceptionLevel.Error);

            exceptionList.RemoveAll(e => e.Level == ExceptionLevel.Error);

            return retVal;
        }

        public void Clear()
        {
            exceptionList.Clear();
        }
    }
}
