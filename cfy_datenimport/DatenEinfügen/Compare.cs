using System;
using System.Collections.Generic;
using MySQL;
using MEClary;

namespace MySQL
{
	//Vergleichsklasse
	public class Compare
	{

		private static bool compared=false;
		private static MySQLInsertData insert = new MySQLInsertData ();

		private long insertCounter = 0;
		private long updateCounter = 0;
		private MySQLInsertLog log = new MySQLInsertLog ();
		private MySQLGetData get = new MySQLGetData ();

		public Compare ()
		{
			StartCompareOneValued ();

		}

		//Vergleich bei einwertigen Parametern z.B. cluster_id
		public void StartCompareOneValued()
		{
			//Zugriff auf alle Zeilen einer Tabelle: LoadMySQLData.tables["tabellenname"][0 = Spalte 1, oder 1 = Spalte 2][]

			
			if (!compared) {


				//SIGNALQUELLE_ID
				foreach (MEClary.Clary.Clary_List item1 in MEClary.Clary.cfy_rohdaten) {
					bool wurdeGefunden = false;

					//Alle auf inaktiv setzen. 

					for (int i=0; i< LoadMySQLData.tables["cfy_sq"][0].Count; i++) {
						if (item1.signalquelle_id == LoadMySQLData.tables ["cfy_sq"] [0] [i]) {
							//Auf Aktiv setzen, AUCH in der Liste
							wurdeGefunden = true;

						} 
					}
					if (!wurdeGefunden) {

						//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
						//auf aktiv gesetzt und ... 
						insert.Insert ("cfy_sq",item1.signalquelle_id.ToString());
						log.InsertIntoErstellt ("cfy_sq", get.GetIdfromTable ("cfy_sq", item1.objekt_id.ToString()),item1.objekt_id.ToString());

						insertCounter = insertCounter + 1;


						//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
						LoadMySQLData.tables ["cfy_sq"] [0].Add (item1.signalquelle_id.ToString());
						//LOG schreiben
					}


				}

				//SIGNALLIEFERANT
				foreach (MEClary.Clary.Clary_List item1 in MEClary.Clary.cfy_rohdaten) {
					bool wurdeGefunden = false;

					for (int i=0; i< LoadMySQLData.tables["cfy_signallieferant"][0].Count; i++) {
						if (item1.signallieferant == LoadMySQLData.tables ["cfy_signallieferant"] [0] [i]) {
							wurdeGefunden = true;

						} 
					}
					if (!wurdeGefunden) {

						//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
						//auf aktiv gesetzt und ... 
						insert.Insert ("cfy_signallieferant",item1.signallieferant.ToString());
						log.InsertIntoErstellt ("cfy_signallieferant", get.GetIdfromTable ("cfy_signallieferant", item1.objekt_id.ToString()),item1.objekt_id.ToString());

						insertCounter = insertCounter + 1;


						//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
						LoadMySQLData.tables ["cfy_signallieferant"] [0].Add (item1.signallieferant.ToString());
						//LOG schreiben
					}


				}

				//CLUSTER_ID
				foreach (MEClary.Clary.Clary_List item1 in MEClary.Clary.cfy_rohdaten) {
					bool wurdeGefunden = false;

					for (int i=0; i< LoadMySQLData.tables["cfy_cluster"][0].Count; i++) {
						if (item1.cluster_id == LoadMySQLData.tables ["cfy_cluster"] [0] [i]) {
							wurdeGefunden = true;
						} 
					}
					if (!wurdeGefunden) {

						//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
						//auf aktiv gesetzt und ... 
						insert.Insert ("cfy_cluster",item1.cluster_id.ToString());
						log.InsertIntoErstellt ("cfy_cluster", get.GetIdfromTable ("cfy_cluster", item1.objekt_id.ToString()),item1.objekt_id.ToString());

						insertCounter = insertCounter + 1;


						//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
						LoadMySQLData.tables ["cfy_cluster"] [0].Add (item1.cluster_id.ToString());
						//LOG schreiben
					}


				}
				//CMTS
				foreach (MEClary.Clary.Clary_List item1 in MEClary.Clary.cfy_rohdaten) {
					bool wurdeGefunden = false;

					for (int i=0; i< LoadMySQLData.tables["cfy_cmts"][0].Count; i++) {
						if (item1.cmts == LoadMySQLData.tables ["cfy_cmts"] [0] [i]) {
							wurdeGefunden = true;
						} 
					}
					if (!wurdeGefunden) {

						//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
						//auf aktiv gesetzt und ... 
						insert.Insert ("cfy_cmts",item1.cmts.ToString());
						log.InsertIntoErstellt ("cfy_cmts", get.GetIdfromTable ("cfy_cmts", item1.objekt_id.ToString()),item1.objekt_id.ToString());

						insertCounter = insertCounter + 1;


						//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
						LoadMySQLData.tables ["cfy_cmts"] [0].Add (item1.cmts.ToString());
						//LOG schreiben
					}


				}

				//TYPE
				foreach (MEClary.Clary.Clary_List item1 in MEClary.Clary.cfy_rohdaten) {
					bool wurdeGefunden = false;

					for (int i=0; i< LoadMySQLData.tables["cfy_sqtyp"][0].Count; i++) {
						if (item1.type == LoadMySQLData.tables ["cfy_sqtyp"] [0] [i]) {
							wurdeGefunden = true;
						} 
					}
					if (!wurdeGefunden) {

						//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
						//auf aktiv gesetzt und ... 
						insert.Insert ("cfy_sqtyp",item1.type.ToString());
						log.InsertIntoErstellt ("cfy_sqtyp", get.GetIdfromTable ("cfy_sqtyp", item1.objekt_id.ToString()),item1.objekt_id.ToString());

						insertCounter = insertCounter + 1;


						//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
						LoadMySQLData.tables ["cfy_sqtyp"] [0].Add (item1.type.ToString());
						//LOG schreiben
					}


				}

				//PLZ
				foreach (MEClary.Clary.Clary_List item1 in MEClary.Clary.cfy_rohdaten) {
					bool wurdeGefunden = false;

					for (int i=0; i< LoadMySQLData.tables["cfy_plz"][0].Count; i++) {
						if (item1.plz == LoadMySQLData.tables ["cfy_plz"] [0] [i]) {
							wurdeGefunden = true;
						} 
					}
					if (!wurdeGefunden) {

						//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
						//auf aktiv gesetzt und ... 
						insert.Insert ("cfy_plz",item1.plz.ToString());
						log.InsertIntoErstellt ("cfy_plz", get.GetIdfromTable ("cfy_plz", item1.objekt_id.ToString()),item1.objekt_id.ToString());

						insertCounter = insertCounter + 1;


						//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
						LoadMySQLData.tables ["cfy_plz"] [0].Add (item1.plz.ToString());
						//LOG schreiben
					}


				}
				//ORT
				foreach (MEClary.Clary.Clary_List item1 in MEClary.Clary.cfy_rohdaten) {
					bool wurdeGefunden = false;

					for (int i=0; i< LoadMySQLData.tables["cfy_ort"][0].Count; i++) {
						if (item1.ort == LoadMySQLData.tables ["cfy_ort"] [0] [i]) {
							wurdeGefunden = true;
						} 
					}
					if (!wurdeGefunden) {

						//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
						//auf aktiv gesetzt und ... 
						insert.Insert ("cfy_ort",item1.ort.ToString());
						log.InsertIntoErstellt ("cfy_ort", get.GetIdfromTable ("cfy_ort", item1.objekt_id.ToString()),item1.objekt_id.ToString());

						insertCounter = insertCounter + 1;


						//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
						LoadMySQLData.tables ["cfy_ort"] [0].Add (item1.ort.ToString());
						//LOG schreiben
					}


				}
				//STRASSE
				foreach (MEClary.Clary.Clary_List item1 in MEClary.Clary.cfy_rohdaten) {
					bool wurdeGefunden = false;

					for (int i=0; i< LoadMySQLData.tables["cfy_strasse"][0].Count; i++) {
						if (item1.strasse.ToString() == LoadMySQLData.tables ["cfy_strasse"] [0] [i].ToString()) {
							wurdeGefunden = true;
						} 
					}
					if (!wurdeGefunden) {

						//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
						//auf aktiv gesetzt und ... 
						insert.Insert ("cfy_strasse",item1.strasse.ToString());
						log.InsertIntoErstellt ("cfy_strasse", get.GetIdfromTable ("cfy_strasse", item1.objekt_id.ToString()),item1.objekt_id.ToString());

						insertCounter = insertCounter + 1;

						//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
						LoadMySQLData.tables ["cfy_strasse"] [0].Add (item1.strasse.ToString());
						//LOG schreiben
					}


				}
				//HNR
				foreach (MEClary.Clary.Clary_List item1 in MEClary.Clary.cfy_rohdaten) {
					bool wurdeGefunden = false;

					for (int i=0; i< LoadMySQLData.tables["cfy_hnr"][0].Count; i++) {
						if (item1.hnr == LoadMySQLData.tables ["cfy_hnr"] [0] [i]) {
							wurdeGefunden = true;
						} 
					}
					if (!wurdeGefunden) {

						//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
						//auf aktiv gesetzt und ... 
						insert.Insert ("cfy_hnr",item1.hnr.ToString());
						log.InsertIntoErstellt ("cfy_hnr", get.GetIdfromTable ("cfy_hnr", item1.objekt_id.ToString()),item1.objekt_id.ToString());

						insertCounter = insertCounter + 1;

						//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
						LoadMySQLData.tables ["cfy_hnr"] [0].Add (item1.hnr.ToString());
						//LOG schreiben
					}


				}

				//GESTATT_NAME
				foreach (MEClary.Clary.Clary_List item1 in MEClary.Clary.cfy_rohdaten) {
					bool wurdeGefunden = false;

					for (int i=0; i< LoadMySQLData.tables["cfy_gestattungsgeber_name"][0].Count; i++) {
						if (item1.gestatt_name == LoadMySQLData.tables ["cfy_gestattungsgeber_name"] [0] [i]) {
							wurdeGefunden = true;
						} 
					}
					if (!wurdeGefunden) {

						//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
						//auf aktiv gesetzt und ... 
						insert.Insert ("cfy_gestattungsgeber_name",item1.gestatt_name.ToString());
						log.InsertIntoErstellt ("cfy_gestattungsgeber_name", get.GetIdfromTable ("cfy_gestattungsgeber_name", item1.objekt_id.ToString()),item1.objekt_id.ToString());
						insertCounter = insertCounter + 1;

						//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
						LoadMySQLData.tables ["cfy_gestattungsgeber_name"] [0].Add (item1.gestatt_name.ToString());
						//LOG schreiben
					}


				}

				//GESTATT_VERTRAGSN
				foreach (MEClary.Clary.Clary_List item1 in MEClary.Clary.cfy_rohdaten) {
					bool wurdeGefunden = false;

					for (int i=0; i< LoadMySQLData.tables["cfy_gestattungsgeber_daten"][0].Count; i++) {
						if (item1.gestatt_vertragsn == LoadMySQLData.tables ["cfy_gestattungsgeber_daten"] [0] [i]) {
							wurdeGefunden = true;
						} 
					}
					if (!wurdeGefunden) {

						//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
						//auf aktiv gesetzt und ... 
						insert.Insert ("cfy_gestattungsgeber_daten",item1.gestatt_vertragsn.ToString());

						log.InsertIntoErstellt ("cfy_gestattungsgeber_daten", get.GetIdfromTable ("cfy_gestattungsgeber_daten", item1.objekt_id.ToString()),item1.objekt_id.ToString());
						insertCounter = insertCounter + 1;

						//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
						LoadMySQLData.tables ["cfy_gestattungsgeber_daten"] [0].Add (item1.gestatt_vertragsn.ToString());
						//LOG schreiben
					}


				}


				StartCompareTwoValued ();
				StartCompareMultiValued ();
				StartCompareDataTable ();
				compared = true;
				Console.Write ("Vergleich abgeschlossen... Neu: ");
				Console.Write (insertCounter);
				Console.Write("... Geändert: ");
				Console.WriteLine (updateCounter);


				//newData Löschen
			}
		}

