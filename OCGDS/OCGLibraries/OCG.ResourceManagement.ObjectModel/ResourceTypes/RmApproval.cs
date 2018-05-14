using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel.ResourceTypes
{
    public class RmApproval : RmResource
    {
        /// <summary>
        /// The type of the wrapped resource.
        /// </summary>
        protected const String ResourceType = @"Approval";

        /// <summary>
        /// Gets the FIM name of the wrapped resource type.
        /// </summary>
        /// <returns>The FIM name of the wrapped resource type.</returns>
        public override string GetResourceType()
        {
            return ResourceType;
        }

        public static string StaticResourceType()
        {
            return RmApproval.ResourceType;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RmApproval()
            : base()
        {
        }

        ///// <summary>
        ///// Constructor for serialization.
        ///// </summary>
        //protected RmApproval(
        //    SerializationInfo info,
        //    StreamingContext context)
        //    : base(info, context)
        //{
        //}

        #region promoted properties

        /// <summary>
        /// Approval Duration
        /// ApprovalDuration
        /// </summary>
        public DateTime? ApprovalDuration
        {
            get
            {
                return GetDateTime(AttributeNames.ApprovalDuration);
            }
            set
            {
                base[AttributeNames.ApprovalDuration].Value = value;
            }
        }

        RmList<RmReference> _approvalResponse;

        /// <summary>
        /// Approval Response
        /// This is a reference type to ApprovalResponse resource
        /// </summary>
        public IList<RmReference> ApprovalResponse
        {
            get
            {
                if (this._approvalResponse == null)
                {
                    lock (base.attributes)
                    {
                        this._approvalResponse = GetMultiValuedReference(AttributeNames.ApprovalResponse);
                        return this._approvalResponse;
                    }
                }
                else
                {
                    return this._approvalResponse;
                }
            }
        }

        /// <summary>
        /// Approval Status
        /// ApprovalStatus
        /// </summary>
        public string ApprovalStatus
        {
            get { return GetString(AttributeNames.ApprovalStatus); }
            set { base[AttributeNames.ApprovalStatus].Value = value; }
        }

        /// <summary>
        /// Approval Threshold
        /// ApprovalThreshold
        /// </summary>
        public int? ApprovalThreshold
        {
            get
            {
                return GetInteger(AttributeNames.ApprovalThreshold);
            }
            set
            {
                base[AttributeNames.ApprovalThreshold].Value = value;
            }
        }

        RmList<RmReference> _approver;

        /// <summary>
        /// Approver
        /// The set of approvers.
        /// </summary>
        public IList<RmReference> Approver
        {
            get
            {
                if (this._approver == null)
                {
                    lock (base.attributes)
                    {
                        this._approver = GetMultiValuedReference(AttributeNames.Approver);
                        return this._approver;
                    }
                }
                else
                {
                    return this._approver;
                }
            }
        }

        RmList<RmReference> _computedActor;

        /// <summary>
        /// Computed Actor
        /// This attribute is intended to be used to setup rights as appropriate.
        /// </summary>
        public IList<RmReference> ComputedActor
        {
            get
            {
                if (this._computedActor == null)
                {
                    lock (base.attributes)
                    {
                        this._computedActor = GetMultiValuedReference(AttributeNames.ComputedActor);
                        return this._computedActor;
                    }
                }
                else
                {
                    return this._computedActor;
                }
            }
        }

        List<string> _endpointAddress;

        /// <summary>
        /// Endpoint Address
        /// The endpoint address on which a workflow instance is listening.
        /// </summary>
        public List<string> EndpointAddress
        {
            get
            {
                if (this._endpointAddress == null)
                {
                    lock (base.attributes)
                    {
                        this._endpointAddress = GetMultiValuedString(AttributeNames.EndpointAddress);
                        return this._endpointAddress;
                    }
                }
                else
                {
                    return this._endpointAddress;
                }
            }
        }

        /// <summary>
        /// Request
        /// The Request associated with the given Approval.
        /// </summary>
        public RmReference Request
        {
            get { return GetReference(AttributeNames.Request); }
            set { base[AttributeNames.Request].Value = value; }
        }

        /// <summary>
        /// Requestor
        /// This attribute is intended to be used to setup rights as appropriate.
        /// </summary>
        public RmReference Requestor
        {
            get { return GetReference(AttributeNames.Requestor); }
            set { base[AttributeNames.Requestor].Value = value; }
        }

        /// <summary>
        /// Workflow Instance
        /// WorkflowInstance
        /// </summary>
        public RmReference WorkflowInstance
        {
            get { return GetReference(AttributeNames.WorkflowInstance); }
            set { base[AttributeNames.WorkflowInstance].Value = value; }
        }
        #endregion

        #region Protected methods

        /// <summary>
        /// Ensures all attributes exist.
        /// </summary>
        protected override void EnsureAllAttributesExist()
        {
            base.EnsureAllAttributesExist();

            EnsureAttributeExists(AttributeNames.ApprovalDuration);
            EnsureAttributeExists(AttributeNames.ApprovalResponse);
            EnsureAttributeExists(AttributeNames.ApprovalStatus);
            EnsureAttributeExists(AttributeNames.ApprovalThreshold);
            EnsureAttributeExists(AttributeNames.Approver);
            EnsureAttributeExists(AttributeNames.ComputedActor);
            EnsureAttributeExists(AttributeNames.EndpointAddress);
            EnsureAttributeExists(AttributeNames.Request);
            EnsureAttributeExists(AttributeNames.Requestor);
            EnsureAttributeExists(AttributeNames.WorkflowInstance);
        }

        #endregion

        #region AttributeNames

        /// <summary>
        /// Names of Approval specific attributes
        /// </summary>
        public sealed new class AttributeNames
        {
            public static RmAttributeName ApprovalDuration = new RmAttributeName(@"ApprovalDuration");
            public static RmAttributeName ApprovalResponse = new RmAttributeName(@"ApprovalResponse");
            public static RmAttributeName ApprovalStatus = new RmAttributeName(@"ApprovalStatus");
            public static RmAttributeName ApprovalThreshold = new RmAttributeName(@"ApprovalThreshold");
            public static RmAttributeName Approver = new RmAttributeName(@"Approver");
            public static RmAttributeName ComputedActor = new RmAttributeName(@"ComputedActor");
            public static RmAttributeName EndpointAddress = new RmAttributeName(@"EndpointAddress");
            public static RmAttributeName Request = new RmAttributeName(@"Request");
            public static RmAttributeName Requestor = new RmAttributeName(@"Requestor");
            public static RmAttributeName WorkflowInstance = new RmAttributeName(@"WorkflowInstance");
        }

        #endregion

        /// <summary>
        /// Gets the approval endpoint address.
        /// </summary>
        public string ApprovalEndpointAddress
        {
            get { return GetApprovalEndpointAddress(); }
        }

        /// <summary>
        /// Gets the approval endpoint address.
        /// </summary>
        /// <returns></returns>
        private string GetApprovalEndpointAddress()
        {
            // note that the http protocol is second in the list behind the mail 
            // listener's endpoint
            if (EndpointAddress == null)
            {
                // This could happen e.g. if the object was retrieved with a 
                // query that was specifying the parameters list excluding the 
                // EndpointAddress attribute. As this might be difficult to 
                // find out, let's provide a detailed error message.
                string message =
                    "The Approval object contains no endpoint information. " +
                    "If the object was retrieved with a query or a Get " +
                    "operation, make sure that the 'EndpointAddress' attribute " +
                    "was specified in the list of attributes to retrieve.";
                throw new InvalidOperationException(message);
            }
            if (EndpointAddress.Count != 2)
            {
                // this should never happen
                throw new InvalidOperationException(string.Format(
                    "The Approval object contains {0} endpoints instead of 2.",
                    EndpointAddress.Count));
            }
            return EndpointAddress[1];
        }
    }
}
