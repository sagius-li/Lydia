using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel.ResourceTypes
{
    public class RmPermission : RmResource
    {
        private RmList<RmReference> _clientRefs;
        private RmList<RmReference> _sharePointGroupRefs;

        public RmPermission()
            : base()
        {

        }

        public sealed new class AttributeNames
        {
            public static RmAttributeName PermissionId = new RmAttributeName(@"ocgPermissionId");
            public static RmAttributeName PermissionType = new RmAttributeName(@"ocgPermissionType");
            public static RmAttributeName ClientRefs = new RmAttributeName(@"ocgClientRefs");
            public static RmAttributeName FromSharePoint = new RmAttributeName(@"ocgFromSharePoint");
            public static RmAttributeName SharePointGroupRefs = new RmAttributeName(@"ocgSharePointGroupRefs");
            public static RmAttributeName ResourceChanged = new RmAttributeName(@"ocgResourceChanged");
            public static RmAttributeName ResourceCreated = new RmAttributeName(@"ocgResourceCreated");
        }

        protected override void EnsureAllAttributesExist()
        {
            base.EnsureAllAttributesExist();

            base.EnsureAttributeExists(AttributeNames.PermissionId);
            base.EnsureAttributeExists(AttributeNames.PermissionType);
            base.EnsureAttributeExists(AttributeNames.ClientRefs);
            base.EnsureAttributeExists(AttributeNames.FromSharePoint);
            base.EnsureAttributeExists(AttributeNames.SharePointGroupRefs);
            base.EnsureAttributeExists(AttributeNames.ResourceChanged);
            base.EnsureAttributeExists(AttributeNames.ResourceCreated);
        }

        protected const String ResourceType = @"ocgPermission";

        public override string GetResourceType()
        {
            return RmPermission.ResourceType;
        }

        public static string StaticResourceType()
        {
            return RmPermission.ResourceType;
        }

        #region promoted properties

        public String PermissionId
        {
            get
            {
                return GetString(AttributeNames.PermissionId);
            }
            set
            {
                base[AttributeNames.PermissionId].Value = value;
            }
        }

        public String PermissionType
        {
            get
            {
                return GetString(AttributeNames.PermissionType);
            }
            set
            {
                base[AttributeNames.PermissionType].Value = value;
            }
        }

        public IList<RmReference> ClientRefs
        {
            get
            {
                if (this._clientRefs == null)
                {
                    this._clientRefs = GetMultiValuedReference(AttributeNames.ClientRefs);
                    return this._clientRefs;
                }
                else
                {
                    return this._clientRefs;
                }
            }
        }

        public bool FromSharePoint
        {
            get
            {
                return GetBoolean(AttributeNames.FromSharePoint);
            }
            set
            {
                base[AttributeNames.FromSharePoint].Value = value;
            }
        }

        public IList<RmReference> SharePointGroupRefs
        {
            get
            {
                if (this._sharePointGroupRefs == null)
                {
                    this._sharePointGroupRefs = GetMultiValuedReference(AttributeNames.SharePointGroupRefs);
                    return this._sharePointGroupRefs;
                }
                else
                {
                    return this._sharePointGroupRefs;
                }
            }
        }

        public bool ResourceChanged
        {
            get
            {
                return GetBoolean(AttributeNames.ResourceChanged);
            }
            set
            {
                base[AttributeNames.ResourceChanged].Value = value;
            }
        }

        public bool ResourceCreated
        {
            get
            {
                return GetBoolean(AttributeNames.ResourceCreated);
            }
            set
            {
                base[AttributeNames.ResourceCreated].Value = value;
            }
        }

        #endregion
    }
}
