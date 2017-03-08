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

		public Compare ()
		{
			StartCompare ();

		}

		public void StartCompare()
		{
			//Zugriff auf alle Zeilen einer Tabelle: LoadMySQLData.tables["tabellenname"][0 = Spalte 1, oder 1 = Spalte 2][]

			
			if (!compared) {

				MySQLInsertData insert = new MySQLInsertData ();

				foreach (MEClary.Clary.Clary_List item1 in MEClary.Clary.cfy_rohdaten) {
					bool wurdeGefunden = false;
					//foreach(string item2 in LoadMySQLData.KeyList)
					for (int i=0; i< LoadMySQLData.tables["cfy_cluster"][0].Count; i++) {
						if (item1.cluster_id == LoadMySQLData.tables ["cfy_cluster"] [0] [i]) {
							wurdeGefunden = true;
							//Update+Log

						} 
					}
					if (!wurdeGefunden) {
						var myVar = 

						//Falls ein Datensatz nicht gefunden wurde, wird er eingefügt, gleichzeitig
						//auf aktiv gesetzt und ... 
						string tableName = insert.GetTableNameForColumn ("cluster_id");
						insert.Insert (tableName,item1.cluster_id.ToString());
						//...der Dictionary Liste hinzugefügt. Sonst wird der gleiche Datensatz mehrfach 
						LoadMySQLData.tables [tableName] [0].Add (item1.cluster_id.ToString());
						//LOG schreiben
					}


				}
				compared = true;
				//newData Löschen
			}
		}





	}
}

