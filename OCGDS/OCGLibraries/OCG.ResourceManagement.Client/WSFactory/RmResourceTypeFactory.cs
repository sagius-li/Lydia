using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OCG.ResourceManagement.ObjectModel;
using OCG.ResourceManagement.ObjectModel.ResourceTypes;

namespace OCG.ResourceManagement.Client.WSFactory
{
    public class RmResourceTypeFactory
    {
        public RmGeneric CreateResource(string resourceType, bool returnGenericType)
        {
            if (returnGenericType)
            {
                return new RmGeneric();
            }
            if (String.IsNullOrEmpty(resourceType))
            {
                return new RmResource();
            }
            String upperCaseResourceType = resourceType.ToUpperInvariant();
            switch (upperCaseResourceType)
            {
                case @"GROUP":
                    return new RmGroup();
                case @"PERSON":
                    return new RmPerson();
                case @"OBJECTTYPEDESCRIPTION":
                    return new RmType();
                case @"ATTRIBUTETYPEDESCRIPTION":
                    return new RmAttribute();
                case @"OCGORGUNIT":
                    return new RmOrgUnit();
                case @"OCGORGASSIGNMENT":
                    return new RmOrgAssignment();
                case @"OCGROLE":
                    return new RmRole();
                case @"OCGUSERASSIGNMENT":
                    return new RmUserAssignment();
                case @"OCGPERMISSION":
                    return new RmPermission();
                case @"OCGSSOD":
                    return new RmSSoD();
                case @"APPROVAL":
                    return new RmApproval();
                case @"REQUEST":
                    return new RmRequest();
                default:
                    return new RmResource();
            }
        }
    }
}
