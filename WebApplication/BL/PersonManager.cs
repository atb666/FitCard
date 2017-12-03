using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace PersonDemo
{
	/// <summary>
	/// Manages persistence of Person objects.
	/// </summary>
	public class PersonManager
	{
		public PersonManager()
		{
		}

		/// <summary>
		/// Loads a Person from the database.
		/// </summary>
		public PersonProperties Load(string socialSecurityNumber)
		{
			PersonProperties properties = new PersonProperties();

			string query = "Select * from Person where SSN = '" + socialSecurityNumber + "'";
				
			OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, conn);
			DataSet dataSet = new DataSet();
			dataAdapter.Fill(dataSet, "Person");
			DataTable table = dataSet.Tables["Person"];

			bool socialSecurityNumberExists = (table.Rows.Count > 0);

			if (socialSecurityNumberExists)
			{
				DataRow row = table.Rows[0];
				
				string name = row["Name"].ToString();
				string birthDate = row["Birthdate"].ToString();
				
				properties.SocialSecurityNumber = socialSecurityNumber;
				properties.Name = name;
				properties.BirthDate = birthDate;
			}
			else
			{
				throw new ArgumentException("The person with social security number, " + 
					socialSecurityNumber + ", was not found.");
			}
			
			return properties;
		}

		/// <summary>
		/// Saves a Person to the database.
		/// </summary>
		public void Save(Person person)
		{
			PersonProperties properties = person.GetState();
			string query = "Select * from Person where SSN = '" + properties.SocialSecurityNumber + "'";
				
			OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, conn);
			DataSet dataSet = new DataSet();
			dataAdapter.Fill(dataSet, "Person");
			DataTable table = dataSet.Tables["Person"];

			bool socialSecurityNumberExists = (table.Rows.Count > 0);

			if (socialSecurityNumberExists) // Update
			{
				// Note: if Person is new the existing record will just be overwritten.
				// In a production environment we would disallow this.

				DataRow row = table.Rows[0];

				row["SSN"] = properties.SocialSecurityNumber;
				row["Name"] = properties.Name;
				row["Birthdate"] = properties.BirthDate;

				// Update it
				OleDbCommandBuilder cb = new OleDbCommandBuilder(dataAdapter);
				dataAdapter.Update(dataSet, "Person");
			}
			else // Insert
			{
				Trace.Assert(person.IsNew || person.IsDirty, "Person should be new or modified.");
				
				OleDbConnection dbConn = new OleDbConnection(conn);

				try
				{
					dbConn.Open();

					string insertQuery = 
						"INSERT INTO Person (SSN, Name, Birthdate) VALUES ('" + 
						properties.SocialSecurityNumber + "', '" + 
						properties.Name + "', '" + 
						properties.BirthDate + "')";
					OleDbCommand cmd = new OleDbCommand(insertQuery, dbConn);
					int numInserted = cmd.ExecuteNonQuery();
				}
				finally
				{
					dbConn.Close();
				}
			}
		}

		/// <summary>
		/// Deletes a Person from the database.
		/// </summary>
		public void Delete(string socialSecurityNumber)
		{
			OleDbConnection dbConn = new OleDbConnection(conn);

			try
			{
				dbConn.Open();
				string deleteQuery = "DELETE FROM Person WHERE SSN = '" + socialSecurityNumber + "'";
				OleDbCommand cmd = new OleDbCommand(deleteQuery, dbConn);
				int numDeleted = cmd.ExecuteNonQuery();
			}
			finally
			{
				dbConn.Close();
			}
		}

		// The database connection
		private const string    conn = 
			"provider=Microsoft.JET.OLEDB.4.0; " +
			"data source = ..\\..\\person.mdb";
	}
}
