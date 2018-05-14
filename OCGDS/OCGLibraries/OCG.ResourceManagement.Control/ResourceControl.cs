using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OCG.ResourceManagement.ObjectModel;

namespace OCG.ResourceManagement.Control
{
    public class ResourceControl
    {
        #region Public Member

        public enum OperationType
        {
            None = 0, 
            Opration_Is, 
            Operation_Contain, 
            Operation_StartWith
        }

        public ClientControl Client { get; set; }

        #endregion

        #region Constructor

        public ResourceControl(ClientControl client)
        {
            Client = client;
        }

        #endregion

        #region Functions to be overriden

        protected List<RmResource> Base_GetAllResource(string resourceType, string[] attributes)
        {
            List<RmResource> retVal = new List<RmResource>();

            if (!Client.SchemaCached)
            {
                Client.RefreshSchema();
            }

            string filter = string.Format("/{0}", resourceType);

            if (attributes.Length == 0)
            {
                foreach (RmResource resource in Client.Enumerate(filter))
                {
                    retVal.Add(resource);
                }
            }
            else
            {
                foreach (RmResource resource in Client.Enumerate(filter, attributes))
                {
                    retVal.Add(resource);
                }
            }

            return retVal;
        }

        protected RmResource Base_GetResourceByDisplayName(string sourceType, string displayName, string[] attributes)
        {
            RmResource retVal = null;

            if (!Client.SchemaCached)
            {
                Client.RefreshSchema();
            }

            string filter = string.Format("/{0}[{1}='{2}']", sourceType, RmResource.AttributeNames.DisplayName, displayName);

            if (attributes.Length == 0)
            {
                foreach (RmResource resource in Client.Enumerate(filter))
                {
                    retVal = resource;
                    break;
                }
            }
            else
            {
                foreach (RmResource resource in Client.Enumerate(filter, attributes))
                {
                    retVal = resource;
                    break;
                }
            }

            return retVal;
        }

        protected RmResource Base_GetResourceById(string sourceType, string objectId, string[] attributes)
        {
            RmResource retVal = null;

            if (!Client.SchemaCached)
            {
                Client.RefreshSchema();
            }

            string filter = string.Format("/{0}[{1}='{2}']", sourceType, RmResource.AttributeNames.ObjectID.Name, objectId);

            if (attributes.Length == 0)
            {
                foreach (RmResource resource in Client.Enumerate(filter))
                {
                    retVal = resource;
                    break;
                }
            }
            else
            {
                foreach (RmResource resource in Client.Enumerate(filter, attributes))
                {
                    retVal = resource;
                    break;
                }
            }

            return retVal;

            //if (!Client.SchemaCached)
            //{
            //    Client.RefreshSchema();
            //}

            //if (attributes.Length == 0)
            //{
            //    return Client.Get(new RmReference(objectId));
            //}
            //else
            //{
            //    return Client.Get(new RmReference(objectId), attributes);
            //}
        }

        protected List<RmResource> Base_GetResourceByAttribute(string resourceType, string attributeName, string value, OperationType operation, string[] attributes)
        {
            List<RmResource> retVal = new List<RmResource>();

            if (string.IsNullOrEmpty(attributeName) || operation == OperationType.None)
            {
                return retVal;
            }

            if (!Client.SchemaCached)
            {
                Client.RefreshSchema();
            }

            string condition = string.Empty;
            switch (operation)
            {
                case OperationType.Opration_Is:
                    condition = string.Format("[{0}='{1}']", attributeName, value);
                    break;
                case OperationType.Operation_Contain:
                    condition = string.Format("[contains({0},'{1}')]", attributeName, value);
                    break;
                case OperationType.Operation_StartWith:
                    condition = string.Format("[starts-with({0},'{1}')]", attributeName, value);
                    break;
                case OperationType.None:
                default:
                    return retVal;
            }

            string filter = string.Format("/{0}{1}", resourceType, condition);

            if (attributes.Length == 0)
            {
                foreach (RmResource resource in Client.Enumerate(filter))
                {
                    retVal.Add(resource);
                }
            }
            else
            {
                foreach (RmResource resource in Client.Enumerate(filter, attributes))
                {
                    retVal.Add(resource);
                }
            }

            return retVal;
        }

        protected List<RmResource> Base_GetResourceByQuery(string resourceType, string query, string[] attributes)
        {
            List<RmResource> retVal = new List<RmResource>();

            if (!Client.SchemaCached)
            {
                Client.RefreshSchema();
            }

            string filter = string.Format("/{0}{1}", resourceType, query);

            if (attributes.Length == 0)
            {
                foreach (RmResource resource in Client.Enumerate(filter))
                {
                    retVal.Add(resource);
                }
            }
            else
            {
                foreach (RmResource resource in Client.Enumerate(filter, attributes))
                {
                    retVal.Add(resource);
                }
            }

            return retVal;
        }

        protected RmReference Base_CreateResource(RmResource resourceToCreate)
        {
            if (!Client.SchemaCached)
            {
                Client.RefreshSchema();
            }

            return Client.Create(resourceToCreate);
        }

        #endregion

        #region General Functions

        public RmReference CreateResource(RmResource resource)
        {
            if (!Client.SchemaCached)
            {
                Client.RefreshSchema();
            }

            try
            {
                return Client.Create(resource);
            }
            catch
            {
                return null;
            }
        }

        public bool UpdateResource(RmResourceChanges transaction)
        {
            if (!Client.SchemaCached)
            {
                Client.RefreshSchema();
            }

            bool retVal = Client.Put(transaction);

            transaction.AcceptChanges();

            return retVal;
        }

        public bool DeleteResource(RmReference objectId)
        {
            if (!Client.SchemaCached)
            {
                Client.RefreshSchema();
            }

            return Client.Delete(objectId);
        }

        #endregion
    }
}
