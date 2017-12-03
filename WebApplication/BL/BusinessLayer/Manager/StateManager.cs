using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using BL;
using System.Collections.Generic;
using BL.DataLayer;

namespace PersonDemo
{
	public class StateManager
	{
        private StateData data;

        public StateManager()
		{
            this.data = new StateData();
		}

        public State ReadById(int id)
        {
            DataSet set = this.data.ReadById(id);

            if (set.Tables[0].Rows.Count > 0)
            {
                DataRow row = set.Tables[0].Rows[0];

                return new State()
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = Convert.ToString(row["Name"]),
                };
            }

            return null;
        }
        public List<State> ReadAll()
        {
            DataSet set = this.data.ReadAll();
            List<State> list = new List<State>();

            foreach (DataRow row in set.Tables[0].Rows)
            {
                list.Add(new State()
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = Convert.ToString(row["Name"]),
                });
            }

            return list;
        }
	}
}
