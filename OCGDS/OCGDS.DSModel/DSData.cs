using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCGDS.DSModel
{
    public class ConnectionInfo
    {
        public string BaseAddress { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public static ConnectionInfo BuildConnectionInfo(string connection)
        {
            ConnectionInfo ci = new ConnectionInfo();

            if (string.IsNullOrEmpty(connection))
            {
                return null;
            }

            string[] entries = connection.Split(";".ToCharArray());
            if (entries.Length == 0)
            {
                return null;
            }

            foreach (string entry in entries)
            {
                string[] items = entry.Split(":".ToCharArray());
                if (items.Length != 2)
                {
                    continue;
                }

                switch (items[0].ToLower())
                {
                    case "baseaddress":
                        ci.BaseAddress = items[1];
                        break;
                    case "domain":
                        ci.Domain = items[1];
                        break;
                    case "username":
                        ci.UserName = items[1];
                        break;
                    case "password":
                        ci.Password = items[1];
                        break;
                    default:
                        break;
                }
            }

            return ci;
        }
    }

    public class DSResource
    {
        private Dictionary<string, DSAttribute> attributes = new Dictionary<string, DSAttribute>();

        public string DisplayName { get; set; }
        public string ObjectID { get; set; }
        public string ObjectType { get; set; }
        public bool HasPermissionHints { get; set; }
        public Dictionary<string, DSAttribute> Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        public bool IsPresent(string attributeName)
        {
            if (!this.attributes.ContainsKey(attributeName))
            {
                return false;
            }

            if (this.attributes[attributeName].IsNull)
            {
                return false;
            }

            return true;
        }
    }

    public class DSAttribute
    {
        private List<object> values = new List<object>();
        private List<DSResource> resolvedValues = new List<DSResource>();

        public string Description { get; set; }
        public string DisplayName { get; set; }
        public bool IsMultivalued { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsRequired { get; set; }
        public string Regex { get; set; }
        public string SystemName { get; set; }
        public string Type { get; set; }
        public bool IsNull { get; set; }
        public string PermissionHint { get; set; }
        public object Value { get; set; }
        public List<object> Values
        {
            get { return this.values; }
            set { this.values = value; }
        }
        public DSResource ResolvedValue { get; set; }
        public List<DSResource> ResolvedValues
        {
            get { return this.resolvedValues; }
            set { this.resolvedValues = value; }
        }
    }

    public class DSResourceSet
    {
        public int TotalCount { get; set; }

        public bool HasMoreItems { get; set; }

        public List<DSResource> Resources { get; set; }
    }
}
