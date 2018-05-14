using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;

namespace LicenseManager.Utils
{
    public class FileLicenseProvider : LicFileLicenseProvider 
    {

        public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
        {
            ////todo: these lines are only for test purpose. delete it later
            //if (!EventLog.SourceExists("LicenseManager"))
            //    EventLog.CreateEventSource("LicenseManager", "Application");
            //EventLog eventlog = new EventLog();
            //eventlog.Source = "LicenseManager";
            //eventlog.WriteEntry("Testing!!");

            if (context.UsageMode == LicenseUsageMode.Runtime)
            {
                string licenseKey = GetLicenseKey(instance);
                string appName = string.Empty;

                try
                {
                    // using reflection
                    //appName = instance.GetType().GetField("appName", 
                    //    BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance).ToString();

                    appName = instance.ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
                }
                catch
                {
                    throw new LicenseException(type);
                }

                LicenseHandler licenseHandler = new LicenseHandler(licenseKey);

                if (!ValidateLicenseKey(licenseHandler, appName))
                {
                    throw new LicenseException(type);
                }

                return new RuntimeApplicationLicense(type, licenseHandler);
            }

            return null;
        }

        protected string GetLicenseKey(object instance)
        {
            try
            {
                // using reflection
                //string assemblyPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

                //string fileName = string.Format("{0}.{1}.lic",
                //    instance.GetType().GetField("clientName", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance).ToString(),
                //    instance.GetType().GetField("appName", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance).ToString());

                //string licFile = string.Format(@"{0}\{1}", assemblyPath , fileName);

                string licFile = instance.ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1];

                StreamReader licFileStream = new StreamReader(licFile);
                return licFileStream.ReadToEnd();
            }
            catch { }

            return string.Empty;
        }

        protected bool ValidateLicenseKey(LicenseHandler licenseHandler, string appName)
        {
            try
            {
                //LicenseHandler licenseHandler = new LicenseHandler(licenseKey);
                if (licenseHandler == null || appName == string.Empty)
                {
                    throw new NullReferenceException("The licensehandler reference may not be null.");
                }

                return !licenseHandler.Expired && licenseHandler.CheckAppName(appName);
            }
            catch { }

            return false;
        }
    }
}
