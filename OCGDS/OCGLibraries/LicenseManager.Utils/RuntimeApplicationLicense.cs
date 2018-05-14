using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace LicenseManager.Utils
{
    public class RuntimeApplicationLicense : License
    {
        private Type _type;

        private string _clientName;
        public string ClientName
        {
            get { return _clientName; }
        }

        private string[] _serverList;
        public string[] ServerList
        {
            get { return _serverList; }
        }

        private Int64 _licenses;
        public Int64 LicenseCount
        {
            get { return _licenses; }
        }

        private DateTime _expireDate;
        public DateTime ExpireDate
        {
            get { return _expireDate; }
        }

        private string[] _customAtts;
        public string[] CustomAtts
        {
            get { return _customAtts; }
        }

        internal RuntimeApplicationLicense(Type type, LicenseHandler licHandler)
        {
            InitializeFields();

            if (type == null)
            {
                throw new NullReferenceException("The licensed type reference may not be null.");
            }
            _type = type;

            if (licHandler == null)
            {
                throw new NullReferenceException("The licensehandler reference may not be null.");
            }

            _clientName = licHandler.ClientName;
            _customAtts = licHandler.CustomAtts;
            _expireDate = licHandler.ExpireDate;
            _serverList = licHandler.ServerList;
            _licenses = licHandler.Licenses;
        }

        public RuntimeApplicationLicense(Type type)
        {
            InitializeFields();

            if (type == null)
            {
                throw new NullReferenceException("The licensed type reference may not be null.");
            }
            _type = type;
        }

        public override string LicenseKey
        {
            get { return Guid.NewGuid().ToString(); }
        }

        public override void Dispose()
        {
        }

        private void InitializeFields()
        {
            _clientName = string.Empty;
            _expireDate = DateTime.MinValue;
            _serverList = new string[0];
            _customAtts = new string[0];
            _licenses = 0;
        }
    }
}
