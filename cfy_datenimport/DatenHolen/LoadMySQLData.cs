using System;
using System.Collections.Generic;

namespace MySQL
{

	public class LoadMySQLData
	{
		//ein Dictionary mit zwei Parametern. string für den Tabellennamen und ein Listen Array
		//mit den Paramtern für Spalte und Zeile.
		public static Dictionary<string, List<string>[]> tables = new Dictionary<string, List<string>[]> ();
		public static bool datenGeladen = false;
		public static List<string> KeyList = new List<string> ();
		public static long anzahlDatensätze;




		public LoadMySQLData ()
		{
			Fill_Tables ();
			anzahlDatensätze = 0;

		}

		public void Fill_Tables ()
		{
			MySQLGetData get = new MySQLGetData ();
			List<string> table_names = get.GetTableNames ();


//			füllen der Tabellen mit den Daten...

			if (!datenGeladen) {
				
				try {
					Console.Write("Lade Daten Bitte warten");
					foreach (string item1 in table_names) {
						if (item1.Contains("cfy_"))
						{
							if(!(item1.Contains("cfy_rohdaten"))){
								//Console.WriteLine(item1);
								tables.Add (item1, get.getTableData (item1));
								//Console.WriteLine ("...geladen");

								for (int i = 0; i < tables [item1].Length; i++) {

									anzahlDatensätze += tables [item1] [i].Count;
								}
							}

						}


					}
					datenGeladen = true;
					List<string> keys = new List<string> (tables.Keys);
					KeyList = keys;

					Console.WriteLine ("Daten laden abgeschlossen," + anzahlDatensätze + " Datensätze geladen");
				} catch {
					
					datenGeladen = false;
				}


			}

//			Beispiel für den Zugriff auf Daten einer Tabelle: [tabellenname][spaltennummer][zeilennummer]
//			Rückgabewert ist ein string...
//			Die Länge einer Liste z.B. der Spaltenanzahl bekommt man folgendermaßen:
//			mylist.Length -> Anzahl der Spalten
//			mylist[0].Count -> Anzahl der Zeilen in der ersten Spalte



//			Beispiel1:
//			List<string>[] mylist = get.getTableData ("cfy_cluster");
//			for (int i = 0; i < (mylist [1].Count / 2); i++) {
//				Console.WriteLine (tables ["cfy_cluster"] [0] [i].ToString ());
//			}
//			Gibt die Hälfte aller Strings in der ersten Spalte von der Tabelle cfy_cluster aus.




		}


	}
}

