using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OCG.ResourceManagement.ObjectModel;
using OCG.ResourceManagement.ObjectModel.ResourceTypes;

namespace OCG.ResourceManagement.Control.HeritanceControl
{
    public class PersonControl : ResourceControl
    {
        public PersonControl(ClientControl client)
            : base(client)
        {

        }

        #region Override Functions

        public List<RmPerson> GetAllPerson(string[] attributes)
        {
            List<RmResource> resourceList = Base_GetAllResource(RmPerson.StaticResourceType(), attributes);

            return resourceList.ConvertAll<RmPerson>(delegate(RmResource r) { return r as RmPerson; });
        }

        public RmPerson GetPersonByDisplayName(string displayName, string[] attributes)
        {
            return Base_GetResourceByDisplayName(RmPerson.StaticResourceType(), displayName, attributes) as RmPerson;
        }

        public RmPerson GetPersonById(string objectId, string[] attributes)
        {
            return Base_GetResourceById(RmPerson.StaticResourceType(), objectId, attributes) as RmPerson;
        }

        public List<RmPerson> GetPersonByAttribute(string attributeName, string value, OperationType operation, string[] attributes)
        {
            List<RmResource> resourceList = Base_GetResourceByAttribute(RmPerson.StaticResourceType(), attributeName, value, operation, attributes);

            return resourceList.ConvertAll<RmPerson>(delegate(RmResource r) { return r as RmPerson; });
        }

        public List<RmPerson> GetPersonByQuery(string query, string[] attributes)
        {
            List<RmResource> resourceList = Base_GetResourceByQuery(RmPerson.StaticResourceType(), query, attributes);

            return resourceList.ConvertAll<RmPerson>(delegate(RmResource r) { return r as RmPerson; });
        }

        #endregion

        #region Public Methods

