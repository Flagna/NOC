using System;
using System.Data;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Collections.Generic;
using MEQuery;

namespace MySQL
{
	public class MySQLInsertData
	{
		public MySqlConnection connection;
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

		public void Initialize ()
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
		public bool Insert (string table, string value)
		{
			System.Text.Encoding utf_8 = System.Text.Encoding.UTF8;

			byte[] utf = System.Text.Encoding.UTF8.GetBytes (value);

			value = string.Empty;
			value = System.Text.Encoding.UTF8.GetString (utf);


			string query = "INSERT INTO " + table + " (" +
				GetFirstColumnName (table) + ") " +
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

		public bool Insert (string table, string value, string value2)
		{

			string query = "INSERT INTO " + table + " (" +
				GetFirstColumnName (table) + "," +
				GetSecondColumnName (table) + ") " +
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

		public bool Insert (string table, string sollWe, string subscriber, string internet, string telefon, string objektId)
		{
			List<string> columnNames = GetColumnNames (table);
			string query = "INSERT INTO " + table + " (" +
				columnNames [0] + "," +
				columnNames [1] + "," +
				columnNames [2] + "," +
				columnNames [3] + "," +
				columnNames [5] + ") " +
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

		public void InsertIntoData (string[] IdList)
		{

			List<string> columnNames = GetColumnNames ("cfy_daten");
			string query = "INSERT INTO cfy_daten (" +
				columnNames [0] + "," +
				columnNames [1] + "," +
				columnNames [3] + "," +
				columnNames [4] + "," +
				columnNames [5] + "," +
				columnNames [6] + "," +
				columnNames [7] + "," +
				columnNames [8] + "," +
				columnNames [9] + "," +
				columnNames [10] + "," +
				columnNames [11] + "," +
				columnNames [13] + "," +
				columnNames [14] + "," +
				columnNames [15] + "," +
				columnNames [21] + ") " +
				"VALUES ('" +
				IdList [0] + "','" +
				IdList [1] + "','" +
				IdList [2] + "','" +
				IdList [3] + "','" +
				IdList [4] + "','" +
				IdList [5] + "','" +
				IdList [6] + "','" +
				IdList [7] + "','" +
				IdList [8] + "','" +
				IdList [9] + "','" +
				IdList [10] + "','" +
				IdList [11] + "','" +
				IdList [12] + "','" +
				IdList [13] + "','" +
				IdList [14] + "');";

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

		public void ChangeStatus (string tabelle, string id, bool status)
		{
			List<string> columnNames = GetColumnNames (tabelle);

			string stat;
			if (status) {
				stat = "True";
			} else {
				stat = "False";
			}
			string query = "UPDATE " + tabelle + " SET status = " + stat + " WHERE " + columnNames [0] + " like '" + id + "';";


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
		//Satus änderung auf 2stellige Tabellenelemente, in dem Fall nur für cfy_gps
		public void ChangeStatus (string tabelle, string id, string id2, bool status)
		{
			List<string> columnNames = GetColumnNames (tabelle);

			string stat;
			if (status) {
				stat = "True";
			} else {
				stat = "False";
			}
			string query = "UPDATE " + tabelle + " SET status = " + stat + " WHERE " + columnNames [0] + " like '" + id + "' " +
				"AND " + columnNames [1] + " like '" + id2 + "';";

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

		public void ChangeStatusInCustomer (string tabelle, string id, bool status)
		{
			List<string> columnNames = GetColumnNames (tabelle);

			string stat;
			if (status) {
				stat = "True";
			} else {
				stat = "False";
			}
			string query = "UPDATE " + tabelle + " SET status = " + stat + " WHERE " + columnNames [5] + " like '" + id + "';";

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
		//Macht eine Update-Query auf cfy_kunden wobei nur der Status auf inaktiv gesetzt wird und gleichzeitig in cfy_geloescht ein Log geschrieben wird.
		public void UpdateCustomer (string table, string objektId)
		{
			int unix_zeitpunkt = 0;
			Datum time = new Datum ();
			unix_zeitpunkt = time.unix ();
			// Update query erstellen
			string query = "UPDATE " + table + " SET status = 1 where objekt_id = " + objektId + ";";

			//Objekte für Log und Get erstellen
			MySQLInsertLog log = new MySQLInsertLog ();
			MySQLGetData get = new MySQLGetData ();
			//Information, dass ein Datensatz auf inaktiv gesetzt wurde cfy_geloecht entspricht cfy_inkativ heißt aktuell nur anders in der Datenbank
			log.InsertIntoGeloescht ("cfy_kunden", get.GetIdfromTable ("cfy_kunden", objektId), objektId.ToString ());

			//Query ausführen
			try {
				if (OpenConnection () == true) {
					MySqlCommand cmd = new MySqlCommand (query, connection);
					//Ausführen
					cmd.ExecuteNonQuery ();
					//Verbindung schließen
					CloseConnection ();

				}
			} catch (MySqlException ex) {
				//Falls die Query Fehlerhaft ist, die MySQL Exception Nummer ausgeben. Die MySQL Exception Nummern kann man nachlesen in gängigen Internet-Tabellen
				Console.WriteLine ("MySQL Fehler: " + ex.Number);

			}

		}

		//Prüft auf Datenbankebene, ob der übergebene Tabellenname existiert wenn der Tabellenname in der Datenbank gefunden wurde, wird true zurückgegeben sont false.
		public bool CheckTableName (string table_name)
		{
			string getschemaquery = "SHOW TABLES;";

			//MySql Connection anlegen.
			try {
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
			} catch (MySqlException ex) {
				//Falls die Query Fehlerhaft ist, die MySQL Exception Nummer ausgeben. Die MySQL Exception Nummern kann man nachlesen in gängigen Internet-Tabellen
				Console.WriteLine ("MySQL Fehler: " + ex.Number);
				return false;

			}

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
		public string GetTableNameForColumn (string columnName)
		{

			List<string> liste = new List<string> ();


			try {
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

				return liste [0];
				//Returns a string list of tables related to the column
			} catch (Exception) {
				Console.WriteLine ("GetTableNameForColumn Fehler.");
				return "Fehler";//Noch zu bearbeiten...
			}

		}

		public int GetColumnNumbers (string table)
		{
			List<string> columnNames = GetColumnNames (table);
			return columnNames.Count;
		}
		//Gibt die Bezeichnung der ersten Spalte aus der übergebenen Tabelle zurück
		private string GetFirstColumnName (string table)
		{

			if (GetColumnNumbers (table) >= 0) {
				List<string> columnNames = GetColumnNames (table);
				return columnNames [0];
			} else {
				return "Fehler";//Noch zu bearbeiten...
			}

		}

		private string GetSecondColumnName (string table)
		{
			if (GetColumnNumbers (table) >= 1) {
				List<string> columnNames = GetColumnNames (table);
				return columnNames [1];
			} else {
				return "Fehler";
			}

		}

		private string GetThirdColumnName (string table)
		{
			if (GetColumnNumbers (table) >= 1) {
				List<string> columnNames = GetColumnNames (table);
				return columnNames [2];
			} else {
				return "Fehler";
			}

		}

		private string GetFourthColumnName (string table)
		{
			if (GetColumnNumbers (table) >= 1) {
				List<string> columnNames = GetColumnNames (table);
				return columnNames [3];
			} else {
				return "Fehler";
			}

		}

		private string GetSixthColumnName (string table)
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

