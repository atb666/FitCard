using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BL.BusinessLayer
{
    class Connection
    {
        private SqlDataAdapter myAdapter;
        private SqlConnection conn;

        /// <constructor>
        /// Initialise Connection
        /// </constructor>
        public Connection()
        {
            myAdapter = new SqlDataAdapter();
            conn = new SqlConnection(ConnectionConfig.ConnectionString);
        }

        /// <method>
        /// Open Database Connection if Closed or Broken
        /// </method>
        private SqlConnection OpenConnection()
        {
            if (conn.State == ConnectionState.Closed || conn.State ==
                        ConnectionState.Broken)
            {
                conn.Open();
            }
            return conn;
        }

        public DataSet Execute(string procedureName)
        {
            return this.Execute(procedureName, null);
        }

        public DataSet Execute(string procedureName, SqlParameter[] sqlParameter)
        {
            SqlCommand comm = new SqlCommand(procedureName);
            DataSet dataSet = new DataSet();

            try
            {
                comm.Connection = this.conn;
                comm.CommandType = CommandType.StoredProcedure;

                if (sqlParameter != null)
                {
                    comm.Parameters.AddRange(sqlParameter);
                }

                this.Open();
                new SqlDataAdapter(comm).Fill(dataSet);
            }
            catch (Exception e)
            {
                Console.Write("Exception: " + e.ToString());
                throw;
            }
            finally
            {
                this.Close();
            }

            return dataSet;
        }

        private void Open()
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
            {
                conn.Open();
            }
        }
        private void Close()
        {
            conn.Close();
        }
    }
}