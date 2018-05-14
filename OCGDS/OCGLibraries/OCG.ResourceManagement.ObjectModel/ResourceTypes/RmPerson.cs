using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel.ResourceTypes
{
    public class RmPerson : RmResource
    {
        public RmPerson()
            : base()
        {

        }

        private RmList<RmReference> authNWorkflowsRegistered;

        private RmList<RmReference> _roleRefList;
        private RmList<RmReference> _permissionRefList;
        private List<string> _blockedRoles;
        private List<string> _conflictRoles;

        protected const String ResourceType = @"Person";

        public sealed new class AttributeNames
        {
            public static RmAttributeName AccountName = new RmAttributeName(@"AccountName");
            public static RmAttributeName Domain = new RmAttributeName(@"Domain");
            public static RmAttributeName DomainConfiguration = new RmAttributeName(@"DomainConfiguration");
            public static RmAttributeName EmployeeType = new RmAttributeName(@"EmployeeType");
            public static RmAttributeName EmployeeID = new RmAttributeName(@"EmployeeID");
            public static RmAttributeName Email = new RmAttributeName(@"Email");
            public static RmAttributeName FirstName = new RmAttributeName(@"FirstName");
            public static RmAttributeName LastName = new RmAttributeName(@"LastName");
            public static RmAttributeName MailNickname = new RmAttributeName(@"MailNickname");
            public static RmAttributeName Manager = new RmAttributeName(@"Manager");
            public static RmAttributeName ObjectSID = new RmAttributeName(@"ObjectSID");
            public static RmAttributeName Register = new RmAttributeName(@"Register");
            public static RmAttributeName RegistrationRequired = new RmAttributeName(@"RegistrationRequired");
            public static RmAttributeName AuthNWorkflowsRegistered = new RmAttributeName(@"AuthNWFRegistered");
            public static RmAttributeName ResetPassword = new RmAttributeName(@"ResetPassword");
            public static RmAttributeName HomeDrive = new RmAttributeName(@"ocgHomeDrive");
            public static RmAttributeName ProfilePath = new RmAttributeName(@"ocgProfilePath");
            public static RmAttributeName HomeMDB = new RmAttributeName(@"ocgHomeMDB");
            public static RmAttributeName LogonScript = new RmAttributeName(@"ocgLogonScript");
            public static RmAttributeName Room = new RmAttributeName(@"ocgRoom");
            public static RmAttributeName JobTitle = new RmAttributeName(@"JobTitle");

            public static RmAttributeName City = new RmAttributeName(@"City");
            public static RmAttributeName Country = new RmAttributeName(@"Country");
            public static RmAttributeName PostalCode = new RmAttributeName(@"PostalCode");
            public static RmAttributeName Address = new RmAttributeName(@"Address");
            public static RmAttributeName OfficePhone = new RmAttributeName(@"OfficePhone");
            public static RmAttributeName Fax = new RmAttributeName(@"OfficeFax");

            public static RmAttributeName PrimaryOU = new RmAttributeName(@"ocgPrimaryOU");

            public static RmAttributeName RoleRefList = new RmAttributeName(@"ocgRoleRefList");
            public static RmAttributeName PermissionRefList = new RmAttributeName(@"ocgPermissionRefList");

            public static RmAttributeName ConflictRoles = new RmAttributeName(@"ocgConflictRoleList");
            public static RmAttributeName BlockedRoles = new RmAttributeName(@"ocgBlockedRoleList");

            public static RmAttributeName BreakInheritance = new RmAttributeName(@"ocgBreakInheritance");

            public static RmAttributeName Department = new RmAttributeName(@"Department");
            public static RmAttributeName Company = new RmAttributeName(@"Company");

            public static RmAttributeName FromSharePoint = new RmAttributeName(@"ocgFromSharePoint");
            public static RmAttributeName SharePointID = new RmAttributeName(@"ocgSharePointID");
            public static RmAttributeName SharePointContext = new RmAttributeName(@"ocgSharePointContext");

            public static RmAttributeName ResourceChanged = new RmAttributeName(@"ocgResourceChanged");
            public static RmAttributeName ResourceCreated = new RmAttributeName(@"ocgResourceCreated");
        }

        protected override void EnsureAllAttributesExist()
        {
            base.EnsureAllAttributesExist();

            base.EnsureAttributeExists(AttributeNames.AccountName);
            base.EnsureAttributeExists(AttributeNames.Domain);
            base.EnsureAttributeExists(AttributeNames.DomainConfiguration);
            base.EnsureAttributeExists(AttributeNames.Email);
            base.EnsureAttributeExists(AttributeNames.EmployeeID);
            base.EnsureAttributeExists(AttributeNames.EmployeeType);
            base.EnsureAttributeExists(AttributeNames.FirstName);
            base.EnsureAttributeExists(AttributeNames.LastName);
            base.EnsureAttributeExists(AttributeNames.MailNickname);
            base.EnsureAttributeExists(AttributeNames.Manager);
            base.EnsureAttributeExists(AttributeNames.ObjectSID);
            base.EnsureAttributeExists(AttributeNames.Register);
            base.EnsureAttributeExists(AttributeNames.RegistrationRequired);
            base.EnsureAttributeExists(AttributeNames.AuthNWorkflowsRegistered);
            base.EnsureAttributeExists(AttributeNames.ResetPassword);
            base.EnsureAttributeExists(AttributeNames.HomeDrive);
            base.EnsureAttributeExists(AttributeNames.ProfilePath);
            base.EnsureAttributeExists(AttributeNames.HomeMDB);
            base.EnsureAttributeExists(AttributeNames.LogonScript);
            base.EnsureAttributeExists(AttributeNames.Room);
            base.EnsureAttributeExists(AttributeNames.City);
            base.EnsureAttributeExists(AttributeNames.Country);
            base.EnsureAttributeExists(AttributeNames.PostalCode);
            base.EnsureAttributeExists(AttributeNames.Address);
            base.EnsureAttributeExists(AttributeNames.OfficePhone);
            base.EnsureAttributeExists(AttributeNames.Fax);
            base.EnsureAttributeExists(AttributeNames.PrimaryOU);
            base.EnsureAttributeExists(AttributeNames.RoleRefList);
            base.EnsureAttributeExists(AttributeNames.PermissionRefList);
            base.EnsureAttributeExists(AttributeNames.ConflictRoles);
            base.EnsureAttributeExists(AttributeNames.BlockedRoles);
            base.EnsureAttributeExists(AttributeNames.BreakInheritance);
            base.EnsureAttributeExists(AttributeNames.JobTitle);
            base.EnsureAttributeExists(AttributeNames.Department);
            base.EnsureAttributeExists(AttributeNames.Company);
            base.EnsureAttributeExists(AttributeNames.FromSharePoint);
            base.EnsureAttributeExists(AttributeNames.SharePointID);
            base.EnsureAttributeExists(AttributeNames.SharePointContext);
            base.EnsureAttributeExists(AttributeNames.ResourceChanged);
            base.EnsureAttributeExists(AttributeNames.ResourceCreated);
        }

        public override string GetResourceType()
        {
            return RmPerson.ResourceType;
        }

        public static string StaticResourceType()
        {
            return RmPerson.ResourceType;
        }

        #region promoted properties

        public RmReference Manager
        {
            get
            {
                return GetReference(AttributeNames.Manager);
            }
            set
            {
                base[AttributeNames.Manager].Value = value;
            }
        }

        public String FirstName
        {
            get
            {
                return GetString(AttributeNames.FirstName);
            }
            set
            {
                base[AttributeNames.FirstName].Value = value;
            }
        }

        public String LastName
        {
            get
            {
                return GetString(AttributeNames.LastName);
            }
            set
            {
                base[AttributeNames.LastName].Value = value;
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

        public String AccountName
        {
            get
            {
                return GetString(AttributeNames.AccountName);
            }
            set
            {
                base[AttributeNames.AccountName].Value = value;
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

        public String EmployeeID
        {
            get
            {
                return GetString(AttributeNames.EmployeeID);
            }
            set
            {
                base[AttributeNames.EmployeeID].Value = value;
            }
        }

        public String EmployeeType
        {
            get
            {
                return GetString(AttributeNames.EmployeeType);
            }
            set
            {
                base[AttributeNames.EmployeeType].Value = value;
            }
        }

        public IList<RmReference> AuthNWorkflowsRegistered
        {
            get
            {
                if (this.authNWorkflowsRegistered == null)
                {
                    lock (base.attributes)
                    {
                        this.authNWorkflowsRegistered = GetMultiValuedReference(AttributeNames.AuthNWorkflowsRegistered);
                        return this.authNWorkflowsRegistered;
                    }
                }
                else
                {
                    return this.authNWorkflowsRegistered;
                }
            }
        }

        public bool ResetPassword
        {
            get
            {
                return GetBoolean(AttributeNames.ResetPassword);
            }
            set
            {
                base[AttributeNames.ResetPassword].Value = value;
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

        public String City
        {
            get
            {
                return GetString(AttributeNames.City);
            }
            set
            {
                base[AttributeNames.City].Value = value;
            }
        }

        public String Country
        {
            get
            {
                return GetString(AttributeNames.Country);
            }
            set
            {
                base[AttributeNames.Country].Value = value;
            }
        }

        public String PostalCode
        {
            get
            {
                return GetString(AttributeNames.PostalCode);
            }
            set
            {
                base[AttributeNames.PostalCode].Value = value;
            }
        }

        public String Address
        {
            get
            {
                return GetString(AttributeNames.Address);
            }
            set
            {
                base[AttributeNames.Address].Value = value;
            }
        }

        public String OfficePhone
        {
            get
            {
                return GetString(AttributeNames.OfficePhone);
            }
            set
            {
                base[AttributeNames.OfficePhone].Value = value;
            }
        }

        public String Fax
        {
            get
            {
                return GetString(AttributeNames.Fax);
            }
            set
            {
                base[AttributeNames.Fax].Value = value;
            }
        }

        public RmReference PrimaryOU
        {
            get
            {
                return GetReference(AttributeNames.PrimaryOU);
            }
            set
            {
                base[AttributeNames.PrimaryOU].Value = value;
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

        public String JobTitle
        {
            get
            {
                return GetString(AttributeNames.JobTitle);
            }
            set
            {
                base[AttributeNames.JobTitle].Value = value;
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
