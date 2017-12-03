using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BL.BusinessLayer;
using System.Data;
using System.Data.SqlClient;

namespace BL.DataLayer
{
    class EstablishmentData
    {
        private Connection connection;

        public EstablishmentData()
        {
            this.connection = new Connection();
        }

        public long Create(string companyName, string tradingName, string cnpj, string mail, string address, int? cityId,
            string phone, DateTime? registrationDate, short category, short status)
        {
            SqlParameter parameterId = new SqlParameter("@Id", SqlDbType.BigInt);
            parameterId.Direction = ParameterDirection.Output;

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(parameterId);
            parameters.Add(new SqlParameter("@CompanyName", companyName));
            parameters.Add(new SqlParameter("@TradingName", tradingName));
            parameters.Add(new SqlParameter("@Cnpj", cnpj));
            parameters.Add(new SqlParameter("@Mail", mail));
            parameters.Add(new SqlParameter("@Address", address));
            parameters.Add(new SqlParameter("@CityId", cityId));
            parameters.Add(new SqlParameter("@Phone", phone));
            parameters.Add(new SqlParameter("@RegistrationDate", registrationDate));
            parameters.Add(new SqlParameter("@Category", category));
            parameters.Add(new SqlParameter("@Status", status));

            this.connection.Execute("c_Establishment", parameters.ToArray());

            return (long)parameterId.Value;
        }

        public DataSet ReadAll()
        {
            return this.connection.Execute("r_Establishment");
        }
        public DataSet ReadById(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Id", id));

            return this.connection.Execute("r_Establishment", parameters.ToArray());
        }

        public void Update(long id, string companyName, string tradingName, string cnpj, string mail, string address, int? cityId,
            string phone, DateTime? registrationDate, short category, short status)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Id", id));
            parameters.Add(new SqlParameter("@CompanyName", companyName));
            parameters.Add(new SqlParameter("@TradingName", tradingName));
            parameters.Add(new SqlParameter("@Cnpj", cnpj));
            parameters.Add(new SqlParameter("@Mail", mail));
            parameters.Add(new SqlParameter("@Address", address));
            parameters.Add(new SqlParameter("@CityId", cityId));
            parameters.Add(new SqlParameter("@Phone", phone));
            parameters.Add(new SqlParameter("@RegistrationDate", registrationDate));
            parameters.Add(new SqlParameter("@Category", category));
            parameters.Add(new SqlParameter("@Status", status));

            this.connection.Execute("u_Establishment", parameters.ToArray());
        }

        public void Delete(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Id", id));

            this.connection.Execute("d_Establishment", parameters.ToArray());
        }
    }
}