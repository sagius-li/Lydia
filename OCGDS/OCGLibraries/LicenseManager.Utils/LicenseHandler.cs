using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;

namespace LicenseManager.Utils
{
    public class LicenseHandler
    {
        private string key = "m054890mwtruio895435489trec5489754mcrem894589m543cc4m45c89mthrem54c897547ehct43mhrecwnjm45c289m54c8mzc54c54289m54c28c542omrewm5475428n5428n4548905c2c5425c789425478c2n854324278998n54c98n754c28954274785254284528954c278n54c2097545c289547";

        private string _clientName;
        public string ClientName
        {
            get { return _clientName; }
        }

        private string _applicationName;
        public string ApplicationName
        {
            get { return _applicationName; }
        }

        private string[] _serverList;
        public string[] ServerList
        {
            get { return _serverList; }
        }

        private Int64 _licenses;
        public Int64 Licenses
        {
            get { return _licenses; }
        }

        private DateTime _expireDate;
        public DateTime ExpireDate
        {
            get { return _expireDate; }
        }

        private bool _checkServer;
        public bool CheckServer
        {
            get { return _checkServer; }
        }

        private string[] _customAtts;
        public string[] CustomAtts
        {
            get { return _customAtts; }
        }

        private bool _expired;
        public bool Expired
        {
            get { return _expired; }
        }

        public LicenseHandler()
            : this(string.Empty)
        {
        }

        public LicenseHandler(string encryptedLicense)
        {
            InitializeFields();
            if (encryptedLicense != string.Empty) SetLicensKey(encryptedLicense);
        }

        public void SetLicensKey(string licenseKey)
        {
            string encryptedLicense = Encryption.DecryptData(key, licenseKey);

            try
            {
                string[] licenseParts = encryptedLicense.Split('|');

                string version = GetLicenseVersion(licenseParts[0]);

                if ((version == "1.0") && (licenseParts.Length == 8))
                {
                    _clientName = GetClientName(licenseParts[1]);
                    _applicationName = GetApplicationName(licenseParts[2]);
                    _serverList = GetServerList(licenseParts[3]);
                    _licenses = GetLicenseCount(licenseParts[4]);
                    _expireDate = GetExpiredDate(licenseParts[5]);
                    _checkServer = GetServerCheck(licenseParts[6]);
                    _customAtts = GetCustomAtts(licenseParts[7]);
                    _expired = CheckExpired();
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw new Exception("Error while reading licens key");
            }
        }

        private string GetLicenseVersion(string version)
        {
            return version.Replace("[", "").Replace("]", "");
        }

        private bool GetServerCheck(string check)
        {
            string temp = check.Replace("[ServerCheck=", "").Replace("]", "");
            return !bool.Parse(temp);
        }

        private string[] GetCustomAtts(string attributes)
        {
            string temp = attributes.Replace("[", "").Replace("]", "");
            string[] retval = temp.Split("//".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
            return retval;
        }

        private string GetClientName(string clientName)
        {
            return clientName.Split('\\')[1].Replace("\\", "");
        }

        private string GetApplicationName(string appName)
        {
            return appName.Split('\\')[1].Replace("\\", "");
        }

        private string[] GetServerList(string serverList)
        {
            StringCollection sName = new StringCollection();
            string[] sNametemp = serverList.Split(new string[] { "[Name=" }, StringSplitOptions.None);
            for (int i = 1; i < sNametemp.Length; i++)
            {
                sName.Add(sNametemp[i].Replace("\\", "").Replace("]", "").ToLower());
            }

            string[] retval = new string[sName.Count];
            sName.CopyTo(retval, 0);

            return retval;
        }

        private Int64 GetLicenseCount(string licenseCount)
        {
            Int64 lic = 0;
            Int64.TryParse(licenseCount.Replace("[Licenses=", "").Replace("]", ""), out lic);

            return lic;
        }

        private DateTime GetExpiredDate(string expireDate)
        {
            DateTime expire = DateTime.ParseExact(expireDate.Replace("[Expire=", "").Replace("]", ""), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            return expire;
        }

        private string GetComputername()
        {
            return System.Windows.Forms.SystemInformation.ComputerName.ToString();
        }

        private bool CheckExpired()
        {
            bool checkexp = false;

            if (_checkServer)
            {
                foreach (string entry in _serverList)
                {
                    if (entry.ToLower().Equals(GetComputername().ToLower()))
                    {
                        checkexp = true;
                    }
                }
                if (!checkexp) return true;
            }

            if (DateTime.Now.CompareTo(_expireDate) < 0)
            {
                return false;
            }

            return true;
        }

        public bool CheckAppName(string appName)
        {
            if (_applicationName == appName)
            {
                return true;
            }

            return false;
        }

        private void InitializeFields()
        {
            _clientName = string.Empty;
            _applicationName = string.Empty;
            _expireDate = DateTime.MinValue;
            _serverList = new string[0];
            _checkServer = false;
            _customAtts = new string[0];
            _expired = true;
            _licenses = 0;
        }
    }
}