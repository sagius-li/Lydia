using Lithnet.ResourceManagement.Client;
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

        public DSResource convertToDSResource(ResourceManagementClient client, ResourceObject resource, bool includePermission, ResourceOption option)
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
            foreach (AttributeValue attValue in resource.Attributes)
            {
                if (attValue.Attribute.SystemName.Equals("ObjectID") || attValue.Attribute.SystemName.Equals("ObjectType"))
                {
                    continue;
                }

                if (!attValue.IsNull)
                {
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
                        Values = attValue.Attribute.IsMultivalued ? attValue.Values.ToList() : null
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

                    if (option.ResolveID && dsAttribute.Type.Equals("Reference"))
                    {
                        if (dsAttribute.IsMultivalued)
                        {

                        }
                        else
                        {
                            ResourceObject resolvedObject = client.GetResource(attValue.StringValue, option.AttributesToResolve, includePermission);
                            dsAttribute.ResolvedValue = convertToDSResource(client, resolvedObject, includePermission, option);
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

            return convertToDSResource(client, resource, includePermission, resourceOption);
        }
    }
}