		private void StartCompareTwoValued()
		{

			MySQLGetData get = new MySQLGetData ();

			//GPS_LANGENGRAD + GPS_BREITENGRAD
			foreach (MEClary.Clary.Clary_List item1 in MEClary.Clary.cfy_rohdaten) {
				bool wurdeGefunden = false;

				for (int i=0; i< LoadMySQLData.tables["cfy_gps"][0].Count; i++) {
					if (item1.gps_langengrad == LoadMySQLData.tables ["cfy_gps"] [0] [i] && 
					    item1.gps_breitengrad == LoadMySQLData.tables ["cfy_gps"] [1] [i]) {
						wurdeGefunden = true;
					} 
				}
				if (!wurdeGefunden) {
					//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
					//auf aktiv gesetzt und ... 
					insert.Insert ("cfy_gps",item1.gps_langengrad.ToString(), item1.gps_breitengrad.ToString());

					log.InsertIntoErstellt ("cfy_gps", get.GetIdfromTable ("cfy_gps", item1.objekt_id.ToString()),item1.objekt_id.ToString());
					insertCounter = insertCounter + 1;

					//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
					LoadMySQLData.tables ["cfy_gps"] [0].Add (item1.gps_langengrad.ToString());
					LoadMySQLData.tables ["cfy_gps"] [1].Add (item1.gps_breitengrad.ToString());
					//LOG schreiben
				}
			}
		}

