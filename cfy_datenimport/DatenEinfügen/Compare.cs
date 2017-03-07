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
		private List<MEClary.Clary.Clary_List> newData = MEClary.Clary.cfy_rohdaten;

		public Compare ()
		{
			StartCompare ();

		}

		public void StartCompare()
		{
			//Zugriff auf alle Zeilen einer Tabelle: LoadMySQLData.tables["tabellenname"][0 = Spalte 1, oder 1 = Spalte 2][]

			
			if (!compared) {

				foreach (MEClary.Clary.Clary_List item1 in newData) {
					bool wurdeGefunden = false;

					for (int i=0; i< LoadMySQLData.tables["cfy_cluster"][0].Count; i++) {
						if (item1.cluster_id == LoadMySQLData.tables ["cfy_cluster"] [0] [i]) {
							wurdeGefunden = true;
							//Update+Log
							Console.WriteLine ("Gleich");
						} 
					}
					if (!wurdeGefunden) {
						//Insert+Log
						Console.WriteLine ("Nicht gefunden");
					}


				}
				compared = true;
				//newData Löschen
			}
		}



	}
}

