using System;
using System.Xml;
using System.Net;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NOC
{
	public class TestKlasse
	{//Private Variablen für den Verbindungsstring
		private MySqlConnection connection;
		private string server;
		private string database;
		private string uid;
		private string password;
		private string connectionString;
		private string charset;
		private bool connectionIsOpen = false;
		public static bool test = true;

		//Konstruktor
		public TestKlasse ()
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
			charset = "CHARSET=utf8";
			connectionString = "SERVER=" + server + ";" + "DATABASE=" +
				database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" + charset + ";";
			connection = new MySqlConnection (connectionString);
		}

		//Verbindung öffnen
		public bool OpenConnection ()
		{
			if (!connectionIsOpen) {
				try {
					connection.Open ();
					connectionIsOpen = true;
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
			return false;
		}

		//Verbindung schließen
		public bool CloseConnection ()
		{
			if (connectionIsOpen) {
				try {
					connection.Close ();
					connectionIsOpen = false;
					return true;
				} catch (MySqlException ex) {
					Console.WriteLine (ex.Message);
					return false;
				}
			}
			return false;
		}

		public void Test()
		{
			if (test) {
				string query = "SELECT strasse from cfy_strasse where id = 15161;";
				string testString;
				string testString2;
				System.Text.Encoding utf_8 = System.Text.Encoding.UTF8;

				OpenConnection ();

				MySqlCommand command = new MySqlCommand (query, connection);

				try {
					MySqlDataReader reader = command.ExecuteReader ();

					while (reader.Read ()) {
				
						testString = reader.GetString (0);
						testString2 = "teststraße";
						Console.WriteLine (testString);
						Console.WriteLine(testString2);
						byte[] utf = System.Text.Encoding.UTF8.GetBytes(testString);
						byte[] utf2 = System.Text.Encoding.UTF8.GetBytes(testString2);
						string s2 = System.Text.Encoding.UTF8.GetString(utf);
						string s3 = System.Text.Encoding.UTF8.GetString(utf2);
						Console.WriteLine(s2);
						Console.WriteLine(s3);
						
					}
					reader.Close ();
				} catch (Exception) {
					testString = "Leer";
				}
				test = false;
			}

		}

	}
}

