using System;
using System.Data;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Collections.Generic;

namespace MySQL
{
	public class MySQLQueries
	{
		
		private MySqlConnection connection;
		private string server;
		private string database;
		private string uid;
		private string password;
		private string connectionString;

		public MySQLQueries ()
		{
			Initialize ();
		}

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

		public bool ExecuteInsertQuery (string query)
		{
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

		public class Cfy_Log_Queries
		{

			public void cfy_erstellt (string tabelle, int tabelle_id, int objekt_id, int unix_zeitpunkt)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "tabelle," +
				               "tabelle_id," +
				               "objekt_id, " +
				               "unix_zeitpunkt," +
				               "VALUES ('+" +
				               tabelle + "','" +
				               tabelle_id + "','" +
				               objekt_id + "','" +
				               unix_zeitpunkt + "');";

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}


			public void cfy_update (string tabelle, int tabelle_id, int objekt_id, int unix_zeitpunkt)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "tabelle," +
				               "tabelle_id," +
				               "objekt_id, " +
				               "unix_zeitpunkt," +
				               "VALUES ('+" +
				               tabelle + "','" +
				               tabelle_id + "','" +
				               objekt_id + "','" +
				               unix_zeitpunkt + "');";
			
				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}

			public void cfy_geloescht (string tabelle, int tabelle_id, int objekt_id, int unix_zeitpunkt)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "tabelle," +
				               "tabelle_id," +
				               "objekt_id, " +
				               "unix_zeitpunkt," +
				               "VALUES ('+" +
				               tabelle + "','" +
				               tabelle_id + "','" +
				               objekt_id + "','" +
				               unix_zeitpunkt + "');";
				               

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}

