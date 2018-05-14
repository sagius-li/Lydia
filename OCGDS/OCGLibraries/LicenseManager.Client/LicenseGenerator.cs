using System;
using System.Collections.Generic;
using System.Text;
using LicenseManager.Utils;


namespace LicenseManager.Client
{
    class LicenseGenerator
    {
        private static string key = "m054890mwtruio895435489trec5489754mcrem894589m543cc4m45c89mthrem54c897547ehct43mhrecwnjm45c289m54c8mzc54c54289m54c28c542omrewm5475428n5428n4548905c2c5425c789425478c2n854324278998n54c98n754c28954274785254284528954c278n54c2097545c289547";

        public static string GenerateLicenseKeyV1(string clientName, string applicationName, string[] serverList, Int64 Licenses, DateTime expireDate, bool serverCheck, string[] customAtts)
        {
            try
            {
                string licenseString = string.Empty;
                string licVersion = "[1.0]";
                string appClient = string.Format("[Client=\\{0}\\]", clientName);
                string appName = string.Format("[ApplicationName=\\{0}\\]", applicationName);
                string servers = string.Empty;
                foreach (string server in serverList)
                {
                    servers = string.Format("{0}[Name=\\{1}\\]", servers, server);
                }
                string serverName = string.Format("[Servers={0}]", servers);
                string licensesCount = string.Format("[Licenses={0}]", Licenses.ToString());
                string expire = string.Format("[Expire={0}]", expireDate.ToString("yyyyMMdd"));
                string noservercheck = string.Format("[ServerCheck={0}]", serverCheck.ToString());
                string customparms = "[";

                foreach (string att in customAtts)
                {
                    customparms = customparms + string.Format("//{0}", att);
                }
                if (!(customparms.EndsWith("[")))
                {
                    customparms = customparms + "//";
                }
                customparms = customparms + "]";
                licenseString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", licVersion, appClient, appName, serverName, licensesCount, expire, noservercheck, customparms);

                string retVal = Encryption.EncryptData(key, licenseString);

                return retVal;
            }
            catch
            {
                throw new Exception("Error while generating license key");
            }
        }
    }
}