		private void StartCompareMultiValued()
		{
			bool status = false;
			MySQLGetData get = new MySQLGetData ();

			//KUNDEN
			foreach (MEClary.Clary.Clary_List item1 in MEClary.Clary.cfy_rohdaten) {
				bool wurdeGefunden = false;

				for (int i=0; i< LoadMySQLData.tables["cfy_kunden"][5].Count; i++) {


					if (item1.objekt_id.ToString() == LoadMySQLData.tables ["cfy_kunden"] [5] [i]) {

						if ((item1.soll_we.ToString() != LoadMySQLData.tables ["cfy_kunden"] [0] [i].ToString() ||
						    item1.subscriber.ToString() != LoadMySQLData.tables ["cfy_kunden"] [1] [i].ToString() ||
						    item1.subscriber_int.ToString() != LoadMySQLData.tables ["cfy_kunden"] [2] [i].ToString() ||
						    item1.subscriber_tel.ToString() != LoadMySQLData.tables ["cfy_kunden"] [3] [i].ToString()) && 
						    status.ToString() == LoadMySQLData.tables ["cfy_kunden"] [4] [i].ToString())
						{
							//Status auf 1 setzen

							//Update Counter erhöhen

							insert.Insert ("cfy_kunden",
							               item1.soll_we.ToString(), 
							               item1.subscriber.ToString(),
							               item1.subscriber_int.ToString(),
							               item1.subscriber_tel.ToString(),
							               item1.objekt_id.ToString());
							insert.UpdateCustomer ("cfy_kunden", item1.objekt_id.ToString ());

							log.InsertIntoUpdate ("cfy_kunden",get.GetIdfromTable ("cfy_kunden", item1.objekt_id.ToString()),item1.objekt_id.ToString());
							updateCounter = updateCounter + 1;

						}
						wurdeGefunden = true;
					} 
				}
				if (!wurdeGefunden) {
					//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
					//auf aktiv gesetzt und ... 
					insert.Insert ("cfy_kunden",
					               item1.soll_we.ToString(), 
					               item1.subscriber.ToString(),
					               item1.subscriber_int.ToString(),
					               item1.subscriber_tel.ToString(),
					               item1.objekt_id.ToString());
					insertCounter += 1;
					log.InsertIntoErstellt ("cfy_kunden", get.GetIdfromTable ("cfy_kunden", item1.objekt_id.ToString()),item1.objekt_id.ToString());


					//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
					LoadMySQLData.tables ["cfy_kunden"] [0].Add (item1.soll_we.ToString());
					LoadMySQLData.tables ["cfy_kunden"] [1].Add (item1.subscriber.ToString());
					LoadMySQLData.tables ["cfy_kunden"] [2].Add (item1.subscriber_int.ToString());
					LoadMySQLData.tables ["cfy_kunden"] [3].Add (item1.subscriber_tel.ToString());
					LoadMySQLData.tables ["cfy_kunden"] [4].Add (status.ToString());
					LoadMySQLData.tables ["cfy_kunden"] [5].Add (item1.objekt_id.ToString());

					//LOG schreiben
				}
			}
		}

