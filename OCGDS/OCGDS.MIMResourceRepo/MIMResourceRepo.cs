using Lithnet.ResourceManagement.Client;
using Microsoft.ResourceManagement.WebServices.WSEnumeration;
using OCG.ResourceManagement.ObjectModel.ResourceTypes;
using OCG.Security.Operation;
using OCGDS.DSModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OCGDS.MIMResourceRepo
{
    [ExportMetadata("Name", "MIMResource")]
    [Export("ResourceManagement", typeof(IOCGDSRepository))]
    public class MIMResourceRepo : IOCGDSRepository
    {
        private ResourceManagementClient getClient(ConnectionInfo info)
        {
            ResourceManagementClient client = null;

            if (info == null)
            {
                client = new ResourceManagementClient();
            }
            else
            {
                NetworkCredential cred = null;
                if (!string.IsNullOrEmpty(info.Domain) &&
                    !string.IsNullOrEmpty(info.UserName) && !string.IsNullOrEmpty(info.Password))
                {
                    cred = new NetworkCredential(info.UserName,
                        GenericAESCryption.DecryptString(info.Password), info.Domain);
                }

                if (cred == null)
                {
                    client = string.IsNullOrEmpty(info.BaseAddress) ?
                        new ResourceManagementClient() : new ResourceManagementClient(info.BaseAddress);
                }
                else
                {
                    client = string.IsNullOrEmpty(info.BaseAddress) ?
                        new ResourceManagementClient(cred) : new ResourceManagementClient(info.BaseAddress, cred);
                }
            }

            return client;
        }

        private List<RmAttribute> getAttributeDefinition(string objectType, int cultureKey)
        {
            List<RmAttribute> attDef = null;
            if (cultureKey != 127)
            {
                string connString = ConfigManager.GetAppSetting("FIMDBConnectionString",
                    "Integrated Security=SSPI;Initial Catalog=FIMService;Data Source=localhost;");
                string adminAccountName = ConfigManager.GetAppSetting("FIMPortalAdminAccountName", string.Empty);
                string adminAccountPWD = ConfigManager.GetAppSetting("FIMPortalAdminAccountPWD", string.Empty);

                OCG.ResourceManagement.DBAccess.ResourceReader resourceReader = null;
                if (string.IsNullOrEmpty(adminAccountName) || string.IsNullOrEmpty(adminAccountPWD))
                {
                    resourceReader = new OCG.ResourceManagement.DBAccess.ResourceReader(connString);
                }
                else
                {
                    resourceReader = new OCG.ResourceManagement.DBAccess.ResourceReader(
                        connString, adminAccountName, GenericAESCryption.DecryptString(adminAccountPWD));
                }
                attDef = resourceReader.GetAttributes(objectType, cultureKey);
            }

            return attDef;
        }

        private List<SortingAttribute> getSortingAttributes(string[] sortingAttributes)
        {
            List<SortingAttribute> retVal = new List<SortingAttribute>();

            if (sortingAttributes != null && sortingAttributes.Length > 0)
            {
                foreach (string kvp in sortingAttributes)
                {
                    try
                    {
                        string[] kvps = kvp.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string attributeName = kvps[0];
                        string sortType = kvps[1];

                        bool ascending = (sortType.Equals("a", StringComparison.OrdinalIgnoreCase) || sortType.Equals("asc", StringComparison.OrdinalIgnoreCase) || sortType.Equals("ascend", StringComparison.OrdinalIgnoreCase) || sortType.Equals("ascending", StringComparison.OrdinalIgnoreCase)) ? true : false;

                        retVal.Add(new SortingAttribute(attributeName, ascending));
                    }
                    catch { }
                }
            }

            return retVal;
        }

        public DSResource convertToDSResource(ResourceManagementClient client, ResourceObject resource, 
            string[] loadedAttributes, bool includePermission, ResourceOption option, bool deepResolve = true)
        {
            DSResource dsResource = new DSResource
            {
                DisplayName = resource.DisplayName,
                ObjectID = resource.ObjectID.Value,
                ObjectType = resource.ObjectTypeName,
                HasPermissionHints = resource.HasPermissionHints
            };

            List<RmAttribute> attributeDef = getAttributeDefinition(resource.ObjectTypeName, option.CultureKey);

            Dictionary<string, DSAttribute> attributes = new Dictionary<string, DSAttribute>();

            foreach (string attributeName in loadedAttributes)
            {
                if (resource.Attributes.ContainsAttribute(attributeName))
                {
                    AttributeValue attValue = resource.Attributes[attributeName];

                    if (attValue.Attribute.SystemName.Equals("ObjectID") || attValue.Attribute.SystemName.Equals("ObjectType"))
                    {
                        continue;
                    }

                    DSAttribute dsAttribute = new DSAttribute
                    {
                        Description = attValue.Attribute.Description,
                        DisplayName = attValue.Attribute.DisplayName,
                        IsMultivalued = attValue.Attribute.IsMultivalued,
                        IsReadOnly = attValue.Attribute.IsReadOnly,
                        IsRequired = attValue.Attribute.IsRequired,
                        Regex = attValue.Attribute.Regex,
                        SystemName = attValue.Attribute.SystemName,
                        Type = attValue.Attribute.Type.ToString(),
                        IsNull = attValue.IsNull,
                        PermissionHint = attValue.PermissionHint.ToString(),
                        Value = attValue.Value,
                        Values = attValue.Values.ToList()
                    };

                    if (attributeDef != null)
                    {
                        RmAttribute attr = attributeDef.FirstOrDefault(a => a.Name.Equals(attValue.AttributeName));
                        if (attr != null)
                        {
                            dsAttribute.DisplayName = attr.DisplayName;
                            dsAttribute.Description = attr.Description;
                        }
                    }

                    if (!dsAttribute.IsNull &&  dsAttribute.Type.Equals("Reference"))
                    {
                        dsAttribute.Value = attValue.Attribute.IsMultivalued ? 
                            attValue.StringValues.FirstOrDefault() : attValue.StringValue;
                        dsAttribute.Values = attValue.Attribute.IsMultivalued ? 
                            attValue.StringValues.ToList<object>() : new List<object>() { attValue.StringValue };

                        if (!string.IsNullOrEmpty(dsAttribute.Value.ToString()) && deepResolve && option.ResolveID)
                        {
                            if (dsAttribute.IsMultivalued)
                            {
                                foreach (string value in attValue.StringValues)
                                {
                                    ResourceObject resolvedObject = client.GetResource(
                                        value, option.AttributesToResolve, includePermission);
                                    dsAttribute.ResolvedValues.Add(convertToDSResource(client,
                                        resolvedObject, option.AttributesToResolve, includePermission, option, option.DeepResolve));
                                }
                            }
                            else
                            {
                                ResourceObject resolvedObject = client.GetResource(
                                    attValue.StringValue, option.AttributesToResolve, includePermission);
                                dsAttribute.ResolvedValue = convertToDSResource(client,
                                    resolvedObject, option.AttributesToResolve, includePermission, option, option.DeepResolve);
                            }
                        }
                    }
                    
                    attributes.Add(attValue.AttributeName, dsAttribute);
                }
            }

            dsResource.Attributes = attributes;

            return dsResource;
        }

        public new string GetType()
        {
            return "MIM Resource Repository";
        }

        public DSResource GetResourceByID(
            string id, string[] attributes, bool includePermission = false, ResourceOption resourceOption = null)
        {
            ResourceOption option = resourceOption == null ? new ResourceOption() : resourceOption;

            ResourceManagementClient client = getClient(option.ConnectionInfo);
            client.RefreshSchema();

            ResourceObject resource = client.GetResource(id, attributes, includePermission);

            return convertToDSResource(client, resource, attributes, includePermission, resourceOption);
        }

        public DSResourceSet GetResourceByQuery(string query, string[] attributes, 
            int pageSize = 0, int index = 0, ResourceOption resourceOption = null)
        {
            ResourceOption option = resourceOption == null ? new ResourceOption() : resourceOption;

            ResourceManagementClient client = getClient(option.ConnectionInfo);
            client.RefreshSchema();

            DSResourceSet retVal = new DSResourceSet();

            List<SortingAttribute> sortingAttributes = getSortingAttributes(resourceOption.SortingAttributes);

            if (pageSize == 0)
            {
                SearchResultCollection src = sortingAttributes.Count == 0 ? 
                    client.GetResources(query, attributes) as SearchResultCollection : 
                    client.GetResources(query, attributes, sortingAttributes) as SearchResultCollection;

                if (src != null)
                {
                    retVal.TotalCount = src.Count;
                    foreach (ResourceObject resource in src)
                    {
                        retVal.Resources.Add(convertToDSResource(client, resource, attributes, false, resourceOption));
                    }
                }
            }
            else
            {
                SearchResultPager srp = sortingAttributes.Count == 0 ? 
                    client.GetResourcesPaged(query, pageSize, attributes) : 
                    client.GetResourcesPaged(query, pageSize, attributes, sortingAttributes);

                if (index >= 0)
                {
                    srp.CurrentIndex = index;
                }

                srp.PageSize = pageSize;

                foreach (ResourceObject resource in srp.GetNextPage())
                {
                    retVal.Resources.Add(convertToDSResource(client, resource, attributes, false, resourceOption));
                }

                retVal.TotalCount = srp.TotalCount;
                retVal.HasMoreItems = srp.HasMoreItems;
            }

            return retVal;
        }
    }
}