			public void cfy_fehler (string tabelle, int tabelle_id, int objekt_id, string beschreibung, int unix_zeitpunkt)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "tabelle," +
				               "tabelle_id," +
				               "objekt_id, " +
				               "beschreibung, " +
				               "unix_zeitpunkt)" +
				               "VALUES ('+" +
				               tabelle + "','" +
				               tabelle_id + "','" +
				               objekt_id + "','" +
				               beschreibung + "','" +
				               unix_zeitpunkt + "');";
				   

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}

			public void cfy_info (string tabelle, int tabelle_id, int objekt_id, string beschreibung, int unix_zeitpunkt)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "tabelle," +
				               "tabelle_id," +
				               "objekt_id, " +
				               "beschreibung, " +
				               "unix_zeitpunkt)" +
				               "VALUES ('+" +
				               tabelle + "','" +
				               tabelle_id + "','" +
				               objekt_id + "','" +
				               beschreibung + "','" +
				               unix_zeitpunkt + "');";

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}
		}

		public class Cfy_Data_Queries
		{

			public void cfy_sq (string signalquelle_id, byte status)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "signalquelle_id," +
				               "status)" +
				               "VALUES ('+" +
				               signalquelle_id + "','" +
				               status + "');";
				          

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}

			public void cfy_sqtyp (string sqtyp, byte status)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "sqtyp," +
				               "status)" +
				               "VALUES ('+" +
				               sqtyp + "','" +
				               status + "');";

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}

			public void cfy_signallieferant (string name, byte status)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "name," +
				               "status)" +
				               "VALUES ('+" +
				               name + "','" +
				               status + "');";

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}

			public void cfy_plz (string plz, byte status)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "plz," +
				               "status)" +
				               "VALUES ('+" +
				               plz + "','" +
				               status + "');";

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}

			public void cfy_ort (string ort, byte status)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "ort," +
				               "status)" +
				               "VALUES ('+" +
				               ort + "','" +
				               status + "');";

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}

			public void cfy_cluster (string cluster_id, byte status)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "cluster_id," +
				               "status)" +
				               "VALUES ('+" +
				               cluster_id + "','" +
				               status + "');";

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}

			public void cfy_cmts (string cmts, byte status)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "cmts," +
				               "status)" +
				               "VALUES ('+" +
				               cmts + "','" +
				               status + "');";

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}

			public void cfy_strasse (string strasse, byte status)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "strasse," +
				               "status)" +
				               "VALUES ('+" +
				               strasse + "','" +
				               status + "');";

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}

			public void cfy_hnr (string hnr, byte status)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "hnr," +
				               "status)" +
				               "VALUES ('+" +
				               hnr + "','" +
				               status + "');";

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}

			public void cfy_gestattungsgeber_daten (string vertragsnummer, byte status)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "vertragsnummer," +
				               "status)" +
				               "VALUES ('+" +
				               vertragsnummer + "','" +
				               status + "');";

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}

			public void cfy_gestattungsgeber_name (string name, byte status)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "name," +
				               "status)" +
				               "VALUES ('+" +
				               name + "','" +
				               status + "');";

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}

			public void cfy_kunden (int soll_we, int subscriber, int internet, int telefon, byte status, int objekt_id)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "soll_we," +
				               "subscriber," +
				               "internet," +
				               "telefon," +
				               "status," +
				               "objekt_id) " +
				               "VALUES ('+" +
				               soll_we + "','" +
				               subscriber + "','" +
				               internet + "','" +
				               telefon + "','" +
				               status + "','" +
				               objekt_id + "');";

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}

			public void cfy_gps (string langengrad, string breitengrad, byte status)
			{
				string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
				               "langengrad," +
				               "breitengrad," +
				               "status) " +
				               "VALUES ('+" +
				               langengrad + "','" +
				               breitengrad + "','" +
				               status + "');";

				MySQLQueries execute = new MySQLQueries ();
				execute.ExecuteInsertQuery (query);
			}
		}

		public void cfy_daten_insert (string signalquelle_id, string type, string kategorie, string cluster_id, string ort, int region, string plz,
		                       string strasse, string hnr, string gestatt_name, string gestatt_vertragsnr, int objekt_id, int soll_we, int subscriber,
		                       int subscriber_int, int subscriber_tel, string status, string cmts, string gps_langengrad, string gps_breitengrad,
		                       string signallieferant)
		{
			string query = "INSERT INTO " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " (" +
			               "signalquelle_id," +
			               "type," +
			               "kategorie," +
			               "cluster_id," +
			               "ort," +
			               "region," +
			               "plz," +
			               "strasse," +
			               "hnr," +
			               "gestatt_name," +
			               "gestatt_vertragsnr," +
			               "objekt_id," +
			               "soll_we," +
			               "subscriber," +
			               "subscriber_int," +
			               "subscriber_tel," +

			               "status," +
			               "cmts," +
			               "gps_langengrad," +
			               "gps_breitengrad," +
			               "signallieferant) " +
			               "VALUES ('" +
			               signalquelle_id + "','" +
			               type + "','" +
			               kategorie + "','" +
			               cluster_id + "','" +
			               ort + "','" +
			               region + "','" +
			               plz + "','" +
			               strasse + "','" +
			               hnr + "','" +
			               gestatt_name + "','" +
			               gestatt_vertragsnr + "','" +
			               objekt_id + "','" +
			               soll_we + "','" +
			               subscriber + "','" +
			               subscriber_int + "','" +
			               subscriber_tel + "','" +

			               status + "','" +
			               cmts + "','" +
			               gps_langengrad + "','" +
			               gps_breitengrad + "','" +
			               signallieferant + "');";


			MySQLQueries execute = new MySQLQueries ();
			execute.ExecuteInsertQuery (query);
		}

		public void cfy_daten_update (string signalquelle_id, string type, string kategorie, string cluster_id, string ort, int region, string plz,
		                       string strasse, string hnr, string gestatt_name, string gestatt_vertragsnr, int objekt_id, int soll_we, int subscriber,
		                       int subscriber_int, int subscriber_tel, string status, string cmts, string gps_langengrad, string gps_breitengrad,
		                       string signallieferant)
		{
			string query = "UPDATE " + System.Reflection.MethodBase.GetCurrentMethod ().Name + " SET " +
			               "signalquelle_id='" + signalquelle_id + "'," +
			               "type ='" + type + "'," +
			               "kategorie ='" + kategorie + "'," +
			               "cluster_id ='" + cluster_id + "'," +
			               "ort ='" + ort + "'," +
			               "region =" + region + "," +
			               "plz ='" + plz + "'," +
			               "strasse='" + strasse + "'," +
			               "hnr='" + hnr + "'," +
			               "gestatt_name='" + gestatt_name + "'," +
			               "gestatt_vertragsnr='" + gestatt_vertragsnr + "'," +
			               "objekt_id=" + objekt_id + "," +
			               "soll_we=" + soll_we + "," +
			               "subscriber=" + subscriber + "," +
			               "subscriber_int=" + subscriber_int + "," +
			               "subscriber_tel=" + subscriber_tel + "," +

			               "status='" + status + "'," +
			               "cmts='" + cmts + "'," +
			               "gps_langengrad='" + gps_langengrad + "'," +
			               "gps_breitengrad='" + gps_breitengrad + "'," +
			               "signallieferant='" + signallieferant + "'" +
			               " WHERE objekt_id = " + objekt_id + ";";


			MySQLQueries execute = new MySQLQueries ();
			execute.ExecuteInsertQuery (query);	
		}
	}
}

