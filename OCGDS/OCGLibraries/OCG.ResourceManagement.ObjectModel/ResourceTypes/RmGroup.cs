using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel.ResourceTypes
{
    public class RmGroup : RmResource
    {
        public RmGroup()
            : base()
        {
        }

        private RmList<RmReference> _roleRefList;
        private RmList<RmReference> _permissionRefList;
        private List<string> _blockedRoles;
        private List<string> _conflictRoles;

        protected const String ResourceType = @"Group";

        RmList<RmReference> explicitMember;
        RmList<RmReference> computedMember;

        protected override void EnsureAllAttributesExist()
        {
            base.EnsureAllAttributesExist();
            base.EnsureAttributeExists(AttributeNames.AccountName);
            base.EnsureAttributeExists(AttributeNames.ComputedMember);
            base.EnsureAttributeExists(AttributeNames.DisplayedOwner);
            base.EnsureAttributeExists(AttributeNames.Domain);
            base.EnsureAttributeExists(AttributeNames.DomainConfiguration);
            base.EnsureAttributeExists(AttributeNames.Email);
            base.EnsureAttributeExists(AttributeNames.ExplicitMember);
            base.EnsureAttributeExists(AttributeNames.Filter);
            base.EnsureAttributeExists(AttributeNames.MailNickname);
            base.EnsureAttributeExists(AttributeNames.MembershipAddWorkflow);
            base.EnsureAttributeExists(AttributeNames.MembershipLocked);
            base.EnsureAttributeExists(AttributeNames.MemberToAdd);
            base.EnsureAttributeExists(AttributeNames.MemberToRemove);
            base.EnsureAttributeExists(AttributeNames.ObjectSID);
            base.EnsureAttributeExists(AttributeNames.Owner);
            base.EnsureAttributeExists(AttributeNames.Scope);
            base.EnsureAttributeExists(AttributeNames.Type);
            base.EnsureAttributeExists(AttributeNames.RoleRefList);
            base.EnsureAttributeExists(AttributeNames.PermissionRefList);
            base.EnsureAttributeExists(AttributeNames.ConflictRoles);
            base.EnsureAttributeExists(AttributeNames.BlockedRoles);
            base.EnsureAttributeExists(AttributeNames.BreakInheritance);
            base.EnsureAttributeExists(AttributeNames.FromSharePoint);
            base.EnsureAttributeExists(AttributeNames.SharePointContext);
            base.EnsureAttributeExists(AttributeNames.SharePointGroupID);
            base.EnsureAttributeExists(AttributeNames.SharePointID);
            base.EnsureAttributeExists(AttributeNames.ResourceChanged);
            base.EnsureAttributeExists(AttributeNames.ResourceCreated);
            base.EnsureAttributeExists(AttributeNames.IsPermission);
        }

        public override string GetResourceType()
        {
            return RmGroup.ResourceType;
        }

        public static string StaticResourceType()
        {
            return RmGroup.ResourceType;
        }

        #region promoted properties

        public RmReference Owner
        {
            get
            {
                return GetReference(AttributeNames.Owner);
            }
            set
            {
                base[AttributeNames.Owner].Value = value;
            }
        }

        public RmReference DisplayedOwner
        {
            get
            {
                return GetReference(AttributeNames.DisplayedOwner);
            }
            set
            {
                base[AttributeNames.DisplayedOwner].Value = value;
            }
        }

        public IList<RmReference> ComputedMember
        {
            get
            {
                if (this.computedMember == null)
                {
                    lock (base.Attributes)
                    {
                        this.computedMember = GetMultiValuedReference(AttributeNames.ComputedMember);
                        return this.computedMember;
                    }
                }
                else
                {
                    return this.computedMember;
                }
            }
        }

        public IList<RmReference> ExplicitMember
        {
            get
            {
                if (this.explicitMember == null)
                {
                    this.explicitMember = GetMultiValuedReference(AttributeNames.ExplicitMember);
                    return this.explicitMember;
                }
                else
                {
                    return this.explicitMember;
                }
            }
        }

        public String Filter
        {
            get
            {
                return GetString(AttributeNames.Filter);
            }
            set
            {
                this[AttributeNames.Filter].Value = value;
            }
        }

        public String AccountName
        {
            get
            {
                return GetString(AttributeNames.AccountName);
            }
            set
            {
                this[AttributeNames.AccountName].Value = value;
            }
        }

        public String Domain
        {
            get
            {
                return GetString(AttributeNames.Domain);
            }
            set
            {
                base[AttributeNames.Domain].Value = value;
            }
        }

        public String Email
        {
            get
            {
                return GetString(AttributeNames.Email);
            }
            set
            {
                base[AttributeNames.Email].Value = value;
            }
        }

        public String MailNickname
        {
            get
            {
                return GetString(AttributeNames.MailNickname);
            }
            set
            {
                base[AttributeNames.MailNickname].Value = value;
            }
        }

        public bool MembershipLocked
        {
            get
            {
                return GetBoolean(AttributeNames.MembershipLocked);
            }
            set
            {
                base[AttributeNames.MembershipLocked].Value = value;
            }
        }

        public RmGroupScope Scope
        {
            get
            {
                Object o = null;
                RmAttributeValue rma = null;
                base.TryGetValue(AttributeNames.Scope, out rma);
                if (rma != null)
                    o = rma.Value;
                if (o == null)
                {
                    return RmGroupScope.Domain;
                }
                else
                {
                    return (RmGroupScope)Enum.Parse(typeof(RmGroupScope), o.ToString());
                }
            }
            set
            {
                base[AttributeNames.Scope].Value = value;
            }
        }

        public RmGroupType Type
        {
            get
            {
                Object o = null;
                RmAttributeValue rma = null;
                base.TryGetValue(AttributeNames.Type, out rma);
                if (rma != null)
                    o = rma.Value;
                if (o == null)
                {
                    return RmGroupType.Distribution;
                }
                else
                {
                    return (RmGroupType)Enum.Parse(typeof(RmGroupType), o.ToString(), true);
                }
            }
            set
            {
                base[AttributeNames.Type].Value = value;
            }
        }

        public IList<RmReference> RoleRefList
        {
            get
            {
                if (this._roleRefList == null)
                {
                    this._roleRefList = GetMultiValuedReference(AttributeNames.RoleRefList);
                    return this._roleRefList;
                }
                else
                {
                    return this._roleRefList;
                }
            }
        }

        public IList<RmReference> PermissionRefList
        {
            get
            {
                if (this._permissionRefList == null)
                {
                    this._permissionRefList = GetMultiValuedReference(AttributeNames.PermissionRefList);
                    return this._permissionRefList;
                }
                else
                {
                    return this._permissionRefList;
                }
            }
        }

        public List<string> ConflictRoles
        {
            get
            {
                if (this._conflictRoles == null)
                {
                    this._conflictRoles = GetMultiValuedString(AttributeNames.ConflictRoles);
                    return this._conflictRoles;
                }
                else
                {
                    return this._conflictRoles;
                }
            }
        }

        public List<string> BlockedRoles
        {
            get
            {
                if (this._blockedRoles == null)
                {
                    this._blockedRoles = GetMultiValuedString(AttributeNames.BlockedRoles);
                    return this._blockedRoles;
                }
                else
                {
                    return this._blockedRoles;
                }
            }
        }

        public bool BreakInheritance
        {
            get
            {
                return GetBoolean(AttributeNames.BreakInheritance);
            }
            set
            {
                base[AttributeNames.BreakInheritance].Value = value;
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

        public String SharePointGroupID
        {
            get
            {
                return GetString(AttributeNames.SharePointGroupID);
            }
            set
            {
                base[AttributeNames.SharePointGroupID].Value = value;
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

        public bool IsPermission
        {
            get
            {
                return GetBoolean(AttributeNames.IsPermission);
            }
            set
            {
                base[AttributeNames.IsPermission].Value = value;
            }
        }

        #endregion

        public sealed new class AttributeNames
        {
            public static RmAttributeName AccountName = new RmAttributeName(@"AccountName");
            public static RmAttributeName ComputedMember = new RmAttributeName(@"ComputedMember");
            public static RmAttributeName DisplayedOwner = new RmAttributeName(@"DisplayedOwner");
            public static RmAttributeName Domain = new RmAttributeName(@"Domain");
            public static RmAttributeName DomainConfiguration = new RmAttributeName(@"DomainConfiguration");
            public static RmAttributeName Email = new RmAttributeName(@"Email");
            public static RmAttributeName ExplicitMember = new RmAttributeName(@"ExplicitMember");
            public static RmAttributeName Filter = new RmAttributeName(@"Filter");
            public static RmAttributeName MailNickname = new RmAttributeName(@"MailNickname");
            public static RmAttributeName MemberToAdd = new RmAttributeName(@"MemberToAdd");
            public static RmAttributeName MemberToRemove = new RmAttributeName(@"MemberToRemove");
            public static RmAttributeName MembershipAddWorkflow = new RmAttributeName(@"MembershipAddWorkflow");
            public static RmAttributeName MembershipLocked = new RmAttributeName(@"MembershipLocked");
            public static RmAttributeName ObjectSID = new RmAttributeName(@"ObjectSID");
            public static RmAttributeName Owner = new RmAttributeName(@"Owner");
            public static RmAttributeName Scope = new RmAttributeName(@"Scope");
            public static RmAttributeName Type = new RmAttributeName(@"Type");

            public static RmAttributeName RoleRefList = new RmAttributeName(@"ocgRoleRefList");
            public static RmAttributeName PermissionRefList = new RmAttributeName(@"ocgPermissionRefList");

            public static RmAttributeName ConflictRoles = new RmAttributeName(@"ocgConflictRoleList");
            public static RmAttributeName BlockedRoles = new RmAttributeName(@"ocgBlockedRoleList");
            public static RmAttributeName BreakInheritance = new RmAttributeName(@"ocgBreakInheritance");

            public static RmAttributeName FromSharePoint = new RmAttributeName(@"ocgFromSharePoint");
            public static RmAttributeName SharePointGroupID = new RmAttributeName(@"ocgSharePointGroupID");
            public static RmAttributeName SharePointID = new RmAttributeName(@"ocgSharePointID");
            public static RmAttributeName SharePointContext = new RmAttributeName(@"ocgSharePointContext");

            public static RmAttributeName ResourceChanged = new RmAttributeName(@"ocgResourceChanged");
            public static RmAttributeName ResourceCreated = new RmAttributeName(@"ocgResourceCreated");

            public static RmAttributeName IsPermission = new RmAttributeName(@"ocgIsPermission");
        }
    }

    public enum RmGroupScope
    {
        Global,
        Universal,
        Domain
    }

    public enum RmGroupType
    {
        Distribution = 1,
        Security = 2,
    }
}
