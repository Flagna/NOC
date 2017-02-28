using System;
using System.Net;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Collections.Generic;


namespace MySQL
{
	public class MySQLGetData
	{
		//Private Variablen für den Verbindungsstring
		private MySqlConnection connection;
		private string server;
		private string database;
		private string uid;
		private string password;
		private string connectionString;

		//Konstruktor
		public MySQLGetData ()
		{
			Initialize ();
		}

		//Initialisieren des Verbindungsstrings
		private void Initialize ()
		{
			server = "localhost";
			database = "noc_portal";
			uid = "root";
			password = "";
			connectionString = "SERVER=" + server + ";" + "DATABASE=" +
			database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
			connection = new MySqlConnection (connectionString);
		}

		//Verbindung öffnen
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

		//Verbindung schließen
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

		//Prüft ob der Tabellenname exisitiert und gibt entsprechend true oder false zurück
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

		//Gibt eine Array-Liste mit den Tabellendaten zurück.
		//Zugriff Beispiel getTableData("cfy_sq")
		//Rückgabeliste und der Zugriff darauf.
		//List<string>[] data = new List<string>[];
		//data[0] <-- Erste Spalte
		//foreach (string item in data){
		//data[0].getString() <-- Zugriffsbeispiel

		public List<string>[] getTableData (string table_name)
		{
			
			int column_length = GetColumnNames (table_name).Count;
			List<string>[] a = new List<string>[column_length];

			if (CheckTableName (table_name)) {

				string selectquery = "select * from " + table_name;

				OpenConnection ();

				MySqlCommand command = new MySqlCommand (selectquery, connection);

				for (int i = 0; i < column_length; i++) {
					try {
						MySqlDataReader reader = command.ExecuteReader ();
					
						a [i] = new List<string> ();
						while (reader.Read ()) {
							try {
								a [i].Add (reader.GetString (i));
							} catch (Exception) {
								a [i].Add ("");
							}
						
						}
						reader.Close ();
					} catch (Exception) {
						return a;
					}
				}

				
				CloseConnection ();
				return a;

			} else {
				Console.WriteLine ("Table doesn't exist");
				return a;
			}

			//Returns an array-list of Table Data
			//Usage: 
//			MySQLGetData get = new MySQLGetData ();
//			List<string>[] mylist = get.getTableData ("cfy_sq");
//			Console.WriteLine ("Rank: "+mylist.Length);
//			for (int i = 0; i < mylist.Length; i++) {
//				foreach (string item1 in mylist[i]) {
//					Console.WriteLine (item1);
//				}
//			}


		}
		//Gibt eine Liste mit den Tabellennamen zurück.(Auch den Namen der View)
		public List<string> GetTableNames ()
		{

			List<string> table_names = new List<string> ();
			string getschemaquery = "SHOW TABLES;";

			//MySql Connection anlegen.
			OpenConnection ();
			MySqlCommand command = new MySqlCommand (getschemaquery, connection);

			//Reader starten
			MySqlDataReader reader = command.ExecuteReader ();
			//Reader liest Zeilenweise in der ersten Spalte (0)
			while (reader.Read ()) {
				table_names.Add (reader.GetString (0));
			}
			//Reader schließen
			reader.Close ();
			//Verbindung schließen
			CloseConnection ();
			return table_names;
		}

	}
}

