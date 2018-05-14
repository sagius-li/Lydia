using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using OCG.ResourceManagement.ObjectModel;
using OCG.ResourceManagement.ObjectModel.ResourceTypes;

namespace OCG.ResourceManagement.DBAccess
{
    public class ResourceReader : IDisposable
    {
        #region Private Member

        private string connectionString = "Integrated Security=SSPI;Initial Catalog=FIMService;Data Source=localhost;";

        private SqlConnection conn;

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            if (conn != null)
            {
                if (conn.State != System.Data.ConnectionState.Closed)
                {
                    try
                    {
                        conn.Close();
                    }
                    catch (InvalidOperationException e)
                    {
                    }

                }

                conn.Dispose();
            }
        }

        #endregion

        #region Constructor

        public ResourceReader()
        {
            conn = new SqlConnection(this.connectionString);
            conn.Open();
        }

        public ResourceReader(string connectionString)
        {
            this.connectionString = connectionString;

            conn = new SqlConnection(this.connectionString);
            conn.Open();
        }

        public ResourceReader(string connectionString, string userID, string password)
        {
            this.connectionString = connectionString + "User Id={0};Password={1};";

            conn = new SqlConnection(string.Format(this.connectionString, userID, password));
            conn.Open();
        }

        #endregion

        #region Public Methods

        public List<RmResource> GetResourceByQuery(string query, int location, string[] attributes)
        {
            throw new NotImplementedException();
        }

        public List<RmResource> GetResourceByQuery(string query, int skip, int take, string[] attributes)
        {
            throw new NotImplementedException();
        }

        public List<RmResource> GetResourceByQuery(string query, int location, int skip, int take, string[] attributes)
        {
            throw new NotImplementedException();
        }

        public List<RmRight> GetRights(Guid actor, Guid target, string dbName) 
        { 
            return DBAccess.GetRights(ref conn, actor, target, dbName); 
        }

        public RmAttribute GetAttribute(string attributeName)
        {
            return DBAccess.GetAttribute(ref conn, attributeName);
        }

        public List<RmAttribute> GetAttributes(string objectType, int localeKey) 
        { 
            return DBAccess.GetAttributes(ref conn, objectType, localeKey); 
        }

        public List<RmResource> GetResourceByQuery(string query, string[] attributes, string requestorID, bool checkRights = true, int count = -1, int skip = 0)
        {
            return DBAccess.GetResourceByQuery(ref conn, requestorID, query, attributes, checkRights, count, skip);
        }

        #endregion
    }
}
