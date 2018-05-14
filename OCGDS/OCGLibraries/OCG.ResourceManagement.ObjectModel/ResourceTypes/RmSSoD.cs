using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel.ResourceTypes
{
    public class RmSSoD : RmResource
    {
        private RmList<RmReference> _rejectionRefs;

        public RmSSoD()
            : base()
        {

        }

        protected const String ResourceType = @"ocgSSoD";

        public sealed new class AttributeNames
        {
            public static RmAttributeName Enabled = new RmAttributeName(@"ocgSSodEnabled");
            public static RmAttributeName RejectionRefs = new RmAttributeName(@"ocgSSodRefList");
        }

        protected override void EnsureAllAttributesExist()
        {
            base.EnsureAllAttributesExist();

            base.EnsureAttributeExists(AttributeNames.Enabled);
            base.EnsureAttributeExists(AttributeNames.RejectionRefs);
        }

        public override string GetResourceType()
        {
            return RmSSoD.ResourceType;
        }

        public static string StaticResourceType()
        {
            return RmSSoD.ResourceType;
        }

        #region promoted properties

        public bool Enabled
        {
            get
            {
                return GetBoolean(AttributeNames.Enabled);
            }
            set
            {
                base[AttributeNames.Enabled].Value = value;
            }
        }

        public IList<RmReference> RejectionRefs
        {
            get
            {
                if (this._rejectionRefs == null)
                {
                    this._rejectionRefs = GetMultiValuedReference(AttributeNames.RejectionRefs);
                    return this._rejectionRefs;
                }
                else
                {
                    return this._rejectionRefs;
                }
            }
        }

        #endregion
    }
}