		private void StartCompareDataTable()
		{
			MySQLGetData get = new MySQLGetData ();
			bool status = false;
			//Daten neu Laden. Weil die IDs neu gesetzt werden müssen(In den einzelnen Tabellen)
			//LoadMySQLData load = new LoadMySQLData ();
			//Daten geladen...
			/*edit Es werden nur Datenbankabfragen für jede ID ausgeführt. Geht schneller.*/
			//DATEN
			foreach (MEClary.Clary.Clary_List item1 in MEClary.Clary.cfy_rohdaten) {
				bool wurdeGefunden = false;
				for (int i=0; i< LoadMySQLData.tables["cfy_daten"][13].Count; i++) {
				

					if (item1.objekt_id.ToString() == LoadMySQLData.tables ["cfy_daten"] [13] [i] && LoadMySQLData.tables ["cfy_daten"] [16] [i] == "False") {
						if (get.GetIdfromTable ("cfy_sq", item1.signalquelle_id) == "" || 
							get.GetIdfromTable ("cfy_sqtyp", item1.type) == "" || 
							get.GetIdfromTable ("cfy_signallieferant", item1.signallieferant) == "" ||
							get.GetIdfromTable ("cfy_cmts", item1.cmts) == "" ||
							get.GetIdfromTable ("cfy_cluster", item1.cluster_id) == "" ||
							get.GetIdfromTable ("cfy_ort", item1.ort) == "" ||
							get.GetIdfromTable ("cfy_plz", item1.plz) == "" ||
							get.GetIdfromTable ("cfy_strasse", item1.strasse) == "" ||
							get.GetIdfromTable ("cfy_hnr", item1.hnr) == "" ||
							get.GetIdfromTable ("cfy_gestattungsgeber_daten", item1.gestatt_vertragsn) == "" ||
							get.GetIdfromTable ("cfy_gestattungsgeber_name", item1.gestatt_name) == "" ||
							get.GetIdfromTable ("cfy_gps", item1.gps_langengrad, item1.gps_breitengrad) == "" ||
							get.GetIdfromTable (item1.objekt_id.ToString ()) == "") {

							string[] idList = new string[15];
							idList [0] = get.GetIdfromTable ("cfy_sq", item1.signalquelle_id);
							idList [1] = get.GetIdfromTable ("cfy_sqtyp", item1.type);
							idList [2] = get.GetIdfromTable ("cfy_signallieferant", item1.signallieferant);
							idList [3] = get.GetIdfromTable ("cfy_cmts", item1.cmts);
							idList [4] = get.GetIdfromTable ("cfy_cluster", item1.cluster_id);
							idList [5] = get.GetIdfromTable ("cfy_ort", item1.ort);
							idList [6] = get.GetIdfromTable ("cfy_plz", item1.plz);
							idList [7] = get.GetIdfromTable ("cfy_strasse", item1.strasse);
							idList [8] = get.GetIdfromTable ("cfy_hnr", item1.hnr);
							idList [9] = get.GetIdfromTable ("cfy_gestattungsgeber_daten", item1.gestatt_vertragsn);
							idList [10] = get.GetIdfromTable ("cfy_gestattungsgeber_name", item1.gestatt_name);
							idList [11] = item1.objekt_id.ToString ();
							idList [12] = get.GetIdfromTable ("cfy_gps", item1.gps_langengrad, item1.gps_breitengrad);
							idList [13] = get.GetIdfromTable (item1.objekt_id.ToString ());
							idList [14] = item1.region.ToString ();
							insert.InsertIntoData (idList);

							insert.UpdateCustomer ("cfy_daten", item1.objekt_id.ToString ());

							log.InsertIntoUpdate ("cfy_daten",idList[13],idList[11]);
							updateCounter = updateCounter + 1;

						}
						wurdeGefunden = true; //ObjektId existiert bereits

					} 

				}
				if (!wurdeGefunden) {
					//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
					//auf aktiv gesetzt und ... 
					//insert.Insert ("","");
					//insertCounter += 1;

					//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
					//LoadMySQLData.tables ["cfy_kunden"] [0].Add ("");

					string[] idList = new string[15];
					idList [0] = get.GetIdfromTable ("cfy_sq", item1.signalquelle_id);
					idList [1] = get.GetIdfromTable ("cfy_sqtyp", item1.type);
					idList [2] = get.GetIdfromTable ("cfy_signallieferant", item1.signallieferant);
					idList [3] = get.GetIdfromTable ("cfy_cmts", item1.cmts);
					idList [4] = get.GetIdfromTable ("cfy_cluster", item1.cluster_id);
					idList [5] = get.GetIdfromTable ("cfy_ort", item1.ort);
					idList [6] = get.GetIdfromTable ("cfy_plz", item1.plz);
					idList [7] = get.GetIdfromTable ("cfy_strasse", item1.strasse);
					idList [8] = get.GetIdfromTable ("cfy_hnr", item1.hnr);
					idList [9] = get.GetIdfromTable ("cfy_gestattungsgeber_daten", item1.gestatt_vertragsn);
					idList [10] = get.GetIdfromTable ("cfy_gestattungsgeber_name", item1.gestatt_name);
					idList [11] = item1.objekt_id.ToString();
					idList [12] = get.GetIdfromTable ("cfy_gps", item1.gps_langengrad, item1.gps_breitengrad);
					idList [13] = get.GetIdfromTable (item1.objekt_id.ToString());
					idList [14] = item1.region.ToString ();
					insert.InsertIntoData (idList);


					log.InsertIntoErstellt ("cfy_daten", idList[13], idList[11]);
					log.InsertCreateTimeIntoDaten (idList[11]);

					insertCounter += 1;

					//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
					LoadMySQLData.tables ["cfy_daten"] [0].Add (idList [0]);
					LoadMySQLData.tables ["cfy_daten"] [1].Add (idList [1]);
					LoadMySQLData.tables ["cfy_daten"] [3].Add (idList [2]);
					LoadMySQLData.tables ["cfy_daten"] [4].Add (idList [3]);
					LoadMySQLData.tables ["cfy_daten"] [5].Add (idList [4]);
					LoadMySQLData.tables ["cfy_daten"] [6].Add (idList [5]);
					LoadMySQLData.tables ["cfy_daten"] [7].Add (idList [6]);
					LoadMySQLData.tables ["cfy_daten"] [8].Add (idList [7]);
					LoadMySQLData.tables ["cfy_daten"] [9].Add (idList [8]);
					LoadMySQLData.tables ["cfy_daten"] [10].Add (idList [9]);
					LoadMySQLData.tables ["cfy_daten"] [11].Add (idList [10]);
					LoadMySQLData.tables ["cfy_daten"] [13].Add (idList [11]);
					LoadMySQLData.tables ["cfy_daten"] [14].Add (idList [12]);
					LoadMySQLData.tables ["cfy_daten"] [15].Add (idList [13]);
					LoadMySQLData.tables ["cfy_daten"] [21].Add (idList [14]);
				}
			}

		}


	}
}

