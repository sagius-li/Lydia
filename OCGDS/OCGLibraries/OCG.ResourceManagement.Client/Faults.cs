using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace OCG.ResourceManagement.Client
{
    public class Faults
    {
        [XmlRoot(Namespace = Constants.Rm.Namespace)]
        [DataContract(Namespace = Constants.Rm.Namespace)]
        public class AuthenticationRequiredFault
        {
            [XmlElement()]
            [DataMember()]
            public string SecurityTokenServiceAddress;

            [XmlElement()]
            [DataMember()]
            public bool? UserRegistered;

            [XmlElement()]
            [DataMember()]
            public bool? UserLockedOut;

            [XmlElement()]
            [DataMember()]
            public String ContextIdentifier;
        }

        [DataContract(Namespace = Constants.Rm.Namespace)]
        [XmlRoot(Namespace = Constants.Rm.Namespace)]
        public class AuthorizationRequiredFault
        {
            public AuthorizationRequiredFault()
            {

            }
            [XmlElement(Namespace = Constants.Addressing.Namespace)]
            [DataMember()]
            public string EndpointReference;
        }

        [DataContract(Namespace = Constants.WsEnumeration.Namespace)]
        public class CannotProcessFilter
        {
        }

        [DataContract(Namespace = Constants.Rm.Namespace)]
        public class EndpointUnavailable
        {
            public EndpointUnavailable()
            {

            }
        }

        [DataContract(Namespace = Constants.WsEnumeration.Namespace)]
        public class FilterDialectRequestedUnavailable
        {
        }

        [DataContract(Namespace = Constants.Rm.Namespace)]
        public class FragmentDialectNotSupported
        {
            public FragmentDialectNotSupported()
            {

            }
        }

        [DataContract(Namespace = Constants.Rm.Namespace)]
        public class InvalidEnumerationContext
        {
        }

        [DataContract(Namespace = Constants.Rm.Namespace)]
        public class InvalidRepresentationFault
        {
            public InvalidRepresentationFault()
            {

            }
        }

        [DataContract(Namespace = Constants.Rm.Namespace)]
        public class PermissionDeniedFault
        {

            public PermissionDeniedFault()
            {

            }

            [XmlElement(Namespace = Constants.Addressing.Namespace)]
            [DataMember()]
            public string EndpointReference;
        }

        [DataContract(Namespace = Constants.WsTrust.Namespace)]
        public class RequestFailed
        {

        }

        [DataContract(Namespace = Constants.Rm.Namespace)]
        public class UnsupportedExpiration
        {
        }

        [DataContract(Namespace = Constants.Imda.Namespace)]
        public class UnwillingToPerformFault
        {
            public UnwillingToPerformFault()
            {

            }
        }
    }
}
