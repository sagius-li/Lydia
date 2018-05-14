using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel.ResourceTypes
{
    public class RmRole : RmResource
    {
        private RmList<RmReference> _premissionRefs;

        public RmRole()
            : base()
        {

        }

        protected const String ResourceType = @"ocgRole";

        public sealed new class AttributeNames
        {
            public static RmAttributeName RoleId = new RmAttributeName(@"ocgRoleId");
            public static RmAttributeName RoleType = new RmAttributeName(@"ocgRoleType");
            public static RmAttributeName ParentRef = new RmAttributeName(@"ocgParentRef");
            public static RmAttributeName PremissionRefs = new RmAttributeName(@"ocgPermissionRefs");
            public static RmAttributeName FromSharePoint = new RmAttributeName(@"ocgFromSharePoint");
            public static RmAttributeName SharePointID = new RmAttributeName(@"ocgSharePointID");
            public static RmAttributeName SharePointContext = new RmAttributeName(@"ocgSharePointContext");
            public static RmAttributeName ResourceChanged = new RmAttributeName(@"ocgResourceChanged");
            public static RmAttributeName ResourceCreated = new RmAttributeName(@"ocgResourceCreated");
        }

        protected override void EnsureAllAttributesExist()
        {
            base.EnsureAllAttributesExist();

            base.EnsureAttributeExists(AttributeNames.RoleId);
            base.EnsureAttributeExists(AttributeNames.ParentRef);
            base.EnsureAttributeExists(AttributeNames.RoleType);
            base.EnsureAttributeExists(AttributeNames.PremissionRefs);
            base.EnsureAttributeExists(AttributeNames.FromSharePoint);
            base.EnsureAttributeExists(AttributeNames.SharePointContext);
            base.EnsureAttributeExists(AttributeNames.SharePointID);
            base.EnsureAttributeExists(AttributeNames.ResourceChanged);
            base.EnsureAttributeExists(AttributeNames.ResourceCreated);
        }

        public override string GetResourceType()
        {
            return RmRole.ResourceType;
        }

        public static string StaticResourceType()
        {
            return RmRole.ResourceType;
        }

        #region promoted properties

        public String RoleId
        {
            get
            {
                return GetString(AttributeNames.RoleId);
            }
            set
            {
                base[AttributeNames.RoleId].Value = value;
            }
        }

        public String RoleType
        {
            get
            {
                return GetString(AttributeNames.RoleType);
            }
            set
            {
                base[AttributeNames.RoleType].Value = value;
            }
        }

        public RmReference ParentRef
        {
            get
            {
                RmReference parent = GetReference(AttributeNames.ParentRef);
                return parent;
            }
            set
            {
                base[AttributeNames.ParentRef].Value = value;
            }
        }

        public IList<RmReference> PremissionRefs
        {
            get
            {
                if (this._premissionRefs == null)
                {
                    this._premissionRefs = GetMultiValuedReference(AttributeNames.PremissionRefs);
                    return this._premissionRefs;
                }
                else
                {
                    return this._premissionRefs;
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

        public String SharePointID
        {
            get
            {
                return GetString(AttributeNames.SharePointID);
            }
            set
            {
                base[AttributeNames.SharePointID].Value = value;
            }
        }

        public String SharePointContext
        {
            get
            {
                return GetString(AttributeNames.SharePointContext);
            }
            set
            {
                base[AttributeNames.SharePointContext].Value = value;
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
