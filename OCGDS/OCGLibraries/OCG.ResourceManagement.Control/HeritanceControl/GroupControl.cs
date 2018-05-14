using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OCG.ResourceManagement.ObjectModel;
using OCG.ResourceManagement.ObjectModel.ResourceTypes;

namespace OCG.ResourceManagement.Control.HeritanceControl
{
    public class GroupControl : ResourceControl
    {
        public GroupControl(ClientControl client)
            : base(client)
        {

        }

        #region Override Functions

        public List<RmGroup> GetAllGroup(string[] attributes)
        {
            List<RmResource> resourceList = Base_GetAllResource(RmGroup.StaticResourceType(), attributes);

            return resourceList.ConvertAll<RmGroup>(delegate(RmResource r) { return r as RmGroup; });
        }

        public RmGroup GetGroupByDisplayName(string displayName, string[] attributes)
        {
            return Base_GetResourceByDisplayName(RmGroup.StaticResourceType(), displayName, attributes) as RmGroup;
        }

        public RmGroup GetGroupById(string objectId, string[] attributes)
        {
            return Base_GetResourceById(RmGroup.StaticResourceType(), objectId, attributes) as RmGroup;
        }

        public List<RmGroup> GetGroupByAttribute(string attributeName, string value, OperationType operation, string[] attributes)
        {
            List<RmResource> resourceList = Base_GetResourceByAttribute(RmGroup.StaticResourceType(), attributeName, value, operation, attributes);

            return resourceList.ConvertAll<RmGroup>(delegate(RmResource r) { return r as RmGroup; });
        }

        public List<RmGroup> GetGroupByQuery(string query, string[] attributes)
        {
            List<RmResource> resourceList = Base_GetResourceByQuery(RmGroup.StaticResourceType(), query, attributes);

            return resourceList.ConvertAll<RmGroup>(delegate(RmResource r) { return r as RmGroup; });
        }

        #endregion

        #region Public Methods

        public List<RmRole> GetAssignedRoles(RmGroup group, string[] attributes)
        {
            List<RmRole> retVal = new List<RmRole>();

            foreach (RmUserAssignment assignment in Base_GetResourceByAttribute(
                RmUserAssignment.StaticResourceType(),
                RmUserAssignment.AttributeNames.AssignedUser.Name,
                group.ObjectID.Value,
                OperationType.Opration_Is,
                new string[] { RmUserAssignment.AttributeNames.AssignedRole.Name }))
            {
                RmRole role = Base_GetResourceById(RmRole.StaticResourceType(), assignment.AssignedRole.Value, attributes) as RmRole;

                if (role != null)
                {
                    if (!retVal.Any(r => r.ObjectID.Value == role.ObjectID.Value))
                    {
                        retVal.Add(role);
                    }
                }
            }

            return retVal;
        }

        public RmResource GetGroupDisplayedOwner(RmGroup group, string[] attributes)
        {
            RmPerson person = Base_GetResourceById(RmPerson.StaticResourceType(), group.DisplayedOwner.Value, attributes) as RmPerson;
            if (person != null)
            {
                return person;
            }

            return Base_GetResourceById(RmGroup.StaticResourceType(), group.DisplayedOwner.Value, attributes);
        }

        public List<RmResource> GetGroupExplicitMembers(RmGroup group, string[] attributes)
        {
            List<RmResource> retVal = new List<RmResource>();

            foreach (RmReference memberRef in group.ExplicitMember)
            {
                RmPerson person = Base_GetResourceById(RmPerson.StaticResourceType(), memberRef.Value, attributes) as RmPerson;
                if (person != null)
                {
                    retVal.Add(person);
                    continue;
                }

                RmGroup gp = Base_GetResourceById(RmGroup.StaticResourceType(), memberRef.Value, attributes) as RmGroup;
                if (gp != null)
                {
                    retVal.Add(gp);
                }
            }

            return retVal;
        }

        public List<RmGroup> GetAllGroupWithExplicitMember()
        {
            List<RmGroup> retVal = new List<RmGroup>();

            foreach (RmGroup gp in Base_GetAllResource(RmGroup.StaticResourceType(), new string[] { 
                RmResource.AttributeNames.ObjectID.Name, 
                RmResource.AttributeNames.DisplayName.Name, 
                RmGroup.AttributeNames.ExplicitMember.Name, 
                RmGroup.AttributeNames.Filter.Name, 
                RmGroup.AttributeNames.FromSharePoint.Name
            }))
            {
                if (gp != null && (gp.Filter == null || gp.Filter == string.Empty))
                {
                    retVal.Add(gp);
                }
            }

            return retVal;
        }

        public void AddPersonToGroup(RmPerson person, RmGroup group)
        {
            if (!Client.SchemaCached)
            {
                Client.RefreshSchema();
            }

            if (person.ObjectID == null)
            {
                ClientControl.ErrorControl.AddError(new ErrorData(@"Cannot find person object ID"));
                return;
            }

            if (group.ExplicitMember == null)
            {
                ClientControl.ErrorControl.AddError(new ErrorData(@"Cannot load ExplicitMember property of the group"));
                return;
            }

            using (RmResourceChanges transaction = new RmResourceChanges(group))
            {
                if (!group.ExplicitMember.Contains(person.ObjectID))
                {
                    transaction.BeginChanges();

                    group.ExplicitMember.Add(person.ObjectID);
                    Client.Put(transaction);

                    transaction.AcceptChanges();
                }
            }
        }

        public void RemovePersonFromGroup(RmPerson person, RmGroup group)
        {
            if (!Client.SchemaCached)
            {
                Client.RefreshSchema();
            }

            if (person.ObjectID == null)
            {
                ClientControl.ErrorControl.AddError(new ErrorData(@"Cannot find person object ID"));
                return;
            }

            if (group.ExplicitMember == null)
            {
                ClientControl.ErrorControl.AddError(new ErrorData(@"Cannot load ExplicitMember property of the group"));
                return;
            }

            using (RmResourceChanges transaction = new RmResourceChanges(group))
            {
                if (group.ExplicitMember.Contains(person.ObjectID))
                {
                    transaction.BeginChanges();

                    group.ExplicitMember.Remove(person.ObjectID);
                    Client.Put(transaction);

                    transaction.AcceptChanges();
                }
            }
        }

        #endregion
    }
}
