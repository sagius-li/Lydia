using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OCG.ResourceManagement.ObjectModel;
using OCG.ResourceManagement.ObjectModel.ResourceTypes;

namespace OCG.ResourceManagement.Control.HeritanceControl
{
    public class OcgRoleControl : ResourceControl
    {
        public OcgRoleControl(ClientControl client)
            : base(client)
        {

        }

        #region Override Functions

        public List<RmRole> GetAllRole(string[] attributes)
        {
            List<RmResource> resourceList = Base_GetAllResource(RmRole.StaticResourceType(), attributes);

            return resourceList.ConvertAll<RmRole>(delegate(RmResource r) { return r as RmRole; });
        }

        public RmRole GetRoleByDisplayName(string displayName, string[] attributes)
        {
            return Base_GetResourceByDisplayName(RmRole.StaticResourceType(), displayName, attributes) as RmRole;
        }

        public RmRole GetRoleById(string objectId, string[] attributes)
        {
            return Base_GetResourceById(RmRole.StaticResourceType(), objectId, attributes) as RmRole;
        }

        public List<RmRole> GetRoleByAttribute(string attributeName, string value, OperationType operation, string[] attributes)
        {
            List<RmResource> resourceList = Base_GetResourceByAttribute(RmRole.StaticResourceType(), attributeName, value, operation, attributes);

            return resourceList.ConvertAll<RmRole>(delegate(RmResource r) { return r as RmRole; });
        }

        public List<RmRole> GetRoleByQuery(string query, string[] attributes)
        {
            List<RmResource> resourceList = Base_GetResourceByQuery(RmRole.StaticResourceType(), query, attributes);

            return resourceList.ConvertAll<RmRole>(delegate(RmResource r) { return r as RmRole; });
        }

        #endregion

        #region Public Methods

        public List<RmPermission> GetRolePermissions(RmRole role, string[] attributes)
        {
            List<RmPermission> retVal = new List<RmPermission>();

            foreach (RmReference permissionRef in role.PremissionRefs)
            {
                RmPermission permission = Base_GetResourceById(RmPermission.StaticResourceType(), permissionRef.Value, attributes) as RmPermission;

                if (permission != null)
                {
                    retVal.Add(permission);
                }
            }

            return retVal;
        }

        public List<string> GetPermissionBackLink(RmPermission permission)
        {
            List<string> retVal = new List<string>();

            foreach (RmRole role in Base_GetResourceByAttribute(RmRole.StaticResourceType(), RmRole.AttributeNames.PremissionRefs.Name, permission.ObjectID.Value,
                OperationType.Opration_Is, new string[] { RmResource.AttributeNames.ObjectID.Name }))
            {
                if (role != null)
                {
                    foreach (RmPerson person in Base_GetResourceByAttribute(RmPerson.StaticResourceType(), RmPerson.AttributeNames.RoleRefList.Name,
                        role.ObjectID.Value, OperationType.Opration_Is, new string[] { RmResource.AttributeNames.ObjectID.Name }))
                    {
                        if (person != null)
                        {
                            if (!retVal.Contains(person.ObjectID.Value))
                            {
                                retVal.Add(person.ObjectID.Value);
                            }
                        }
                    }
                }
            }

            return retVal;
        }

        public List<string> GetPermissionGroupBackLink(RmGroup group)
        {
            List<string> retVal = new List<string>();

            foreach (RmRole role in Base_GetResourceByAttribute(RmRole.StaticResourceType(), RmRole.AttributeNames.PremissionRefs.Name, group.ObjectID.Value,
                OperationType.Opration_Is, new string[] { RmResource.AttributeNames.ObjectID.Name }))
            {
                if (role != null)
                {
                    foreach (RmPerson person in Base_GetResourceByAttribute(RmPerson.StaticResourceType(), RmPerson.AttributeNames.RoleRefList.Name,
                        role.ObjectID.Value, OperationType.Opration_Is, new string[] { RmResource.AttributeNames.ObjectID.Name }))
                    {
                        if (person != null)
                        {
                            if (!retVal.Contains(person.ObjectID.Value))
                            {
                                retVal.Add(person.ObjectID.Value);
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
