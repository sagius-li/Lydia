using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace LicenseManager.Utils.UnitTest
{
    [LicenseProvider(typeof(FileLicenseProvider))]
    public class TestLicenseManagerUtils
    {
        private License license = null;
        private string appName = "SAP Agent";

        public TestLicenseManagerUtils()
        {
            Console.Out.WriteLine(typeof(TestLicenseManagerUtils).FullName);
            try
            {
                license = System.ComponentModel.LicenseManager.Validate(typeof(TestLicenseManagerUtils), this);
                RuntimeApplicationLicense lic = (RuntimeApplicationLicense)license;
                Console.Out.WriteLine("Clientname: {0}", lic.ClientName);
                Console.Out.WriteLine("Expiredate: {0}", lic.ExpireDate);
                Console.Out.WriteLine("Licensecount: {0}", lic.LicenseCount);
                Console.Out.WriteLine("Servers: {0}", lic.ServerList.Length);
                Console.Out.WriteLine("Custom Attributes: {0}", lic.CustomAtts.Length);
            }
            catch
            {
                Console.Error.WriteLine("No License");
            }

            Console.In.ReadLine();
        }

        static void Main(string[] args)
        {
            TestLicenseManagerUtils testLicenseManagerUtils = new TestLicenseManagerUtils();
        }
    }
}
