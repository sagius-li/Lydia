using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
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
                string[] items = entry.Trim().Split(":".ToCharArray());
                if (items.Length != 2)
                {
                    continue;
                }

                switch (items[0].Trim().ToLower())
                {
                    case "baseaddress":
                        ci.BaseAddress = "http:" + items[1].Trim();
                        break;
                    case "domain":
                        ci.Domain = items[1].Trim();
                        break;
                    case "username":
                        ci.UserName = items[1].Trim();
                        break;
                    case "password":
                        ci.Password = items[1].Trim();
                        break;
                    default:
                        break;
                }
            }

            return ci;
        }
    }

    public class ResourceOption
    {
        private string[] attributesToResolve = new string[] { "DisplayName" };
        private string[] sortingAttributes = null;
        private ConnectionInfo connectionInfo = new ConnectionInfo();

        public bool ResolveID { get; set; }
        public bool DeepResolve { get; set; }
        public int CultureKey { get; set; }

        public ConnectionInfo ConnectionInfo
        {
            get { return connectionInfo; }
            set { connectionInfo = value; }
        }
        public string[] AttributesToResolve
        {
            get { return attributesToResolve; }
            set { attributesToResolve = value; }
        }
        public string[] SortingAttributes
        {
            get { return sortingAttributes; }
            set { sortingAttributes = value; }
        }

        public ResourceOption()
        {
            ResolveID = false;
            DeepResolve = false;
            CultureKey = 127;
            SortingAttributes = null;
        }

        public ResourceOption(ConnectionInfo connectionInfo, int cultureKey = 127, bool resolveID = false, 
            bool deepResolve = false, string[] attributesToResolve = null, string[] sortingAttributes = null)
        {
            ConnectionInfo = connectionInfo;
            CultureKey = cultureKey;
            ResolveID = resolveID;
            DeepResolve = deepResolve;
            if (attributesToResolve != null && attributesToResolve.Length != 0 && attributesToResolve[0] != null)
            {
                AttributesToResolve = attributesToResolve;
            }
            if (sortingAttributes != null && sortingAttributes.Length != 0 && sortingAttributes[0] != null)
            {
                SortingAttributes = sortingAttributes;
            }
        }
    }

    public class RepositoryManager
    {
        public static Lazy<IOCGDSRepository> GetRepository(
            IEnumerable<Lazy<IOCGDSRepository, Dictionary<string, object>>> repositories, string name)
        {
            Lazy<IOCGDSRepository> repo = null;

            foreach (Lazy<IOCGDSRepository, Dictionary<string, object>> item in repositories)
            {
                if (item.Metadata.ContainsKey("Name") &&
                    item.Metadata["Name"].ToString().Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    repo = item;
                    break;
                }
            }

            return repo;
        }
    }

    public class ConfigManager
    {
        public static string GetAppSetting(string key, string defaultValue = null)
        {
            return string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]) ? 
                defaultValue : ConfigurationManager.AppSettings[key];
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
        private List<DSResource> resources = new List<DSResource>();

        public int TotalCount { get; set; }

        public bool HasMoreItems { get; set; }

        public List<DSResource> Resources
        {
            get { return this.resources; }
            set { this.resources = value; }
        }

        public DSResourceSet()
        {
            TotalCount = 0;
            HasMoreItems = false;
        }
    }
}
