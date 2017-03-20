using System;
using System.Data;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Collections.Generic;
using MEQery;

namespace MySQL
{
	public class MySQLInsertLog:MySQLInsertData
	{
		public MySQLInsertLog ()
		{
			Initialize ();
		}

		public void InsertCreateTimeIntoDaten(string objekt_id)
		{
			int unix_zeitpunkt = 0;
			MEQery.Datum time = new MEQery.Datum ();
			unix_zeitpunkt = time.unix ();

			string query = "UPDATE cfy_daten SET unix_erstellt = '" + unix_zeitpunkt + "' where objekt_id like '" + objekt_id + "';";

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

		public void InsertUpdateTimeIntoDaten(string objekt_id)
		{
			int unix_zeitpunkt = 0;
			MEQery.Datum time = new MEQery.Datum ();
			unix_zeitpunkt = time.unix ();

			string query = "UPDATE cfy_daten SET unix_update = '" + unix_zeitpunkt + "' where objekt_id like '" + objekt_id + "';";

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


		public void InsertIntoErstellt(string tabelle, string tabelle_id, string objekt_id)
		{
			int unix_zeitpunkt = 0;
			MEQery.Datum time = new MEQery.Datum ();
			unix_zeitpunkt = time.unix ();

			List<string> columnNames = GetColumnNames ("cfy_erstellt");

			string query = "INSERT INTO cfy_erstellt (" +
				columnNames[0] + "," +
					columnNames[1] + "," +
					columnNames[2] + "," +
					columnNames[3] + ") " +
					"VALUES ('" +
					tabelle + "','" +
					tabelle_id + "','" +
					objekt_id + "','" +
					unix_zeitpunkt + "');";

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

		public void InsertIntoUpdate(string tabelle, string tabelle_id, string objekt_id)
		{
			int unix_zeitpunkt = 0;
			MEQery.Datum time = new MEQery.Datum ();
			unix_zeitpunkt = time.unix ();

			List<string> columnNames = GetColumnNames ("cfy_update");

			string query = "INSERT INTO cfy_update (" +
				columnNames[0] + "," +
					columnNames[1] + "," +
					columnNames[2] + "," +
					columnNames[3] + ") " +
					"VALUES ('" +
					tabelle + "','" +
					tabelle_id + "','" +
					objekt_id + "','" +
					unix_zeitpunkt + "');";

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

		public void InsertIntoGeloescht(string tabelle, string tabelle_id, string objekt_id)
		{
			int unix_zeitpunkt = 0;
			MEQery.Datum time = new MEQery.Datum ();
			unix_zeitpunkt = time.unix ();

			List<string> columnNames = GetColumnNames ("cfy_geloescht");

			string query = "INSERT INTO cfy_geloescht (" +
				columnNames[0] + "," +
					columnNames[1] + "," +
					columnNames[2] + "," +
					columnNames[3] + ") " +
					"VALUES ('" +
					tabelle + "','" +
					tabelle_id + "','" +
					objekt_id + "','" +
					unix_zeitpunkt + "');";

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

		public void InsertIntoInfo(string tabelle, string tabelle_id, string objekt_id, string beschreibung)
		{
			int unix_zeitpunkt = 0;
			MEQery.Datum time = new MEQery.Datum ();
			unix_zeitpunkt = time.unix ();

			List<string> columnNames = GetColumnNames ("cfy_info");

			string query = "INSERT INTO cfy_info (" +
				columnNames[0] + "," +
					columnNames[1] + "," +
					columnNames[2] + "," +
					columnNames[3] + "," +
					columnNames[4] + ") " +
					"VALUES ('" +
					tabelle + "','" +
					tabelle_id + "','" +
					objekt_id + "','" +
					beschreibung + "','" +
					unix_zeitpunkt + "');";

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

		public void InsertIntoFehler(string tabelle, string tabelle_id, string objekt_id, string beschreibung)
		{
			int unix_zeitpunkt = 0;
			MEQery.Datum time = new MEQery.Datum ();
			unix_zeitpunkt = time.unix ();

			List<string> columnNames = GetColumnNames ("cfy_fehler");

			string query = "INSERT INTO cfy_fehler (" +
				columnNames[0] + "," +
					columnNames[1] + "," +
					columnNames[2] + "," +
					columnNames[3] + "," +
					columnNames[4] + ") " +
					"VALUES ('" +
					tabelle + "','" +
					tabelle_id + "','" +
					objekt_id + "','" +
					beschreibung + "','" +
					unix_zeitpunkt + "');";

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
	}
}

