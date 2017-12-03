using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL.BusinessLayer;
using System.Data;
using System.Data.SqlClient;

namespace BL.DataLayer
{
    class CityData
    {
        private Connection connection;

        public CityData()
        {
            this.connection = new Connection();
        }

        public DataSet ReadById(int id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Id", id));

            return this.connection.Execute("r_City", parameters.ToArray());
        }
        public DataSet ReadByStateId(int stateId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@StateId", stateId));

            return this.connection.Execute("r_City", parameters.ToArray());
        }
    }
}