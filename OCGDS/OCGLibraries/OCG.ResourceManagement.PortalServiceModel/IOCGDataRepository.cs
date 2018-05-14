using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.PortalServiceModel
{
    public enum RMAccessType
    {
        UserRM = 1,
        AdminRM,
        RemoteRM,
        NamedRM
    }

    public interface IOCGDataRepository
    {
        void InitRemoteRM(string userName, string userDomain, string userPWD, string serviceAddress = "");

        List<PsResource> GetResourceByQueryDB(string query, string requestorID = "", string[] attributes = null, bool resolveRef = false, bool checkRights = true, int count = -1, int skip = 0);

        List<PsAttributeDef> GetTypeAttributesDB(string typeName, int localeKey = 127);

        List<PsRight> GetRightsDB(Guid actor, Guid target);

        PsResource GetResourceByID(string objectID, string[] attributes = null, RMAccessType accessType = RMAccessType.UserRM, bool resolveRef = false);

        List<PsResource> GetResourceByQuery(string query, string[] attributes = null, RMAccessType accessType = RMAccessType.UserRM, bool resolveRef = false);

        bool UpdateResource(PsResource resource, RMAccessType accessType = RMAccessType.UserRM, bool isDelta = false);

        string CreateResource(PsResource resource, RMAccessType accessType = RMAccessType.UserRM);

        bool DeleteResource(string objectID, RMAccessType accessType = RMAccessType.UserRM);
    }
}
