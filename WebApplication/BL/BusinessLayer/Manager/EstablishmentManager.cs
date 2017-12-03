using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using BL;
using System.Collections.Generic;
using BL.DataLayer;

namespace PersonDemo
{
	public class EstablishmentManager
	{
        private EstablishmentData data;

        public EstablishmentManager()
		{
            this.data = new EstablishmentData();
		}

        public Establishment ReadById(long id)
        {
            DataSet set = this.data.ReadById(id);

            if (set.Tables[0].Rows.Count > 0)
            {
                DataRow row = set.Tables[0].Rows[0];

                return new Establishment()
                {
                    Id = DataUtils.GetInt64N(row["Id"]),
                    CompanyName = Convert.ToString(row["CompanyName"]),
                    TradingName = Convert.ToString(row["TradingName"]),
                    Cnpj = Convert.ToString(row["Cnpj"]),
                    Mail = Convert.ToString(row["Mail"]),
                    Address = Convert.ToString(row["Address"]),
                    CityId = DataUtils.GetInt32N(row["CityId"]),
                    Phone = Convert.ToString(row["Phone"]),
                    RegistrationDate = DataUtils.GetDateTimeN(row["RegistrationDate"]),
                    Category = (EstablishmentCategory)Convert.ToInt32(row["Category"]),
                    Status = (EstablishmentStatus)Convert.ToInt32(row["Status"])
                };
            }

            return null;
        }
        public List<Establishment> ReadAll()
        {
            DataSet set = this.data.ReadAll();
            List<Establishment> list = new List<Establishment>();

            foreach (DataRow row in set.Tables[0].Rows)
            {
                list.Add(new Establishment()
                {
                    Id = DataUtils.GetInt64N(row["Id"]),
                    CompanyName = Convert.ToString(row["CompanyName"]),
                    TradingName = Convert.ToString(row["TradingName"]),
                    Cnpj = Convert.ToString(row["Cnpj"]),
                    Mail= Convert.ToString(row["Mail"]),
                    Address = Convert.ToString(row["Address"]),
                    CityId = DataUtils.GetInt32N(row["CityId"]),
                    Phone = Convert.ToString(row["Phone"]),
                    RegistrationDate = DataUtils.GetDateTimeN(row["RegistrationDate"]),
                    Category = (EstablishmentCategory)Convert.ToInt32(row["Category"]),
                    Status = (EstablishmentStatus)Convert.ToInt32(row["Status"])
                });
            }

            return list;
        }

        public void Create(Establishment establishment)
        {
            string error;
            if (!this.Validate(establishment, out error))
            {
                throw new InvalidOperationException(error);
            }

            establishment.Id = this.data.Create(
                establishment.CompanyName,
                establishment.TradingName,
                establishment.Cnpj,
                establishment.Mail,
                establishment.Address,
                establishment.CityId,
                establishment.Phone,
                establishment.RegistrationDate,
                (short)establishment.Category,
                (short)establishment.Status);
        }

        public void Update(Establishment establishment)
        {
            string error;
            if (!this.Validate(establishment, out error))
            {
                throw new InvalidOperationException(error);
            }

            this.data.Update(
                establishment.Id.Value,
                establishment.CompanyName,
                establishment.TradingName,
                establishment.Cnpj,
                establishment.Mail,
                establishment.Address,
                establishment.CityId,
                establishment.Phone,
                establishment.RegistrationDate,
                (short)establishment.Category,
                (short)establishment.Status);
        }

        public void Delete(long id)
        {
            this.data.Delete(id);
        }

        public bool Validate(Establishment establishment, out string message)
        {
            CityManager cityManager = new CityManager();
            
            if (string.IsNullOrEmpty(establishment.CompanyName))
            {
                message = "Razão social é obrigatório.";
                return false;
            }
            if (string.IsNullOrEmpty(establishment.Cnpj))
            {
                message = "CNPJ é obrigatório.";
                return false;
            }
            else if (!Utils.IsCnpjValid(establishment.Cnpj))
            {
                message = "CNPJ não é válido.";
                return false;
            }
            if (!string.IsNullOrEmpty(establishment.Mail) && !Utils.IsMailValid(establishment.Mail))
            {
                message = "E-mail inválido.";
                return false;
            }
            if (!Enum.IsDefined(typeof(EstablishmentCategory), establishment.Category))
            {
                message = "Categoria é obrigatório.";
                return false;
            }
            if (!Enum.IsDefined(typeof(EstablishmentStatus), establishment.Status))
            {
                message = "Status é obrigatório.";
                return false;
            }
            if (establishment.Category == EstablishmentCategory.Restaurant && string.IsNullOrEmpty(establishment.Phone))
            {
                message = "Telefone é obrigatório.";
                return false;
            }

            message = null;
            return true;
        }
	}
}