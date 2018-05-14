using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel.ResourceTypes
{
    public class RmOrgUnit : RmResource
    {
        private List<string> _conflictRoles;
        private RmList<RmReference> _roleRefList;

        public RmOrgUnit()
            : base()
        {

        }

        protected const String ResourceType = @"ocgOrgUnit";

        public sealed new class AttributeNames
        {
            public static RmAttributeName UnitId = new RmAttributeName(@"ocgOrgUnitId");
            public static RmAttributeName ParentRef = new RmAttributeName(@"ocgParentRef");
            public static RmAttributeName Department = new RmAttributeName(@"ocgOuDepartment");
            public static RmAttributeName HomeShare = new RmAttributeName(@"ocgOuHomeShare");
            public static RmAttributeName ProjectShare = new RmAttributeName(@"ocgOuProjectShare");
            public static RmAttributeName DepartmentShare = new RmAttributeName(@"ocgOuDepartmentShare");
            public static RmAttributeName ExchangeDb = new RmAttributeName(@"ocgOuExchangeDb");
            public static RmAttributeName Telephone = new RmAttributeName(@"ocgOuTelephone");
            public static RmAttributeName HomeDrive = new RmAttributeName(@"ocgHomeDrive");
            public static RmAttributeName ProfilePath = new RmAttributeName(@"ocgProfilePath");
            public static RmAttributeName HomeMDB = new RmAttributeName(@"ocgHomeMDB");
            public static RmAttributeName LogonScript = new RmAttributeName(@"ocgLogonScript");
            public static RmAttributeName MailNickName = new RmAttributeName(@"MailNickname");
            public static RmAttributeName Room = new RmAttributeName(@"ocgRoom");
            public static RmAttributeName BreakInheritance = new RmAttributeName(@"ocgBreakInheritance");
            public static RmAttributeName ConflictRoles = new RmAttributeName(@"ocgConflictRoleList");
            public static RmAttributeName RoleRefList = new RmAttributeName(@"ocgRoleRefList");
            public static RmAttributeName Company = new RmAttributeName(@"Company");
            public static RmAttributeName FromSharePoint = new RmAttributeName(@"ocgFromSharePoint");
            public static RmAttributeName SharePointID = new RmAttributeName(@"ocgSharePointID");
            public static RmAttributeName SharePointContext = new RmAttributeName(@"ocgSharePointContext");
            public static RmAttributeName SharePointRootWeb = new RmAttributeName(@"ocgSharePointRootWeb");
            public static RmAttributeName SharePoionRelativeUrl = new RmAttributeName(@"ocgSharePointRelativeUrl");
            public static RmAttributeName SharePointCompleteUrl = new RmAttributeName(@"ocgSharePointCompleteUrl");
            public static RmAttributeName ResourceChanged = new RmAttributeName(@"ocgResourceChanged");
            public static RmAttributeName ResourceCreated = new RmAttributeName(@"ocgResourceCreated");
            public static RmAttributeName AttributeChanged = new RmAttributeName(@"ocgAttributeChanged");
            public static RmAttributeName IsProfile = new RmAttributeName(@"ocgIsProfile");
            public static RmAttributeName IsParent = new RmAttributeName(@"OCGisParent");
        }

        protected override void EnsureAllAttributesExist()
        {
            base.EnsureAllAttributesExist();

            base.EnsureAttributeExists(AttributeNames.UnitId);
            base.EnsureAttributeExists(AttributeNames.ParentRef);
            base.EnsureAttributeExists(AttributeNames.Department);
            base.EnsureAttributeExists(AttributeNames.HomeShare);
            base.EnsureAttributeExists(AttributeNames.ProjectShare);
            base.EnsureAttributeExists(AttributeNames.Telephone);
            base.EnsureAttributeExists(AttributeNames.DepartmentShare);
            base.EnsureAttributeExists(AttributeNames.ExchangeDb);
            base.EnsureAttributeExists(AttributeNames.HomeDrive);
            base.EnsureAttributeExists(AttributeNames.ProfilePath);
            base.EnsureAttributeExists(AttributeNames.HomeMDB);
            base.EnsureAttributeExists(AttributeNames.LogonScript);
            base.EnsureAttributeExists(AttributeNames.MailNickName);
            base.EnsureAttributeExists(AttributeNames.Room);
            base.EnsureAttributeExists(AttributeNames.BreakInheritance);
            base.EnsureAttributeExists(AttributeNames.ConflictRoles);
            base.EnsureAttributeExists(AttributeNames.RoleRefList);
            base.EnsureAttributeExists(AttributeNames.Company);
            base.EnsureAttributeExists(AttributeNames.FromSharePoint);
            base.EnsureAttributeExists(AttributeNames.SharePointID);
            base.EnsureAttributeExists(AttributeNames.SharePointContext);
            base.EnsureAttributeExists(AttributeNames.SharePointCompleteUrl);
            base.EnsureAttributeExists(AttributeNames.SharePointRootWeb);
            base.EnsureAttributeExists(AttributeNames.SharePoionRelativeUrl);
            base.EnsureAttributeExists(AttributeNames.ResourceChanged);
            base.EnsureAttributeExists(AttributeNames.ResourceCreated);
            base.EnsureAttributeExists(AttributeNames.AttributeChanged);
            base.EnsureAttributeExists(AttributeNames.IsProfile);
            base.EnsureAttributeExists(AttributeNames.IsParent);
        }

        public override string GetResourceType()
        {
            return RmOrgUnit.ResourceType;
        }

        public static string StaticResourceType()
        {
            return RmOrgUnit.ResourceType;
        }

        #region promoted properties

        public String UnitId
        {
            get
            {
                return GetString(AttributeNames.UnitId);
            }
            set
            {
                base[AttributeNames.UnitId].Value = value;
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

        public String ExchangeDb
        {
            get
            {
                return GetString(AttributeNames.ExchangeDb);
            }
            set
            {
                base[AttributeNames.ExchangeDb].Value = value;
            }
        }

        public String Department
        {
            get
            {
                return GetString(AttributeNames.Department);
            }
            set
            {
                base[AttributeNames.Department].Value = value;
            }
        }

        public String DepartmentShare
        {
            get
            {
                return GetString(AttributeNames.DepartmentShare);
            }
            set
            {
                base[AttributeNames.DepartmentShare].Value = value;
            }
        }

        public String HomeShare
        {
            get
            {
                return GetString(AttributeNames.HomeShare);
            }
            set
            {
                base[AttributeNames.HomeShare].Value = value;
            }
        }

        public String ProjectShare
        {
            get
            {
                return GetString(AttributeNames.ProjectShare);
            }
            set
            {
                base[AttributeNames.ProjectShare].Value = value;
            }
        }

        public String Telephone
        {
            get
            {
                return GetString(AttributeNames.Telephone);
            }
            set
            {
                base[AttributeNames.Telephone].Value = value;
            }
        }

        public String Room
        {
            get
            {
                return GetString(AttributeNames.Room);
            }
            set
            {
                base[AttributeNames.Room].Value = value;
            }
        }

        public String MailNickName
        {
            get
            {
                return GetString(AttributeNames.MailNickName);
            }
            set
            {
                base[AttributeNames.MailNickName].Value = value;
            }
        }

        public String LogonScript
        {
            get
            {
                return GetString(AttributeNames.LogonScript);
            }
            set
            {
                base[AttributeNames.LogonScript].Value = value;
            }
        }

        public String HomeMDB
        {
            get
            {
                return GetString(AttributeNames.HomeMDB);
            }
            set
            {
                base[AttributeNames.HomeMDB].Value = value;
            }
        }

        public String ProfilePath
        {
            get
            {
                return GetString(AttributeNames.ProfilePath);
            }
            set
            {
                base[AttributeNames.ProfilePath].Value = value;
            }
        }

        public String HomeDrive
        {
            get
            {
                return GetString(AttributeNames.HomeDrive);
            }
            set
            {
                base[AttributeNames.HomeDrive].Value = value;
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

        public List<string> ConflictRoleList
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

        public String Company
        {
            get
            {
                return GetString(AttributeNames.Company);
            }
            set
            {
                base[AttributeNames.Company].Value = value;
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

        public bool SharePointRootWeb
        {
            get
            {
                return GetBoolean(AttributeNames.SharePointRootWeb);
            }
            set
            {
                base[AttributeNames.SharePointRootWeb].Value = value;
            }
        }

        public String SharePointRelativeUrl
        {
            get
            {
                return GetString(AttributeNames.SharePoionRelativeUrl);
            }
            set
            {
                base[AttributeNames.SharePoionRelativeUrl].Value = value;
            }
        }

        public String SharePointCompleteUrl
        {
            get
            {
                return GetString(AttributeNames.SharePointCompleteUrl);
            }
            set
            {
                base[AttributeNames.SharePointCompleteUrl].Value = value;
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

        public bool AttributeChanged
        {
            get
            {
                return GetBoolean(AttributeNames.AttributeChanged);
            }
            set
            {
                base[AttributeNames.AttributeChanged].Value = value;
            }
        }

        public bool IsProfile
        {
            get
            {
                return GetBoolean(AttributeNames.IsProfile);
            }
            set
            {
                base[AttributeNames.IsProfile].Value = value;
            }
        }

        public bool IsParent
        {
            get
            {
                return GetBoolean(AttributeNames.IsParent);
            }
            set
            {
                base[AttributeNames.IsParent].Value = value;
            }
        }

        #endregion
    }
}
