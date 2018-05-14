using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OCG.ResourceManagement.ObjectModel;
using OCG.ResourceManagement.ObjectModel.ResourceTypes;

namespace OCG.ResourceManagement.Control.HeritanceControl
{
    public class OcgOrgUnitControl : ResourceControl
    {
        public OcgOrgUnitControl(ClientControl client)
            : base(client)
        {

        }

        #region Override Functions

        public List<RmOrgUnit> GetAllOrgUnit(string[] attributes)
        {
            List<RmResource> resourceList = Base_GetAllResource(RmOrgUnit.StaticResourceType(), attributes);

            return resourceList.ConvertAll<RmOrgUnit>(delegate(RmResource r) { return r as RmOrgUnit; });
        }

        public RmOrgUnit GetOrgUnitByDisplayName(string displayName, string[] attributes)
        {
            return Base_GetResourceByDisplayName(RmOrgUnit.StaticResourceType(), displayName, attributes) as RmOrgUnit;
        }

        public RmOrgUnit GetOrgUnitById(string objectId, string[] attributes)
        {
            return Base_GetResourceById(RmOrgUnit.StaticResourceType(), objectId, attributes) as RmOrgUnit;
        }

        public List<RmOrgUnit> GetOrgUnitByAttribute(string attributeName, string value, OperationType operation, string[] attributes)
        {
            List<RmResource> resourceList = Base_GetResourceByAttribute(RmOrgUnit.StaticResourceType(), attributeName, value, operation, attributes);

            return resourceList.ConvertAll<RmOrgUnit>(delegate(RmResource r) { return r as RmOrgUnit; });
        }

        public List<RmOrgUnit> GetOrgUnitByQuery(string query, string[] attributes)
        {
            List<RmResource> resourceList = Base_GetResourceByQuery(RmOrgUnit.StaticResourceType(), query, attributes);

            return resourceList.ConvertAll<RmOrgUnit>(delegate(RmResource r) { return r as RmOrgUnit; });
        }

        #endregion

        #region Public Methods

        public void RecursiveDeleteOU(RmReference topOUID)
        {
            foreach (RmOrgUnit orgUnit in Base_GetResourceByAttribute(RmOrgUnit.StaticResourceType(), RmOrgUnit.AttributeNames.ParentRef.Name, topOUID.Value, 
                OperationType.Opration_Is, new string[] { RmResource.AttributeNames.ObjectID.Name }))
            {
                RecursiveDeleteOU(orgUnit.ObjectID);
            }

            DeleteResource(topOUID);
        }

        public bool ContainsDisplayName(string parentOUID, string displayName)
        {
            foreach (RmOrgUnit orgUnit in Base_GetResourceByAttribute(RmOrgUnit.StaticResourceType(), RmOrgUnit.AttributeNames.ParentRef.Name, parentOUID,
                OperationType.Opration_Is, new string[] { RmResource.AttributeNames.DisplayName.Name }))
            {
                if (orgUnit.DisplayName.Equals(displayName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsGroup(string ouID, string groupName)
        {
            RmOrgUnit ou = new RmOrgUnit() { ObjectID = new RmReference(ouID) };

            foreach (string objectID in GetAssignedResources(ou))
            {
                RmGroup group = Base_GetResourceById(RmGroup.StaticResourceType(), objectID, new string[] { RmResource.AttributeNames.DisplayName.Name }) as RmGroup;

                if (group != null)
                {
                    if (group.DisplayName.Equals(groupName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public List<string> GetAssignedResources(RmOrgUnit ou)
        {
            List<string> retVal = new List<string>();

            string filter = string.Format("[{0}='{1}' and {2}=/{3}/{4}]",
                    RmOrgAssignment.AttributeNames.AssignedOrgUnit,
                    ou.ObjectID.Value,
                    RmOrgAssignment.AttributeNames.AssignedUser.Name,
                    RmOrgAssignment.StaticResourceType(),
                    RmOrgAssignment.AttributeNames.AssignedUser.Name);
            foreach (RmOrgAssignment orgAssignment in Base_GetResourceByQuery(RmOrgAssignment.StaticResourceType(), filter, new string[] { 
                    RmOrgAssignment.AttributeNames.AssignedUser.Name 
            }))
            {
                if (orgAssignment != null && !retVal.Contains(orgAssignment.AssignedUser.Value))
                {
                    retVal.Add(orgAssignment.AssignedUser.Value);
                }
            }

            return retVal;
        }

        public List<string> GetAssignedRoles(RmOrgUnit ou)
        {
            List<string> retVal = new List<string>();

            string filter = string.Format("[{0}='{1}' and {2}=/{3}/{4}]",
                    RmOrgAssignment.AttributeNames.AssignedOrgUnit,
                    ou.ObjectID.Value,
                    RmOrgAssignment.AttributeNames.AssignedRole.Name,
                    RmOrgAssignment.StaticResourceType(),
                    RmOrgAssignment.AttributeNames.AssignedRole.Name);
            foreach (RmOrgAssignment orgAssignment in Base_GetResourceByQuery(RmOrgAssignment.StaticResourceType(), filter, new string[] { 
                    RmOrgAssignment.AttributeNames.AssignedRole.Name 
            }))
            {
                if (orgAssignment != null && !retVal.Contains(orgAssignment.AssignedRole.Value))
                {
                    retVal.Add(orgAssignment.AssignedRole.Value);
                }
            }

            return retVal;
        }

        public List<string> GetConfilctRoles(RmOrgUnit ou)
        {
            List<string> retVal = new List<string>();

            if (ou.RoleRefList == null)
            {
                return retVal;
            }

            List<string> ouRoleList = new List<string>();
            foreach (RmReference role in ou.RoleRefList)
            {
                ouRoleList.Add(role.Value);
            }

            foreach (RmSSoD ssod in Base_GetResourceByAttribute(RmSSoD.StaticResourceType(), RmSSoD.AttributeNames.Enabled.Name, "true", ResourceControl.OperationType.Opration_Is,
                    new string[] { RmResource.AttributeNames.DisplayName.Name, RmSSoD.AttributeNames.RejectionRefs.Name }))
            {
                List<string> potentialConflictList = new List<string>();
                foreach (RmReference rejectedRef in ssod.RejectionRefs)
                {
                    if (ouRoleList.Contains(rejectedRef.Value))
                    {
                        if (!potentialConflictList.Contains(rejectedRef.Value))
                        {
                            potentialConflictList.Add(rejectedRef.Value);
                        }
                    }
                }
                if (potentialConflictList.Count > 1)
                {
                    string conflict = ssod.ObjectID.Value + ":";
                    foreach (string conflictRole in potentialConflictList)
                    {
                        if (conflict == ssod.ObjectID.Value + ":")
                        {
                            conflict += conflictRole;
                        }
                        else
                        {
                            conflict = conflict + "|" + conflictRole;
                        }
                    }
                    if (!retVal.Contains(conflict))
                    {
                        retVal.Add(conflict);
                    }
                }
            }

            return retVal;
        }

        public bool HaveRootOU(string startOUID, string rootOUID)
        {
            if (startOUID.Equals(rootOUID, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            RmOrgUnit startOU = GetOrgUnitById(startOUID, new string[] { RmOrgUnit.AttributeNames.ParentRef.Name });
            if (startOU == null)
            {
                return false;
            }
            if (startOU.ParentRef == null || string.IsNullOrEmpty(startOU.ParentRef.Value))
            {
                return false;
            }

            return HaveRootOU(startOU.ParentRef.Value, rootOUID);
        }

        #endregion
    }
}
