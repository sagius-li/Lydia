using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Win32;

using OCG.ExceptionHandler;

namespace OCG.Utility
{
    public class RegistryOperator
    {
        public ExceptionHandler.ExceptionHandler ExceptionHandler = new ExceptionHandler.ExceptionHandler();

        public object ReadRegKey(RegistryKey baseKey, string subKey, string keyName)
        {
            if (baseKey == null)
            {
                ExceptionHandler.AddException(new OCGException("Base Key cannot be null", ExceptionLevel.Error));
                return string.Empty;
            }

            RegistryKey rk = baseKey.OpenSubKey(subKey);

            if (rk == null)
            {
                ExceptionHandler.AddException(new OCGException(string.Format("Sub Key not found: {0}", subKey), ExceptionLevel.Error));
                return string.Empty;
            }

            try
            {
                return rk.GetValue(keyName.ToUpper());
            }
            catch (Exception e)
            {
                ExceptionHandler.AddException(new OCGException(e.Message, ExceptionLevel.Error));
                return string.Empty;
            }
        }

        public bool WriteRegKey(RegistryKey baseKey, string subKey, string keyName, object value)
        {
            if (baseKey == null)
            {
                ExceptionHandler.AddException(new OCGException("Base Key cannot be null", ExceptionLevel.Error));
                return false;
            }

            try
            {
                RegistryKey rk = baseKey.CreateSubKey(subKey);

                rk.SetValue(keyName.ToUpper(), value);

                return true;
            }
            catch (Exception e)
            {
                ExceptionHandler.AddException(new OCGException(e.Message, ExceptionLevel.Error));
                return false;
            }
        }

        public bool DeleteKey(RegistryKey baseKey, string subKey, string keyName)
        {
            if (baseKey == null)
            {
                ExceptionHandler.AddException(new OCGException("Base Key cannot be null", ExceptionLevel.Error));
                return false;
            }

            RegistryKey rk = baseKey.CreateSubKey(subKey);

            if (rk == null)
            {
                ExceptionHandler.AddException(new OCGException("Sub Key doesn't exist", ExceptionLevel.Information));
                return true;
            }

            try
            {
                rk.DeleteValue(keyName.ToUpper());
                return true;
            }
            catch (Exception e)
            {
                ExceptionHandler.AddException(new OCGException(e.Message, ExceptionLevel.Error));
                return false;
            }
        }

        public bool DeleteSubKeyTree(RegistryKey baseKey, string subKey)
        {
            if (baseKey == null)
            {
                ExceptionHandler.AddException(new OCGException("Base Key cannot be null", ExceptionLevel.Error));
                return false;
            }

            RegistryKey rk = baseKey.CreateSubKey(subKey);

            if (rk == null)
            {
                ExceptionHandler.AddException(new OCGException("Sub Key doesn't exist", ExceptionLevel.Information));
                return true;
            }

            try
            {
                baseKey.DeleteSubKeyTree(subKey);
                return true;
            }
            catch (Exception e)
            {
                ExceptionHandler.AddException(new OCGException(e.Message, ExceptionLevel.Error));
                return false;
            }
        }
    }
}
