using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel.ResourceTypes
{
    public class RmOrgAssignment : RmResource
    {
        public RmOrgAssignment()
            : base()
        {
        }

        protected const String ResourceType = @"ocgOrgAssignment";

        protected override void EnsureAllAttributesExist()
        {
            base.EnsureAllAttributesExist();

            base.EnsureAttributeExists(AttributeNames.AssignmentId);
            base.EnsureAttributeExists(AttributeNames.AssignedUser);
            base.EnsureAttributeExists(AttributeNames.AssignedOrgUnit);
            base.EnsureAttributeExists(AttributeNames.AssignedRole);
            base.EnsureAttributeExists(AttributeNames.ValidFrom);
            base.EnsureAttributeExists(AttributeNames.ValidTo);
            base.EnsureAttributeExists(AttributeNames.Priority);
            base.EnsureAttributeExists(AttributeNames.FromSharePoint);
            base.EnsureAttributeExists(AttributeNames.SharePointID);
            base.EnsureAttributeExists(AttributeNames.SharePointContext);
            base.EnsureAttributeExists(AttributeNames.ResourceChanged);
            base.EnsureAttributeExists(AttributeNames.ResourceCreated);
            base.EnsureAttributeExists(AttributeNames.IsMandatory);
        }

        public override string GetResourceType()
        {
            return RmOrgAssignment.ResourceType;
        }

        public static string StaticResourceType()
        {
            return RmOrgAssignment.ResourceType;
        }

        #region promoted properties

        public String AssignmentId
        {
            get
            {
                return GetString(AttributeNames.AssignmentId);
            }
            set
            {
                base[AttributeNames.AssignmentId].Value = value;
            }
        }

        public RmReference AssignedUser
        {
            get
            {
                return GetReference(AttributeNames.AssignedUser);
            }
            set
            {
                base[AttributeNames.AssignedUser].Value = value;
            }
        }

        public RmReference AssignedOrgUnit
        {
            get
            {
                return GetReference(AttributeNames.AssignedOrgUnit);
            }
            set
            {
                base[AttributeNames.AssignedOrgUnit].Value = value;
            }
        }

        public RmReference AssignedRole
        {
            get
            {
                return GetReference(AttributeNames.AssignedRole);
            }
            set
            {
                base[AttributeNames.AssignedRole].Value = value;
            }
        }

        public DateTime ValidFrom
        {
            get
            {
                return GetDateTime(AttributeNames.ValidFrom);
            }
            set
            {
                base[AttributeNames.ValidFrom].Value = value;
            }
        }

        public DateTime ValidTo
        {
            get
            {
                return GetDateTime(AttributeNames.ValidTo);
            }
            set
            {
                base[AttributeNames.ValidTo].Value = value;
            }
        }

        public int Priority
        {
            get
            {
                return GetInteger(AttributeNames.Priority);
            }
            set
            {
                base[AttributeNames.Priority].Value = value;
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

        public bool IsMandatory
        {
            get
            {
                return GetBoolean(AttributeNames.IsMandatory);
            }
            set
            {
                base[AttributeNames.IsMandatory].Value = value;
            }
        }

        #endregion

        public sealed new class AttributeNames
        {
            public static RmAttributeName AssignmentId = new RmAttributeName(@"ocgAssignmentId");
            public static RmAttributeName AssignedUser = new RmAttributeName(@"ocgUserRef");
            public static RmAttributeName AssignedOrgUnit = new RmAttributeName(@"ocgOrgUnitRef");
            public static RmAttributeName AssignedRole = new RmAttributeName(@"ocgRoleRef");
            public static RmAttributeName ValidFrom = new RmAttributeName(@"ocgValidFrom");
            public static RmAttributeName ValidTo = new RmAttributeName(@"ocgValidTo");
            public static RmAttributeName Priority = new RmAttributeName(@"ocgOuPrio");
            public static RmAttributeName FromSharePoint = new RmAttributeName(@"ocgFromSharePoint");
            public static RmAttributeName SharePointID = new RmAttributeName(@"ocgSharePointID");
            public static RmAttributeName SharePointContext = new RmAttributeName(@"ocgSharePointContext");
            public static RmAttributeName ResourceChanged = new RmAttributeName(@"ocgResourceChanged");
            public static RmAttributeName ResourceCreated = new RmAttributeName(@"ocgResourceCreated");
            public static RmAttributeName IsMandatory = new RmAttributeName(@"ocgIsMandatory");
        }
    }
}
