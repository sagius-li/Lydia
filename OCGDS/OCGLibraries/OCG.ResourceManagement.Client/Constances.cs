using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.ServiceModel.Channels;

namespace OCG.ResourceManagement.Client
{
    internal class Constants
    {
        private Constants() { }

        public class Bindings
        {
            public WSHttpBinding MetadataExchangeHttpBinding_IMetadataExchange = new WSHttpBinding(System.ServiceModel.SecurityMode.None, false)
            {
                Name = "MetadataExchangeHttpBinding_IMetadataExchange",
                CloseTimeout = new System.TimeSpan(0, 1, 0),
                OpenTimeout = new System.TimeSpan(0, 1, 0),
                ReceiveTimeout = new System.TimeSpan(0, 10, 0),
                SendTimeout = new System.TimeSpan(0, 1, 0),
                BypassProxyOnLocal = false,
                TransactionFlow = false,
                HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard,
                MaxBufferPoolSize = 524288,
                MaxReceivedMessageSize = 965536,
                MessageEncoding = System.ServiceModel.WSMessageEncoding.Text,
                TextEncoding = System.Text.Encoding.UTF8,
                UseDefaultWebProxy = true,
                AllowCookies = false,
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                {
                    MaxDepth = 32,
                    MaxStringContentLength = 8192,
                    MaxArrayLength = 16384,
                    MaxBytesPerRead = 4096,
                    MaxNameTableCharCount = 16384
                }
            };

            public WSHttpContextBinding ServiceMultipleTokenBinding_Resource = new WSHttpContextBinding(System.ServiceModel.SecurityMode.Message, false)
            {
                Name = "ServiceMultipleTokenBinding_Resource",
                CloseTimeout = new System.TimeSpan(0, 1, 0),
                OpenTimeout = new System.TimeSpan(0, 1, 0),
                ReceiveTimeout = new System.TimeSpan(0, 10, 0),
                SendTimeout = new System.TimeSpan(0, 1, 0),
                BypassProxyOnLocal = false,
                TransactionFlow = false,
                HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard,
                MaxBufferPoolSize = 524288,
                MaxReceivedMessageSize = 65536,
                MessageEncoding = System.ServiceModel.WSMessageEncoding.Text,
                TextEncoding = System.Text.Encoding.UTF8,
                UseDefaultWebProxy = true,
                AllowCookies = false,
                ContextProtectionLevel = System.Net.Security.ProtectionLevel.Sign,
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                {
                    MaxDepth = 32,
                    MaxStringContentLength = 8192,
                    MaxArrayLength = 16384,
                    MaxBytesPerRead = 4096,
                    MaxNameTableCharCount = 16384
                },
            };

            public WSHttpContextBinding ServiceMultipleTokenBinding_ResourceFactory = new WSHttpContextBinding(System.ServiceModel.SecurityMode.Message, false)
            {
                Name = "ServiceMultipleTokenBinding_ResourceFactory",
                CloseTimeout = new System.TimeSpan(0, 1, 0),
                OpenTimeout = new System.TimeSpan(0, 1, 0),
                ReceiveTimeout = new System.TimeSpan(0, 10, 0),
                SendTimeout = new System.TimeSpan(0, 1, 0),
                BypassProxyOnLocal = false,
                TransactionFlow = false,
                HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard,
                MaxBufferPoolSize = 524288,
                MaxReceivedMessageSize = 65536,
                MessageEncoding = System.ServiceModel.WSMessageEncoding.Text,
                TextEncoding = System.Text.Encoding.UTF8,
                UseDefaultWebProxy = true,
                AllowCookies = false,
                ContextProtectionLevel = System.Net.Security.ProtectionLevel.Sign,
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                {
                    MaxDepth = 32,
                    MaxStringContentLength = 8192,
                    MaxArrayLength = 16384,
                    MaxBytesPerRead = 4096,
                    MaxNameTableCharCount = 16384
                }
            };

