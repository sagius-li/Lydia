using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OCG.ResourceManagement.ObjectModel;
using OCG.ResourceManagement.ObjectModel.ResourceTypes;

namespace OCG.ResourceManagement.Control.HeritanceControl
{
    public class GenericControl : ResourceControl
    {
        public GenericControl(ClientControl client)
            : base(client)
        {

        }

        #region Override Functions

        public List<RmResource> GetAllResource(string type, string[] attributes)
        {
            return Base_GetAllResource(type, attributes);
        }

        public RmResource GetResourceByDisplayName(string type, string displayName, string[] attributes)
        {
            return Base_GetResourceByDisplayName(type, displayName, attributes);
        }

        public RmResource GetResourceById(string type, string objectId, string[] attributes)
        {
            return Base_GetResourceById(type, objectId, attributes);
        }

        public List<RmResource> GetResourceByAttribute(string type, string attributeName, string value, OperationType operation, string[] attributes)
        {
            return Base_GetResourceByAttribute(type, attributeName, value, operation, attributes);
        }

        public List<RmResource> GetResourceByQuery(string type, string query, string[] attributes)
        {
            return Base_GetResourceByQuery(type, query, attributes);
        }

        public bool modifyResourceAttribute(RmResource resource, RmAttributeName attName, RmAttributeValue attValue, 
            bool overwriteMultivalue = false, bool allowOverwrite = true, bool allowNull = true)
        {
            bool modified = false;

            if (resource == null || attName == null || string.IsNullOrEmpty(attName.Name))
            {
                return modified;
            }

            if (attValue == null || attValue.Value == null)
            {
                if (allowOverwrite && allowNull)
                {
                    if (resource.ContainsKey(attName))
                    {
                        resource.Remove(attName);
                        modified = true;
                    }
                }
            }
            else
            {
                if (resource.ContainsKey(attName))
                {
                    if (attValue.IsMultiValue)
                    {
                        if (resource[attName].Values == null || resource[attName].Values.Count == 0)
                        {
                            resource[attName] = attValue;
                            modified = true;
                        }
                        else
                        {
                            if (overwriteMultivalue)
                            {
                                modified = true;

                                resource[attName].Value = attValue.Value;
                                resource[attName].Values = attValue.Values;
                            }
                            else if (allowOverwrite)
                            {
                                // Compare the multivalues
                                if (resource[attName].Values.Where(v1 => !attValue.Values.Exists(v2 => v2.ToString().Equals(v1.ToString()))).Union(
                                    attValue.Values.Where(v1 => !resource[attName].Values.Exists(v2 => v2.ToString().Equals(v1.ToString())))).Count() != 0)
                                {
                                    modified = true;
                                }

                                resource[attName].Values.RemoveAll(v1 => !attValue.Values.Exists(v2 => v2.ToString().Equals(v1.ToString())));

                                resource[attName].Values.AddRange(
                                    attValue.Values.Where(v1 => !resource[attName].Values.Exists(v2 => v2.ToString().Equals(v1.ToString()))));
                            }
                        }
                    }
                    else
                    {
                        if (resource[attName].Value == null || string.IsNullOrEmpty(resource[attName].Value.ToString()))
                        {
                            resource[attName] = attValue;
                            modified = true;
                        }
                        else
                        {
                            if (allowOverwrite)
                            {
                                if (!resource[attName].Value.Equals(attValue.Value))
                                {
                                    resource[attName].Value = attValue.Value;
                                    modified = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    resource.Add(attName, attValue);
                    modified = true;
                }
            }

            return modified;
        }

        #endregion
    }
}
