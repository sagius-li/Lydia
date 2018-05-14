using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using OCG.ResourceManagement.ObjectModel;
using OCG.ResourceManagement.ObjectModel.ResourceTypes;

namespace OCG.ResourceManagement.Control.HeritanceControl
{
    public class ApprovalControl : ResourceControl
    {
        public ApprovalControl(ClientControl client)
            : base(client)
        {

        }

        #region Override Functions

        public List<RmApproval> GetAllApproval(string[] attributes)
        {
            List<RmResource> resourceList = Base_GetAllResource(RmApproval.StaticResourceType(), attributes);

            return resourceList.ConvertAll<RmApproval>(delegate(RmResource r) { return r as RmApproval; });
        }

        public RmApproval GetApprovalByDisplayName(string displayName, string[] attributes)
        {
            return Base_GetResourceByDisplayName(RmApproval.StaticResourceType(), displayName, attributes) as RmApproval;
        }

        public RmApproval GetApprovalById(string objectId, string[] attributes)
        {
            return Base_GetResourceById(RmApproval.StaticResourceType(), objectId, attributes) as RmApproval;
        }

        public List<RmApproval> GetApprovalByAttribute(string attributeName, string value, OperationType operation, string[] attributes)
        {
            List<RmResource> resourceList = Base_GetResourceByAttribute(RmApproval.StaticResourceType(), attributeName, value, operation, attributes);

            return resourceList.ConvertAll<RmApproval>(delegate(RmResource r) { return r as RmApproval; });
        }

        public List<RmApproval> GetApprovalByQuery(string query, string[] attributes)
        {
            List<RmResource> resourceList = Base_GetResourceByQuery(RmApproval.StaticResourceType(), query, attributes);

            return resourceList.ConvertAll<RmApproval>(delegate(RmResource r) { return r as RmApproval; });
        }

        #endregion

        #region Public Methods

        public bool Approve(RmApproval approval, EndpointIdentity identity)
        {
            if (approval == null)
            {
                return false;
            }

            EndpointAddress address = new EndpointAddress(
                        new Uri(approval.ApprovalEndpointAddress),
                        identity);
            try
            {
                this.Client.Approve(approval, true, address);
            }
            catch (Exception exc)
            {
                return false;
            }

            return true;
        }

        public string Approve(RmApproval approval, EndpointIdentity identity, bool isApproved)
        {
            if (approval == null)
            {
                return "Approval cannot be null";
            }

            EndpointAddress address = new EndpointAddress(
                        new Uri(approval.ApprovalEndpointAddress),
                        identity);

            try
            {
                this.Client.Approve(approval, isApproved, address);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        #endregion
    }
}
