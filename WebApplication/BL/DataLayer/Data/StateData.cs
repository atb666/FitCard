using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL.BusinessLayer;
using System.Data;
using System.Data.SqlClient;

namespace BL.DataLayer
{
    class StateData
    {
        private Connection connection;

        public StateData()
        {
            this.connection = new Connection();
        }

        public DataSet ReadAll()
        {
            return this.connection.Execute("r_State");
        }
        public DataSet ReadById(int id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Id", id));

            return this.connection.Execute("r_State", parameters.ToArray());
        }
    }
}