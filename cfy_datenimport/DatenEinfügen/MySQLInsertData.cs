using System;
using System.Data;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Collections.Generic;

namespace MySQL
{
	public class MySQLInsertData
	{
		private MySqlConnection connection;
		private string server;
		private string database;
		private string uid;
		private string password;
		private string charset;
		private string connectionString;

		public MySQLInsertData ()
		{
			Initialize ();
		}

		private void Initialize ()
		{
			server = "localhost";
			database = "noc_portal";
			uid = "root";
			password = "";
			charset = "CHARSET=utf8";
			connectionString = "SERVER=" + server + ";" + "DATABASE=" +
				database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" + charset + ";";
			connection = new MySqlConnection (connectionString);
		}

		public bool OpenConnection ()
		{
			try {
				connection.Open ();
				return true;
			} catch (MySqlException ex) {
				switch (ex.Number) {
					case 0:
					Console.WriteLine ("Cannot connect to server.  Contact administrator");
					break;

					case 1045:
					Console.WriteLine ("Invalid username/password, please try again");
					break;
				}
				return false;
			}
		}

		public bool CloseConnection ()
		{
			try {
				connection.Close ();
				return true;
			} catch (MySqlException ex) {
				Console.WriteLine (ex.Message);
				return false;
			}
		}

		//Fügt einen übergebenen Parameter in die übergebene Tabelle ein.
		public bool Insert(string table, string value)
		{
			System.Text.Encoding utf_8 = System.Text.Encoding.UTF8;

			byte[] utf = System.Text.Encoding.UTF8.GetBytes(value);

			value = string.Empty;
			value = System.Text.Encoding.UTF8.GetString(utf);


			string query = "INSERT INTO " + table + " (" +
				GetFirstColumnName(table) + ") " +
					"VALUES ('" +
					value + "');";

			try {
				if (OpenConnection () == true) {
					MySqlCommand cmd = new MySqlCommand (query, connection);
					//Execute
					cmd.ExecuteNonQuery ();
					//Close Connection
					CloseConnection ();
					return true;
				}
			} catch (MySqlException ex) {

				Console.WriteLine ("MySQL Fehler: " + ex.Number);

			}
			return false;

		}

		public bool Insert(string table, string value, string value2)
		{

			string query = "INSERT INTO " + table + " (" +
				GetFirstColumnName(table) + "," +
				GetSecondColumnName(table) + ") " +
					"VALUES ('" +
					value + "','" +
					value2 + "');";


			try {
				if (OpenConnection () == true) {
					MySqlCommand cmd = new MySqlCommand (query, connection);
					//Execute
					cmd.ExecuteNonQuery ();
					//Close Connection
					CloseConnection ();
					return true;
				}
			} catch (MySqlException ex) {

				Console.WriteLine ("MySQL Fehler: " + ex.Number);

			}
			return false;

		}
		public bool Insert(string table, string sollWe, string subscriber, string internet, string telefon, string objektId)
		{

			string query = "INSERT INTO " + table + " (" +
					GetFirstColumnName(table) + "," +
					GetSecondColumnName(table) + "," +
					GetThirdColumnName(table) + "," +
					GetFourthColumnName(table) + "," +
					GetSixthColumnName(table) + ") " +
					"VALUES ('" +
					sollWe + "','" +
					subscriber + "','" +
					internet + "','" +
					telefon + "','" +
					objektId + "');";


			try {
				if (OpenConnection () == true) {
					MySqlCommand cmd = new MySqlCommand (query, connection);
					//Execute
					cmd.ExecuteNonQuery ();
					//Close Connection
					CloseConnection ();
					return true;
				}
			} catch (MySqlException ex) {

				Console.WriteLine ("MySQL Fehler: " + ex.Number);

			}
			return false;

		}

		public void UpdateCustomer(string table, string objektId)
		{
			// update cfy_kunden set status = 1 where soll_we = 20;
			string query = "UPDATE " + table + " SET status = 1 where objekt_id = " + objektId + ";";



			try {
				if (OpenConnection () == true) {
					MySqlCommand cmd = new MySqlCommand (query, connection);
					//Execute
					cmd.ExecuteNonQuery ();
					//Close Connection
					CloseConnection ();

				}
			} catch (MySqlException ex) {

				Console.WriteLine ("MySQL Fehler: " + ex.Number);

			}

		}


