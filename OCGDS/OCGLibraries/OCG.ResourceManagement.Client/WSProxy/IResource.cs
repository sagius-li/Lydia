using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Net.Security;

namespace OCG.ResourceManagement.Client.WSProxy
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute(Constants.ServiceModel.AssemblyName, Constants.ServiceModel.AssemblyVersion)]
    [System.ServiceModel.ServiceContractAttribute(Namespace = Constants.WsTransfer.Namespace, ConfigurationName = Constants.WsTransfer.Resource)]
    public interface IResource
    {
        [System.ServiceModel.OperationContractAttribute(ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign,
            Action = Constants.WsTransfer.GetAction, ReplyAction = Constants.WsTransfer.GetResponseAction)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.AuthenticationRequiredFault), Action = Constants.Rm.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Rm.AuthenticationRequiredFault, Namespace = Constants.Rm.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.AuthorizationRequiredFault), Action = Constants.Rm.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Rm.AuthorizationRequiredFault, Namespace = Constants.Rm.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.CannotProcessFilter), Action = Constants.Wsman.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Wsman.CannotProcessFilter, Namespace = Constants.Rm.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.CannotProcessFilter), Action = Constants.Wsman.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Wsman.DataRequiredFault, Namespace = Constants.Rm.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.PermissionDeniedFault), Action = Constants.Rm.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Rm.PermissionDeniedFault, Namespace = Constants.Rm.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.EndpointUnavailable), Action = Constants.Addressing.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Addressing.EndpointUnavailable, Namespace = Constants.Addressing.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.UnwillingToPerformFault), Action = Constants.Imda.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Imda.UnwillingToPerform, Namespace = Constants.Imda.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.FragmentDialectNotSupported), Action = Constants.Wsman.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Wsman.FragmentDialectNotSupported, Namespace = Constants.Wsman.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.InvalidRepresentationFault), Action = Constants.WsTransfer.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Rm.InvalidRepresentation, Namespace = Constants.WsTransfer.Namespace)]
        Message Get(Message request);

        [System.ServiceModel.OperationContractAttribute(ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign, 
            Action = Constants.WsTransfer.PutAction, ReplyAction = Constants.WsTransfer.PutResponseAction)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.AuthenticationRequiredFault), Action = Constants.Rm.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Rm.AuthenticationRequiredFault, Namespace = Constants.Rm.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.AuthorizationRequiredFault), Action = Constants.Rm.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Rm.AuthorizationRequiredFault, Namespace = Constants.Rm.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.CannotProcessFilter), Action = Constants.Wsman.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Wsman.CannotProcessFilter, Namespace = Constants.Rm.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.CannotProcessFilter), Action = Constants.Wsman.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Wsman.DataRequiredFault, Namespace = Constants.Rm.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.PermissionDeniedFault), Action = Constants.Rm.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Rm.PermissionDeniedFault, Namespace = Constants.Rm.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.EndpointUnavailable), Action = Constants.Addressing.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Addressing.EndpointUnavailable, Namespace = Constants.Addressing.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.UnwillingToPerformFault), Action = Constants.Imda.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Imda.UnwillingToPerform, Namespace = Constants.Imda.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.FragmentDialectNotSupported), Action = Constants.Wsman.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Wsman.FragmentDialectNotSupported, Namespace = Constants.Wsman.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.InvalidRepresentationFault), Action = Constants.WsTransfer.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Rm.InvalidRepresentation, Namespace = Constants.WsTransfer.Namespace)]
        Message Put(Message request);

        [System.ServiceModel.OperationContractAttribute(ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign, 
            Action = Constants.WsTransfer.DeleteAction, ReplyAction = Constants.WsTransfer.DeleteResponseAction)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.AuthenticationRequiredFault), Action = Constants.Rm.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Rm.AuthenticationRequiredFault, Namespace = Constants.Rm.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.AuthorizationRequiredFault), Action = Constants.Rm.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Rm.AuthorizationRequiredFault, Namespace = Constants.Rm.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.CannotProcessFilter), Action = Constants.Wsman.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Wsman.CannotProcessFilter, Namespace = Constants.Rm.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.CannotProcessFilter), Action = Constants.Wsman.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Wsman.DataRequiredFault, Namespace = Constants.Rm.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.PermissionDeniedFault), Action = Constants.Rm.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Rm.PermissionDeniedFault, Namespace = Constants.Rm.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.EndpointUnavailable), Action = Constants.Addressing.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Addressing.EndpointUnavailable, Namespace = Constants.Addressing.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.UnwillingToPerformFault), Action = Constants.Imda.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Imda.UnwillingToPerform, Namespace = Constants.Imda.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.FragmentDialectNotSupported), Action = Constants.Wsman.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Wsman.FragmentDialectNotSupported, Namespace = Constants.Wsman.Namespace)]
        [System.ServiceModel.FaultContractAttribute(typeof(Faults.InvalidRepresentationFault), Action = Constants.WsTransfer.Fault, 
            ProtectionLevel = ProtectionLevel.EncryptAndSign, Name = Constants.Rm.InvalidRepresentation, Namespace = Constants.WsTransfer.Namespace)]
        Message Delete(Message request);
    }
}