        public RmPerson GetPersonByDomainAccount(string domainAccount, string[] attributes)
        {
            string[] da = domainAccount.Split(@"\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (da.Length != 2)
            {
                return null;
            }

            string filter = string.Format("[{0}='{1}' and {2}='{3}']", RmPerson.AttributeNames.Domain.Name, da[0], RmPerson.AttributeNames.AccountName.Name, da[1]);
            foreach (RmPerson person in Base_GetResourceByQuery(RmPerson.StaticResourceType(), filter, attributes))
            {
                return person;
            }

            return null;
        }

        public List<string> GetConfilctRoles(RmPerson person)
        {
            List<string> retVal = new List<string>();

            if (person.RoleRefList == null)
            {
                return retVal;
            }

            List<string> ouRoleList = new List<string>();
            foreach (RmReference role in person.RoleRefList)
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

        public List<RmGroup> GetAssignedGroups(RmPerson person, string[] attributes)
        {
            List<RmGroup> retVal = new List<RmGroup>();

            string query = string.Format("[{0}='{1}' or {2}='{3}']", 
                RmGroup.AttributeNames.ExplicitMember.Name, 
                person.ObjectID.Value, 
                RmGroup.AttributeNames.ComputedMember.Name, 
                person.ObjectID.Value);

            foreach (RmGroup group in Base_GetResourceByQuery(RmGroup.StaticResourceType(), query, attributes))
            {
                if (group != null)
                {
                    retVal.Add(group);
                }
            }

            return retVal;
        }

        public List<string> GetAssignedRoles(RmPerson person)
        {
            List<string> retVal = new List<string>();

            foreach (RmUserAssignment assignment in Base_GetResourceByAttribute(RmUserAssignment.StaticResourceType(), RmUserAssignment.AttributeNames.AssignedUser.Name,
                person.ObjectID.Value, OperationType.Opration_Is, new string[] { RmUserAssignment.AttributeNames.AssignedRole.Name }))
            {
                if (assignment != null)
                {
                    if (!retVal.Contains(assignment.AssignedRole.Value))
                    {
                        retVal.Add(assignment.AssignedRole.Value);
                    }
                }
            }

            return retVal;
        }

        public bool HasSharePointGroup(RmPerson person, IList<RmReference> exclusiveGroupId)
        {
            foreach (RmGroup group in Base_GetResourceByAttribute(RmGroup.StaticResourceType(), RmGroup.AttributeNames.ExplicitMember.Name, person.ObjectID.Value, 
                OperationType.Opration_Is, new string[] { RmResource.AttributeNames.ObjectID.Name, RmGroup.AttributeNames.FromSharePoint.Name }))
            {
                if (!group.FromSharePoint)
                {
                    continue;
                }

                if (exclusiveGroupId != null)
                {
                    bool containId = false;
                    foreach (RmReference reference in exclusiveGroupId)
                    {
                        if (reference.Value == group.ObjectID.Value)
                        {
                            containId = true;
                        }
                    }
                    if (!containId)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public List<RmRole> GetAllAvailableRoles(string personID, string[] attributes)
        {
            List<RmRole> retVal = new List<RmRole>();
            List<string> roleIDList = new List<string>();

            foreach (RmOrgAssignment ouAssignment in Base_GetResourceByAttribute(
                RmOrgAssignment.StaticResourceType(),
                RmOrgAssignment.AttributeNames.AssignedUser.Name,
                personID,
                OperationType.Opration_Is,
                new string[] { RmOrgAssignment.AttributeNames.AssignedOrgUnit.Name }))
            {
                if (ouAssignment != null)
                {
                    RmOrgUnit ou = Base_GetResourceById(RmOrgUnit.StaticResourceType(), ouAssignment.AssignedOrgUnit.Value, 
                        new string[] { RmOrgUnit.AttributeNames.RoleRefList.Name }) as RmOrgUnit;
                    if (ou != null && ou.RoleRefList != null)
                    {
                        foreach (RmReference roleRef in ou.RoleRefList)
                        {
                            if (roleRef != null)
                            {
                                if (!roleIDList.Contains(roleRef.Value))
                                {
                                    RmRole role = Base_GetResourceById(RmRole.StaticResourceType(), roleRef.Value, attributes) as RmRole;
                                    if (role != null)
                                    {
                                        retVal.Add(role);
                                        roleIDList.Add(role.ObjectID.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return retVal;
        }

        public List<RmRole> GetAllOptionalRoles(string personID, string[] attributes)
        {
            List<RmRole> retVal = new List<RmRole>();

            foreach (RmOrgAssignment ouAssignment in Base_GetResourceByAttribute(
                RmOrgAssignment.StaticResourceType(), 
                RmOrgAssignment.AttributeNames.AssignedUser.Name, 
                personID, 
                OperationType.Opration_Is, 
                new string[] { RmOrgAssignment.AttributeNames.AssignedOrgUnit.Name }))
            {
                if (ouAssignment != null)
                {
                    string filter = string.Format("[{0}='{1}' and {2}=/{3}/{4}]",
                    RmOrgAssignment.AttributeNames.AssignedOrgUnit, 
                    ouAssignment.AssignedOrgUnit.Value, 
                    RmOrgAssignment.AttributeNames.AssignedRole.Name, 
                    RmOrgAssignment.StaticResourceType(), 
                    RmOrgAssignment.AttributeNames.AssignedRole.Name);
                    foreach (RmOrgAssignment roleAssignment in Base_GetResourceByQuery(
                        RmOrgAssignment.StaticResourceType(), 
                        filter, 
                        new string[] { RmOrgAssignment.AttributeNames.AssignedRole.Name, RmOrgAssignment.AttributeNames.IsMandatory.Name }))
                    {
                        if (roleAssignment != null && !roleAssignment.IsMandatory)
                        {
                            RmRole role = Base_GetResourceById(RmRole.StaticResourceType(), roleAssignment.AssignedRole.Value, attributes) as RmRole;
                            if (role != null)
                            {
                                retVal.Add(role);
                            }
                        }
                    }
                }
            }

            return retVal;
        }

        #endregion
    }
}
