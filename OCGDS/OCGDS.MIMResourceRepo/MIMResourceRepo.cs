using Lithnet.ResourceManagement.Client;
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

        public DSResource convertToDSResource(ResourceObject resource)
        {
            DSResource dsResource = new DSResource
            {
                DisplayName = resource.DisplayName,
                ObjectID = resource.ObjectID.Value,
                ObjectType = resource.ObjectTypeName,
                HasPermissionHints = resource.HasPermissionHints
            };

            Dictionary<string, DSAttribute> attributes = new Dictionary<string, DSAttribute>();
            foreach (AttributeValue attValue in resource.Attributes)
            {
                if (!attValue.IsNull)
                {
                    attributes.Add(attValue.AttributeName, new DSAttribute
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
                    });
                }
            }

            dsResource.Attributes = attributes;

            return dsResource;
        }

        public new string GetType()
        {
            return "MIM Resource Repository";
        }

        public DSResource GetResourceByID(ConnectionInfo info, string id, string[] attributes, bool getPermission, bool getResolved)
        {
            ResourceManagementClient client = getClient(info);
            client.RefreshSchema();

            ResourceObject resource = client.GetResource(id, attributes, getPermission);

            return convertToDSResource(resource);
        }
    }
}
