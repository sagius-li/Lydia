using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OCG.ResourceManagement.ObjectModel;
using OCG.ResourceManagement.ObjectModel.ResourceTypes;

namespace OCG.ResourceManagement.Control.HeritanceControl
{
    public class OcgSSoDControl : ResourceControl
    {
        public OcgSSoDControl(ClientControl client)
            : base(client)
        {

        }

        #region Override Functions

        public List<RmSSoD> GetAllSSoD(string[] attributes)
        {
            List<RmResource> resourceList = Base_GetAllResource(RmSSoD.StaticResourceType(), attributes);

            return resourceList.ConvertAll<RmSSoD>(delegate(RmResource r) { return r as RmSSoD; });
        }

        public RmSSoD GetSSoDByDisplayName(string displayName, string[] attributes)
        {
            return Base_GetResourceByDisplayName(RmSSoD.StaticResourceType(), displayName, attributes) as RmSSoD;
        }

        public RmSSoD GetSSoDById(string objectId, string[] attributes)
        {
            return Base_GetResourceById(RmSSoD.StaticResourceType(), objectId, attributes) as RmSSoD;
        }

        public List<RmSSoD> GetSSoDByAttribute(string attributeName, string value, OperationType operation, string[] attributes)
        {
            List<RmResource> resourceList = Base_GetResourceByAttribute(RmSSoD.StaticResourceType(), attributeName, value, operation, attributes);

            return resourceList.ConvertAll<RmSSoD>(delegate(RmResource r) { return r as RmSSoD; });
        }

        public List<RmSSoD> GetSSoDByQuery(string query, string[] attributes)
        {
            List<RmResource> resourceList = Base_GetResourceByQuery(RmSSoD.StaticResourceType(), query, attributes);

            return resourceList.ConvertAll<RmSSoD>(delegate(RmResource r) { return r as RmSSoD; });
        }

        #endregion

        #region Public Methods

        public List<string> GetConflictRoles(RmRole role)
        {
            List<string> retVal = new List<string>();

            if (role.ObjectID == null)
            {
                return retVal;
            }

            foreach (RmSSoD ssod in Base_GetResourceByAttribute(RmSSoD.StaticResourceType(), RmSSoD.AttributeNames.RejectionRefs.Name, role.ObjectID.Value, OperationType.Opration_Is,
                new string[] {
                    RmResource.AttributeNames.ObjectID.Name, 
                    RmResource.AttributeNames.DisplayName.Name, 
                    RmSSoD.AttributeNames.RejectionRefs.Name
                }))
            {
                string roleIDString = ssod.ObjectID.Value + ":";
                foreach (RmReference roleRef in ssod.RejectionRefs)
                {
                    if (roleIDString == (ssod.ObjectID.Value + ":"))
                    {
                        roleIDString = roleIDString + roleRef.Value;
                    }
                    else
                    {
                        roleIDString = roleIDString + "|" + roleRef.Value;
                    }
                }

                if (!retVal.Contains(roleIDString))
                {
                    retVal.Add(roleIDString);
                }
            }

            return retVal;
        }

        #endregion
    }
}