		public bool CheckTableName (string table_name)
		{
			string getschemaquery = "SHOW TABLES;";

			//MySql Connection anlegen.
			OpenConnection ();
			MySqlCommand command = new MySqlCommand (getschemaquery, connection);


			//Reader starten
			MySqlDataReader reader = command.ExecuteReader ();
			while (reader.Read ()) {
				if (table_name == reader.GetString (0)) {
					reader.Close ();
					CloseConnection ();
					return true;
				}
			}
			reader.Close ();
			CloseConnection ();
			return false;

		}

		//Holt die Spaltennamen der übergebenen Tabelle 
		//und gibt eine Liste mit den Spaltennamen zurück.
		public List<string> GetColumnNames (string table_name)
		{
			List<string> liste = new List<string> ();
			if (CheckTableName (table_name)) {


				string getschemaquery = "SELECT `COLUMN_NAME`" +
					"FROM `INFORMATION_SCHEMA`.`COLUMNS`" +
						"WHERE `TABLE_SCHEMA`='" + database + "'" +
						"AND `TABLE_NAME`='" + table_name + "';";

				//MySql Connection anlegen.
				OpenConnection ();
				MySqlCommand command = new MySqlCommand (getschemaquery, connection);


				//Reader starten
				MySqlDataReader reader = command.ExecuteReader ();
				while (reader.Read ()) {
					liste.Add (reader.GetString (0));
				}
				reader.Close ();
				CloseConnection ();

				return liste;
				//Returns a string list of column names for the table
			} else {
				Console.WriteLine ("Table doesn't exist");
				return liste;
			}
		}

		//Gibt eine Liste mit allen Tabellen in denen der Spaltenname vorkommt aus dem Bereich cfy_%
		//Jedoch ohne cfy_rohdaten 
		//Kann auch Doppelungen beinhalten, wobei jedoch die erste gefundene Tabelle zurückgegeben wird.
		//Das Ergebnis ist nicht immer korrekt, deswegen wird die Funktion nicht mehr benutzt.
		public string GetTableNameForColumn(string columnName)
		{

			List<string> liste = new List<string> ();


			try{
				string getschemaquery = "SELECT DISTINCT `TABLE_NAME`" +
					"FROM `INFORMATION_SCHEMA`.`COLUMNS`" +
						"WHERE `COLUMN_NAME` in ('" + columnName + "')" +
						"AND `TABLE_NAME` lIKE 'cfy_%' AND `TABLE_NAME` NOT IN ('cfy_rohdaten');";

				//MySql Connection anlegen.
				OpenConnection ();
				MySqlCommand command = new MySqlCommand (getschemaquery, connection);


				//Reader starten
				MySqlDataReader reader = command.ExecuteReader ();
				while (reader.Read ()) {
					liste.Add (reader.GetString (0));
				}
				reader.Close ();
				CloseConnection ();

				return liste[0];
				//Returns a string list of tables related to the column
			}catch(Exception){
				Console.WriteLine ("GetTableNameForColumn Fehler.");
				return "Fehler";//Noch zu bearbeiten...
			}

		}


		public int GetColumnNumbers(string table)
		{
			List<string> columnNames = GetColumnNames (table);
			return columnNames.Count;
		}

		//Gibt die Bezeichnung der ersten Spalte aus der übergebenen Tabelle zurück
		private string GetFirstColumnName(string table)
		{

			if (GetColumnNumbers (table) >= 0) {
				List<string> columnNames = GetColumnNames (table);
				return columnNames [0];
			} else {
				return "Fehler";//Noch zu bearbeiten...
			}

		}
		private string GetSecondColumnName(string table)
		{
			if (GetColumnNumbers (table) >= 1) {
				List<string> columnNames = GetColumnNames (table);
				return columnNames [1];
			} else {
				return "Fehler";
			}

		}

		private string GetThirdColumnName(string table)
		{
			if (GetColumnNumbers (table) >= 1) {
				List<string> columnNames = GetColumnNames (table);
				return columnNames [2];
			} else {
				return "Fehler";
			}

		}
		private string GetFourthColumnName(string table)
		{
			if (GetColumnNumbers (table) >= 1) {
				List<string> columnNames = GetColumnNames (table);
				return columnNames [3];
			} else {
				return "Fehler";
			}

		}
		private string GetSixthColumnName(string table)
		{
			if (GetColumnNumbers (table) >= 1) {
				List<string> columnNames = GetColumnNames (table);
				return columnNames [5];
			} else {
				return "Fehler";
			}

		}

	}
}

