using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel.ResourceTypes
{
    public class RmRequest : RmResource
    {
        RmList<RmReference> _actionWorkflowInstance;
        RmList<RmReference> _authenticationWorkflowInstance;
        RmList<RmReference> _authorizationWorkflowInstance;
        RmList<RmReference> _computedActor;
        RmList<RmReference> _managementPolicy;
        List<string> _requestParameter;
        List<string> _requestStatusDetail;

        #region promoted properties

        public IList<RmReference> ActionWorkflowInstance
        {
            get
            {
                if (this._actionWorkflowInstance == null)
                {
                    this._actionWorkflowInstance = GetMultiValuedReference(AttributeNames.ActionWorkflowInstance);
                    return this._actionWorkflowInstance;
                }
                else
                {
                    return this._actionWorkflowInstance;
                }
            }
        }

        public IList<RmReference> AuthenticationWorkflowInstance
        {
            get
            {
                if (this._authenticationWorkflowInstance == null)
                {
                    this._authenticationWorkflowInstance = GetMultiValuedReference(AttributeNames.AuthenticationWorkflowInstance);
                    return this._authenticationWorkflowInstance;
                }
                else
                {
                    return this._authenticationWorkflowInstance;
                }
            }
        }

        public IList<RmReference> AuthorizationWorkflowInstance
        {
            get
            {
                if (this._authorizationWorkflowInstance == null)
                {
                    this._authorizationWorkflowInstance = GetMultiValuedReference(AttributeNames.AuthorizationWorkflowInstance);
                    return this._authorizationWorkflowInstance;
                }
                else
                {
                    return this._authorizationWorkflowInstance;
                }
            }
        }

        public DateTime CommittedTime
        {
            get
            {
                return GetDateTime(AttributeNames.CommittedTime);
            }
            set
            {
                base[AttributeNames.CommittedTime].Value = value;
            }
        }

        public IList<RmReference> ComputedActor
        {
            get
            {
                if (this._computedActor == null)
                {
                    this._computedActor = GetMultiValuedReference(AttributeNames.ComputedActor);
                    return this._computedActor;
                }
                else
                {
                    return this._computedActor;
                }
            }
        }

        public bool HasCollateralRequest
        {
            get
            {
                return GetBoolean(AttributeNames.HasCollateralRequest);
            }
            set
            {
                base[AttributeNames.HasCollateralRequest].Value = value;
            }
        }

        public IList<RmReference> ManagementPolicy
        {
            get
            {
                if (this._managementPolicy == null)
                {
                    this._managementPolicy = GetMultiValuedReference(AttributeNames.ManagementPolicy);
                    return this._managementPolicy;
                }
                else
                {
                    return this._managementPolicy;
                }
            }
        }

        public String Operation
        {
            get
            {
                return GetString(AttributeNames.Operation);
            }
            set
            {
                base[AttributeNames.Operation].Value = value;
            }
        }

        public RmReference ParentRequest
        {
            get
            {
                return GetReference(AttributeNames.ParentRequest);
            }
            set
            {
                base[AttributeNames.ParentRequest].Value = value;
            }
        }

        public String RequestControl
        {
            get
            {
                return GetString(AttributeNames.RequestControl);
            }
            set
            {
                base[AttributeNames.RequestControl].Value = value;
            }
        }

        public List<string> RequestParameter
        {
            get
            {
                if (this._requestParameter == null)
                {
                    this._requestParameter = GetMultiValuedString(AttributeNames.RequestParameter);
                    return this._requestParameter;
                }
                else
                {
                    return this._requestParameter;
                }
            }
        }

        public String RequestStatus
        {
            get
            {
                return GetString(AttributeNames.RequestStatus);
            }
            set
            {
                base[AttributeNames.RequestStatus].Value = value;
            }
        }

        public List<string> RequestStatusDetail
        {
            get
            {
                if (this._requestStatusDetail == null)
                {
                    this._requestStatusDetail = GetMultiValuedString(AttributeNames.RequestStatusDetail);
                    return this._requestStatusDetail;
                }
                else
                {
                    return this._requestStatusDetail;
                }
            }
        }

        public RmReference Target
        {
            get
            {
                return GetReference(AttributeNames.Target);
            }
            set
            {
                base[AttributeNames.Target].Value = value;
            }
        }

        public String TargetObjectType
        {
            get
            {
                return GetString(AttributeNames.TargetObjectType);
            }
            set
            {
                base[AttributeNames.TargetObjectType].Value = value;
            }
        }

        #endregion

        /// <summary>
        /// The type of the wrapped resource.
        /// </summary>
        protected const String ResourceType = @"Request";

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
            return RmRequest.ResourceType;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RmRequest()
            : base()
        {
        }

        #region AttributeNames

        /// <summary>
        /// Names of Approval specific attributes
        /// </summary>
        public sealed new class AttributeNames
        {
            public static RmAttributeName ActionWorkflowInstance = new RmAttributeName(@"ActionWorkflowInstance");
            public static RmAttributeName AuthenticationWorkflowInstance = new RmAttributeName(@"AuthenticationWorkflowInstance");
            public static RmAttributeName AuthorizationWorkflowInstance = new RmAttributeName(@"AuthorizationWorkflowInstance");
            public static RmAttributeName CommittedTime = new RmAttributeName(@"CommittedTime");
            public static RmAttributeName ComputedActor = new RmAttributeName(@"ComputedActor");
            public static RmAttributeName HasCollateralRequest = new RmAttributeName(@"HasCollateralRequest");
            public static RmAttributeName ManagementPolicy = new RmAttributeName(@"ManagementPolicy");
            public static RmAttributeName Operation = new RmAttributeName(@"Operation");
            public static RmAttributeName ParentRequest = new RmAttributeName(@"ParentRequest");
            public static RmAttributeName RequestControl = new RmAttributeName(@"RequestControl");
            public static RmAttributeName RequestParameter = new RmAttributeName(@"RequestParameter");
            public static RmAttributeName RequestStatus = new RmAttributeName(@"RequestStatus");
            public static RmAttributeName RequestStatusDetail = new RmAttributeName(@"RequestStatusDetail");
            public static RmAttributeName Target = new RmAttributeName(@"Target");
            public static RmAttributeName TargetObjectType = new RmAttributeName(@"TargetObjectType");
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Ensures all attributes exist.
        /// </summary>
        protected override void EnsureAllAttributesExist()
        {
            base.EnsureAllAttributesExist();

            EnsureAttributeExists(AttributeNames.ActionWorkflowInstance);
            EnsureAttributeExists(AttributeNames.AuthenticationWorkflowInstance);
            EnsureAttributeExists(AttributeNames.AuthorizationWorkflowInstance);
            EnsureAttributeExists(AttributeNames.CommittedTime);
            EnsureAttributeExists(AttributeNames.ComputedActor);
            EnsureAttributeExists(AttributeNames.HasCollateralRequest);
            EnsureAttributeExists(AttributeNames.ManagementPolicy);
            EnsureAttributeExists(AttributeNames.Operation);
            EnsureAttributeExists(AttributeNames.ParentRequest);
            EnsureAttributeExists(AttributeNames.RequestControl);
            EnsureAttributeExists(AttributeNames.RequestParameter);
            EnsureAttributeExists(AttributeNames.RequestStatus);
            EnsureAttributeExists(AttributeNames.RequestStatusDetail);
            EnsureAttributeExists(AttributeNames.Target);
            EnsureAttributeExists(AttributeNames.TargetObjectType);
        }

        #endregion
    }
}