            public WSHttpContextBinding ServiceMultipleTokenBinding_Enumeration = new WSHttpContextBinding(System.ServiceModel.SecurityMode.Message, false)
            {
                Name = "ServiceMultipleTokenBinding_Enumeration",
                CloseTimeout = new System.TimeSpan(0, 1, 0),
                OpenTimeout = new System.TimeSpan(0, 1, 0),
                ReceiveTimeout = new System.TimeSpan(0, 10, 0),
                SendTimeout = new System.TimeSpan(0, 1, 0),
                BypassProxyOnLocal = false,
                TransactionFlow = false,
                HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard,
                MaxBufferPoolSize = 524288,
                MaxReceivedMessageSize = 2147483647,
                MessageEncoding = System.ServiceModel.WSMessageEncoding.Text,
                TextEncoding = System.Text.Encoding.UTF8,
                UseDefaultWebProxy = true,
                AllowCookies = false,
                ContextProtectionLevel = System.Net.Security.ProtectionLevel.Sign,
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                {
                    MaxDepth = 32,
                    MaxStringContentLength = 8192,
                    MaxArrayLength = 16384,
                    MaxBytesPerRead = 4096,
                    MaxNameTableCharCount = 16384
                }
            };

            public WSHttpContextBinding ServiceMultipleTokenBinding_Alternate = new WSHttpContextBinding(System.ServiceModel.SecurityMode.Message, false)
            {
                Name = "ServiceMultipleTokenBinding_Alternate",
                CloseTimeout = new System.TimeSpan(0, 1, 0),
                OpenTimeout = new System.TimeSpan(0, 1, 0),
                ReceiveTimeout = new System.TimeSpan(0, 10, 0),
                SendTimeout = new System.TimeSpan(0, 1, 0),
                BypassProxyOnLocal = false,
                TransactionFlow = false,
                HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard,
                MaxBufferPoolSize = 524288,
                MaxReceivedMessageSize = 65536,
                MessageEncoding = System.ServiceModel.WSMessageEncoding.Text,
                TextEncoding = System.Text.Encoding.UTF8,
                UseDefaultWebProxy = true,
                AllowCookies = false,
                ContextProtectionLevel = System.Net.Security.ProtectionLevel.Sign,
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                {
                    MaxDepth = 32,
                    MaxStringContentLength = 8192,
                    MaxArrayLength = 16384,
                    MaxBytesPerRead = 4096,
                    MaxNameTableCharCount = 16384
                }
            };

            public WSHttpContextBinding ServiceMultipleTokenBinding_SecurityTokenService = new WSHttpContextBinding(System.ServiceModel.SecurityMode.Message, false)
            {
                Name = "ServiceMultipleTokenBinding_SecurityTokenService",
                CloseTimeout = new System.TimeSpan(0, 1, 0),
                OpenTimeout = new System.TimeSpan(0, 1, 0),
                ReceiveTimeout = new System.TimeSpan(0, 10, 0),
                SendTimeout = new System.TimeSpan(0, 1, 0),
                BypassProxyOnLocal = false,
                TransactionFlow = false,
                HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard,
                MaxBufferPoolSize = 524288,
                MaxReceivedMessageSize = 65536,
                MessageEncoding = System.ServiceModel.WSMessageEncoding.Text,
                TextEncoding = System.Text.Encoding.UTF8,
                UseDefaultWebProxy = true,
                AllowCookies = false,
                ContextProtectionLevel = System.Net.Security.ProtectionLevel.Sign,
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                {
                    MaxDepth = 32,
                    MaxStringContentLength = 8192,
                    MaxArrayLength = 16384,
                    MaxBytesPerRead = 4096,
                    MaxNameTableCharCount = 16384
                }
            };

