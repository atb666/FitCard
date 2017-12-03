using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using BL;
using System.Collections.Generic;
using BL.DataLayer;

namespace PersonDemo
{
	public class CityManager
	{
        private CityData data;

        public CityManager()
		{
            this.data = new CityData();
		}

        public City ReadById(int id)
        {
            DataSet set = this.data.ReadById(id);

            if (set.Tables[0].Rows.Count > 0)
            {
                DataRow row = set.Tables[0].Rows[0];

                return new City()
                {
                    Id = Convert.ToInt32(row["Id"]),
                    StateId = Convert.ToInt32(row["StateId"]),
                    Name = Convert.ToString(row["Name"]),
                };
            }

            return null;
        }
        public List<City> ReadByStateId(int stateId)
        {
            DataSet set = this.data.ReadByStateId(stateId);
            List<City> list = new List<City>();

            foreach (DataRow row in set.Tables[0].Rows)
            {
                list.Add(new City()
                {
                    Id = Convert.ToInt32(row["Id"]),
                    StateId = Convert.ToInt32(row["StateId"]),
                    Name = Convert.ToString(row["Name"]),
                });
            }

            return list;
        }
	}
}
