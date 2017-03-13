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
		private static List<int> idList = new List<int> ();
		private long insertCounter = 0;
		private long updateCounter = 0;

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

					for (int i=0; i< LoadMySQLData.tables["cfy_sq"][0].Count; i++) {
						if (item1.signalquelle_id == LoadMySQLData.tables ["cfy_sq"] [0] [i]) {
							wurdeGefunden = true;

						} 
					}
					if (!wurdeGefunden) {

						//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
						//auf aktiv gesetzt und ... 
						insert.Insert ("cfy_sq",item1.signalquelle_id.ToString());

						insertCounter = insertCounter + 1;


						//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
						LoadMySQLData.tables ["cfy_sq"] [0].Add (item1.signalquelle_id.ToString());
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
							Console.WriteLine ("Gefunden");
							wurdeGefunden = true;
						} 
					}
					if (!wurdeGefunden) {

						//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
						//auf aktiv gesetzt und ... 
						insert.Insert ("cfy_strasse",item1.strasse.ToString());
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
						insertCounter = insertCounter + 1;

						//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
						LoadMySQLData.tables ["cfy_gestattungsgeber_daten"] [0].Add (item1.gestatt_vertragsn.ToString());
						//LOG schreiben
					}


				}


				StartCompareTwoValued ();
				StartCompareMultiValued ();
				getIdDaten ();
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

							insert.UpdateCustomer ("cfy_kunden", item1.objekt_id.ToString());
							updateCounter = updateCounter + 1;
							insert.Insert ("cfy_kunden",
							               item1.soll_we.ToString(), 
							               item1.subscriber.ToString(),
							               item1.subscriber_int.ToString(),
							               item1.subscriber_tel.ToString(),
							               item1.objekt_id.ToString());

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
			bool status = false;

			//DATEN
			foreach (MEClary.Clary.Clary_List item1 in MEClary.Clary.cfy_rohdaten) {
				bool wurdeGefunden = false;

				for (int i=0; i< LoadMySQLData.tables["cfy_daten"][5].Count; i++) {


					if (item1.objekt_id.ToString() == LoadMySQLData.tables ["cfy_daten"] [5] [i]) {


						wurdeGefunden = true;
					} 
				}
				if (!wurdeGefunden) {
					//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
					//auf aktiv gesetzt und ... 
					//insert.Insert ("","");
					//insertCounter += 1;

					//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach eingefügt.
					//LoadMySQLData.tables ["cfy_kunden"] [0].Add ("");


					//LOG schreiben
				}
			}

		}

		private void getIdDaten()
		{
			for (int i=0; i< LoadMySQLData.tables["cfy_daten"][5].Count; i++) {
				Console.WriteLine(LoadMySQLData.tables ["cfy_daten"] [13] [i]);
			}
		}


	}
}