            public Bindings()
            {
                MetadataExchangeHttpBinding_IMetadataExchange.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
                MetadataExchangeHttpBinding_IMetadataExchange.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                MetadataExchangeHttpBinding_IMetadataExchange.Security.Transport.Realm = string.Empty;
                MetadataExchangeHttpBinding_IMetadataExchange.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
                MetadataExchangeHttpBinding_IMetadataExchange.Security.Message.NegotiateServiceCredential = true;
                MetadataExchangeHttpBinding_IMetadataExchange.Security.Message.EstablishSecurityContext = true;

                ServiceMultipleTokenBinding_Resource.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
                ServiceMultipleTokenBinding_Resource.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                ServiceMultipleTokenBinding_Resource.Security.Transport.Realm = string.Empty;
                ServiceMultipleTokenBinding_Resource.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
                ServiceMultipleTokenBinding_Resource.Security.Message.NegotiateServiceCredential = true;
                ServiceMultipleTokenBinding_Resource.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;
                ServiceMultipleTokenBinding_Resource.Security.Message.EstablishSecurityContext = false;

                ServiceMultipleTokenBinding_ResourceFactory.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
                ServiceMultipleTokenBinding_ResourceFactory.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                ServiceMultipleTokenBinding_ResourceFactory.Security.Transport.Realm = string.Empty;
                ServiceMultipleTokenBinding_ResourceFactory.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
                ServiceMultipleTokenBinding_ResourceFactory.Security.Message.NegotiateServiceCredential = true;
                ServiceMultipleTokenBinding_ResourceFactory.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;
                ServiceMultipleTokenBinding_ResourceFactory.Security.Message.EstablishSecurityContext = false;

                ServiceMultipleTokenBinding_Alternate.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
                ServiceMultipleTokenBinding_Alternate.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                ServiceMultipleTokenBinding_Alternate.Security.Transport.Realm = string.Empty;
                ServiceMultipleTokenBinding_Alternate.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
                ServiceMultipleTokenBinding_Alternate.Security.Message.NegotiateServiceCredential = true;
                ServiceMultipleTokenBinding_Alternate.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;
                ServiceMultipleTokenBinding_Alternate.Security.Message.EstablishSecurityContext = false;

                ServiceMultipleTokenBinding_Enumeration.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
                ServiceMultipleTokenBinding_Enumeration.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                ServiceMultipleTokenBinding_Enumeration.Security.Transport.Realm = string.Empty;
                ServiceMultipleTokenBinding_Enumeration.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
                ServiceMultipleTokenBinding_Enumeration.Security.Message.NegotiateServiceCredential = true;
                ServiceMultipleTokenBinding_Enumeration.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;
                ServiceMultipleTokenBinding_Enumeration.Security.Message.EstablishSecurityContext = false;

                ServiceMultipleTokenBinding_SecurityTokenService.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
                ServiceMultipleTokenBinding_SecurityTokenService.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                ServiceMultipleTokenBinding_SecurityTokenService.Security.Transport.Realm = string.Empty;
                ServiceMultipleTokenBinding_SecurityTokenService.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
                ServiceMultipleTokenBinding_SecurityTokenService.Security.Message.NegotiateServiceCredential = true;
                ServiceMultipleTokenBinding_SecurityTokenService.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;
                ServiceMultipleTokenBinding_SecurityTokenService.Security.Message.EstablishSecurityContext = false;
            }
        }

        public class EndpointAddresses
        {
            public EndpointAddress MetadataExchangeHttpBinding_IMetadataExchange = new EndpointAddress(@"http://localhost:5725/ResourceManagementService/MEX");

            public EndpointAddress ServiceMultipleTokenBinding_Resource = new EndpointAddress(@"http://localhost:5725/ResourceManagementService/Resource");
            public EndpointAddress ServiceMultipleTokenBinding_ResourceFactory = new EndpointAddress(@"http://localhost:5725/ResourceManagementService/ResourceFactory");
            public EndpointAddress ServiceMultipleTokenBinding_Enumeration = new EndpointAddress(@"http://localhost:5725/ResourceManagementService/Enumeration");
            public EndpointAddress ServiceMultipleTokenBinding_Alternate = new EndpointAddress(@"http://localhost:5725/ResourceManagementService/Alternate");
            public EndpointAddress ServiceMultipleTokenBinding_SecurityTokenService = new EndpointAddress(@"http://localhost:5726/ResourceManagementService/SecurityTokenService/Registration");
        }

        internal class ServiceModel
        {
            public const string AssemblyName = "System.ServiceModel";
            public const string AssemblyVersion = "3.0.0.0";
        }

