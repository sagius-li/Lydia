using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Xml.Schema;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Security.Principal;

using OCG.ResourceManagement.Client;
using OCG.ResourceManagement.ObjectModel;
using OCG.ResourceManagement.ObjectModel.ResourceTypes;

namespace OCG.ResourceManagement.Control
{
    public class ClientControl
    {
        #region Private Member

        private CredentialType _credentialType;
        private FIMServiceClient _defaultClient = null;

        #endregion

        #region CredentialType

        public enum CredentialType
        {
            None = 0, 
            AdminCredential, 
            UserCredential, 
            ServiceCredential
        }

        #endregion

        #region Public Member

        public CredentialType ClientCredentialType
        {
            get { return _credentialType; }
        }

        public FIMServiceClient Client
        {
            get { return _defaultClient; }
        }

        public static ErrorControl ErrorControl { get; set; }

        public bool SchemaCached
        {
            get
            {
                if (_defaultClient != null)
                {
                    return _defaultClient.SchemaCached;
                }

                return false;
            }
        }

        #endregion

        #region Constructor

        public ClientControl(CredentialType credentialType, ErrorControl errorControl)
        {
            _credentialType = credentialType;
            ErrorControl = errorControl;

            switch (credentialType)
            {
                case CredentialType.None:
                    break;
                case CredentialType.AdminCredential:
                    break;
                case CredentialType.UserCredential:
                    _defaultClient = new FIMServiceClient() { ClientCredential = CredentialCache.DefaultCredentials as NetworkCredential };
                    break;
                case CredentialType.ServiceCredential:
                    break;
                default:
                    break;
            }
        }

        public ClientControl(string userName, string password, string domain, ErrorControl errorControl)
        {
            _credentialType = CredentialType.AdminCredential;
            ErrorControl = errorControl;

            _defaultClient = new FIMServiceClient() { ClientCredential = new NetworkCredential(userName, password, domain) };
        }

        public ClientControl(
            string userName, string password, string domain, ErrorControl errorControl, 
            EndpointAddress mexEndpoint, EndpointAddress resourceEndpoint, EndpointAddress resourceFactoryEndpoint, EndpointAddress enumerationEndpoint)
        {
            _credentialType = CredentialType.AdminCredential;
            ErrorControl = errorControl;

            _defaultClient = new FIMServiceClient(mexEndpoint, resourceEndpoint, resourceFactoryEndpoint, enumerationEndpoint) 
            { 
                ClientCredential = new NetworkCredential(userName, password, domain) 
            };
        }

        public ClientControl(
            ErrorControl errorControl,
            EndpointAddress mexEndpoint, EndpointAddress resourceEndpoint, EndpointAddress resourceFactoryEndpoint, EndpointAddress enumerationEndpoint)
        {
            _credentialType = CredentialType.AdminCredential;
            ErrorControl = errorControl;

            _defaultClient = new FIMServiceClient(mexEndpoint, resourceEndpoint, resourceFactoryEndpoint, enumerationEndpoint)
            {
                ClientCredential = CredentialCache.DefaultCredentials as NetworkCredential
            };
        }

        #endregion

        #region WsTransfer

        public RmResource Get(RmReference objectId)
        {
            try
            {
                return _defaultClient.Get(objectId);
            }
            catch (Exception e)
            {
                HandleError(e);

                return null;
            }
        }

        public RmResource Get(RmReference objectId, CultureInfo culture)
        {
            try
            {
                return _defaultClient.Get(objectId, culture);
            }
            catch (Exception e)
            {
                HandleError(e);

                return null;
            }
        }

        public RmResource Get(RmReference objectId, string[] attributes)
        {
            try
            {
                return _defaultClient.Get(objectId, attributes);
            }
            catch (Exception e)
            {
                HandleError(e);

                return null;
            }
        }

        public RmResource Get(RmReference objectId, CultureInfo culture, string[] attributes)
        {
            try
            {
                return _defaultClient.Get(objectId, culture, attributes);
            }
            catch (Exception e)
            {
                HandleError(e);

                return null;
            }
        }

        public bool Put(RmResourceChanges transaction)
        {
            try
            {
                return _defaultClient.Put(transaction);
            }
            catch (Exception e)
            {
                HandleError(e);

                return false;
            }
        }

        public RmReference Create(RmResource newResource)
        {

            try
            {
                return _defaultClient.Create(newResource);
            }
            catch (Exception e)
            {
                HandleError(e);

                return null;
            }
            
        }

        public bool Delete(RmReference objectId)
        {
            try
            {
                return _defaultClient.Delete(objectId);
            }
            catch (Exception e)
            {
                HandleError(e);

                return false;
            }
        }

        #endregion

        #region Approval

        public void Approve(RmApproval approval, bool isApproved)
        {
            //try
            //{
            //    _defaultClient.Approve(approval, isApproved);
            //}
            //catch (Exception e)
            //{
            //    HandleError(e);
            //}

            _defaultClient.Approve(approval, isApproved);
        }

        public void Approve(RmApproval approval, bool isApproved, EndpointAddress address)
        {
            //try
            //{
            //    _defaultClient.Approve(approval, isApproved, address);
            //}
            //catch (Exception e)
            //{
            //    HandleError(e);
            //}

            _defaultClient.Approve(approval, isApproved, address);
        }

        #endregion

        #region Enumeration

        public IEnumerable<RmResource> Enumerate(string filter)
        {
            return _defaultClient.Enumerate(filter);
        }

        public IEnumerable<RmResource> Enumerate(string filter, string[] attributes)
        {
            return _defaultClient.Enumerate(filter, attributes);
        }

        #endregion

        #region Public Methodes

        public XmlSchemaSet RefreshSchema()
        {
            return _defaultClient.RefreshSchema();
        }

        public void Dispose()
        {
            _defaultClient.Dispose();
        }

        public WindowsIdentity GetCurrentUser()
        {
            return WindowsIdentity.GetCurrent();
        }

        #endregion

        private void HandleError(Exception e)
        {
            string faultType = GetExceptionFaultType(e);

            if (faultType.Equals(typeof(Faults.PermissionDeniedFault).ToString()))
            {
                ErrorControl.AddError(new ErrorData(@"Permission denied", faultType));
            }
            else if (faultType.Equals(typeof(Faults.AuthenticationRequiredFault).ToString()))
            {
                ErrorControl.AddError(new ErrorData(@"Authentication failed", faultType));
            }
            else if (faultType.Equals(typeof(Faults.AuthorizationRequiredFault).ToString()))
            {
                ErrorControl.AddError(new ErrorData(@"Operation need to be authorized", faultType));
            }
            else
            {
                ErrorControl.AddError(new ErrorData(@"Unknown error", faultType));
            }
        }

        private string GetExceptionFaultType(Exception e)
        {
            string fullName = e.GetType().ToString();
            int begin = fullName.IndexOf('[');
            int end = fullName.IndexOf(']');
            if (begin < end)
            {
                return fullName.Substring(begin + 1, end - begin - 1);
            }

            return string.Empty;
        }
    }
}
