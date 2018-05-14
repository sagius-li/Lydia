using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OCG.ResourceManagement.ObjectModel;
using OCG.ResourceManagement.ObjectModel.ResourceTypes;

namespace OCG.ResourceManagement.Control.HeritanceControl
{
    public class OcgOrgAssignmentControl : ResourceControl
    {
        public OcgOrgAssignmentControl(ClientControl client)
            : base(client)
        {

        }

        #region Override Functions

        public List<RmOrgAssignment> GetAllOrgAssignment(string[] attributes)
        {
            List<RmResource> resourceList = Base_GetAllResource(RmOrgAssignment.StaticResourceType(), attributes);

            return resourceList.ConvertAll<RmOrgAssignment>(delegate(RmResource r) { return r as RmOrgAssignment; });
        }

        public RmOrgAssignment GetOrgAssignmentByDisplayName(string displayName, string[] attributes)
        {
            return Base_GetResourceByDisplayName(RmOrgAssignment.StaticResourceType(), displayName, attributes) as RmOrgAssignment;
        }

        public RmOrgAssignment GetOrgAssignmentById(string objectId, string[] attributes)
        {
            return Base_GetResourceById(RmOrgAssignment.StaticResourceType(), objectId, attributes) as RmOrgAssignment;
        }

        public List<RmOrgAssignment> GetOrgAssignmentByAttribute(string attributeName, string value, OperationType operation, string[] attributes)
        {
            List<RmResource> resourceList = Base_GetResourceByAttribute(RmOrgAssignment.StaticResourceType(), attributeName, value, operation, attributes);

            return resourceList.ConvertAll<RmOrgAssignment>(delegate(RmResource r) { return r as RmOrgAssignment; });
        }

        public List<RmOrgAssignment> GetOrgAssignmentByQuery(string query, string[] attributes)
        {
            List<RmResource> resourceList = Base_GetResourceByQuery(RmOrgAssignment.StaticResourceType(), query, attributes);

            return resourceList.ConvertAll<RmOrgAssignment>(delegate(RmResource r) { return r as RmOrgAssignment; });
        }

        #endregion

        #region Public Methods

        public RmOrgAssignment GetOrgAssignmentByRelationship(string userID, string ouID, string[] attributes)
        {
            string filter = string.Format("[{0}='{1}' and {2}='{3}']", 
                RmOrgAssignment.AttributeNames.AssignedUser.Name, 
                userID, 
                RmOrgAssignment.AttributeNames.AssignedOrgUnit, 
                ouID);
            foreach (RmOrgAssignment assignment in Base_GetResourceByQuery(RmOrgAssignment.StaticResourceType(), filter, attributes))
            {
                return assignment;
            }

            return null;
        }

        public bool IsOURoleAssignmentMandatory(string roleID, string ouID)
        {
            string filter = string.Format("[{0}='{1}' and {2}='{3}']",
                RmOrgAssignment.AttributeNames.AssignedRole.Name, 
                roleID, 
                RmOrgAssignment.AttributeNames.AssignedOrgUnit.Name, 
                ouID);
            foreach (RmOrgAssignment assignment in Base_GetResourceByQuery(RmOrgAssignment.StaticResourceType(), filter, 
                new string[] { RmOrgAssignment.AttributeNames.IsMandatory.Name }))
            {
                if (assignment != null)
                {
                    return assignment.IsMandatory;
                }
            }

            return false;
        }

        #endregion
    }
}