        internal class Endpoint
        {
            private Endpoint() { }
            public const String Resource = "ResourceManagementService/Resource";
            public const String ResourceFactory = "ResourceManagementService/ResourceFactory";
            public const String Enumeration = "ResourceManagementService/Enumeration";
            public const String Alternate = "ResourceManagementService/Alternate";
        }

        internal class Addressing
        {
            private Addressing() { }
            public const String Namespace = "http://schemas.xmlsoap.org/ws/2004/08/addressing";
            public const String Prefix = "wsa";
            public const String Fault = "http://www.w3.org/2005/08/addressing/fault";

            public const String EndpointUnavailable = "EndpointUnavailable";
            public const String ReferenceProperties = "ReferenceProperties";
            public const String EndpointAddress = "EndpointAddress";
            public const String Address = "Address";
        }

        internal class Soap
        {
            private Soap() { }
            public const String Namespace = "http://www.w3.org/2003/05/soap-envelope";
            public const String Prefix = "s";

        }

        internal class Xsi
        {
            private Xsi() { }
            public const String Nil = "nil";
            public const String Namespace = "http://www.w3.org/2001/XMLSchema-instance";
            public const String Prefix = "xsi";
        }

        internal class Xsd
        {
            private Xsd() { }

            public const String Namespace = "http://www.w3.org/2001/XMLSchema";
            public const String Prefix = "xsd";
        }

        internal class Dialect
        {
            private Dialect() { }

            public const String IdmXpathFilter = "http://schemas.microsoft.com/2006/11/XPathFilterDialect";
            public const String IdmAttributeType = "http://schemas.microsoft.com/2006/11/ResourceManagement/Dialect/IdentityAttributeType-20080602";
        }

        internal class WsTransfer
        {
            private WsTransfer() { }

            public const String Namespace = "http://scheams.xmlsoap.org/ws/2004/09/transfer";
            public const String Prefix = "wxf";
            public const String Fault = "http://schemas.xmlsoap.org/ws/2004/09/transfer/fault";

            public const String CreateAction = "http://schemas.xmlsoap.org/ws/2004/09/transfer/Create";
            public const String CreateResponseAction = "http://schemas.xmlsoap.org/ws/2004/09/transfer/CreateResponse";
            public const String DeleteAction = "http://schemas.xmlsoap.org/ws/2004/09/transfer/Delete";
            public const String DeleteResponseAction = "http://schemas.xmlsoap.org/ws/2004/09/transfer/DeleteResponse";
            public const String GetAction = "http://schemas.xmlsoap.org/ws/2004/09/transfer/Get";
            public const String GetResponseAction = "http://schemas.xmlsoap.org/ws/2004/09/transfer/GetResponse";
            public const String PutAction = "http://schemas.xmlsoap.org/ws/2004/09/transfer/Put";
            public const String PutResponseAction = "http://schemas.xmlsoap.org/ws/2004/09/transfer/PutResponse";


            public const String Alternate = "Alternate";
            public const String Resource = "Resource";
            public const String ResourceFactory = "ResourceFactory";
            public const String IMEX = "IMEX";
            public const String ResourceCreated = "ResourceCreated";

        }

        internal class WsEnumeration
        {
            private WsEnumeration() { }

            public const String Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration";
            public const String Prefix = "wsen";
            public const String Fault = "http://schemas.xmlsoap.org/ws/2004/09/enumeration/fault";

            public const String EnumerateAction = "http://schemas.xmlsoap.org/ws/2004/09/enumeration/Enumerate";
            public const String EnumerateResponseAction = "http://schemas.xmlsoap.org/ws/2004/09/enumeration/EnumerateResponse";
            public const String GetStatusAction = "http://schemas.xmlsoap.org/ws/2004/09/enumeration/GetStatus";
            public const String GetStatusResponseAction = "http://schemas.xmlsoap.org/ws/2004/09/enumeration/GetStatusResponse";
            public const String PullAction = "http://schemas.xmlsoap.org/ws/2004/09/enumeration/Pull";
            public const String PullResponseAction = "http://schemas.xmlsoap.org/ws/2004/09/enumeration/PullResponse";
            public const String ReleaseAction = "http://schemas.xmlsoap.org/ws/2004/09/enumeration/Release";
            public const String ReleaseResponseAction = "http://schemas.xmlsoap.org/ws/2004/09/enumeration/ReleaseResponse";
            public const String RenewAction = "http://schemas.xmlsoap.org/ws/2004/09/enumeration/Renew";
            public const String RenewResponseAction = "http://schemas.xmlsoap.org/ws/2004/09/enumeration/RenewResponse";

            public const String Enumerate = "Enumerate";
            public const String Pull = "Pull";
            public const String Filter = "Filter";

            public const String EnumerationResponse = "EnumerateResponse";
            public const String PullResponse = "PullResponse";

            public const Int32 DefaultMaxCharacters = 3668672;
            public const Int32 DefaultMaxElements = 20;
        }

        internal class Imda
        {
            private Imda() { }

            public const String Namespace = "http://schemas.microsoft.com/2006/11/IdentityManagement/DirectoryAccess";
            public const String Prefix = "ida";
            public const String Fault = "http://schemas.microsoft.com/2006/11/IdentityManagement/DirectoryAccess/fault";

            public const String ExtensionHeaderName = "IdentityManagementOperation";

            public const String BaseObjectSearchRequest = "BaseObjectSearchRequest";
            public const String BaseObjectSearchResponse = "BaseObjectSearchResponse";
            public const String Dialect = "Dialect";
            public const string AttributeType = "AttributeType";
            public const String Operation = "Operation";
            public const String Change = "Change";
            public const String AttributeTypeAndValue = "AttributeTypeAndValue";
            public const String AttributeTypes = "AttributeTypes";
            public const String ModifyRequest = "ModifyRequest";
            public const String AddRequest = "AddRequest";
            public const String PartialAttribute = "PartialAttribute";

            public const String UnwillingToPerform = "UnwillingToPerform";
        }
        internal class Rm
        {
            private Rm() { }

            public const String Namespace = "http://schemas.microsoft.com/2006/11/ResourceManagement";
            public const String Prefix = "rm";
            public const String Fault = "http://schemas.microsoft.com/2006/11/ResourceManagement/fault";

            public const String PermissionHints = "permissions";
            public const String ResourceReferenceProperty = "ResourceReferenceProperty";
            public const String ResourceTimeProperty = "Time";
            public const String ResourceLocaleProperty = "Locale";
            public const String InvalidRepresentation = "InvalidRepresentation";

            public const String AuthorizationRequiredFault = "AuthorizationRequiredFault";
            public const String PermissionDeniedFault = "PermissionDeniedFault";
            public const String AuthenticationRequiredFault = "AuthenticationRequiredFault";

        }

        internal class Wsman
        {
            private Wsman() { }

            public const String Namespace = "http://schemas.dmtf.org/wbem/wsman/1/wsman.xsd";
            public const String Prefix = "wsman";
            public const String Fault = "http://schemas.dmtf.org/wbem/wsman/1/wsman/fault";

            public const String DataRequiredFault = "DataRequiredFault";
            public const String CannotProcessFilter = "CannotProcessFilter";
            public const String FragmentDialectNotSupported = "FragmentDialectNotSupported";
        }

        internal class WsTrust
        {
            private WsTrust() { }

            public const String Namespace = "http://schemas.xmlsoap.org/ws/2005/02/trust";
            public const String Prefix = "wst";
            public const String Fault = "http://schemas.xmlsoap.org/ws/2005/02/trust";

            public const String SecurityTokenService = "SecurityTokenService";
            public const String RequestSecurityToken = "RequestSecurityToken";
            public const String RequestSecurityTokenResponse = "RequestSecurityTokenResponse";
            public const String RequestFailed = "RequestFailed";

            public const String RequestSecurityTokenIssueAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue";
            public const String RequestSecurityTokenResponseIssueAction = "http://schemas.xmlsoap.org/ws/2005/02/trust/RSTR/Issue";
        }

        internal class WsPolicy
        {
            private WsPolicy() { }

            public const String Namespace = "http://schemas.xmlsoap.org/ws/2004/09/policy";
            public const String Prefix = "wsp";
            public const String Fault = "http://schemas.xmlsoap.org/ws/2004/09/policy/fault";
        }
    }
}
