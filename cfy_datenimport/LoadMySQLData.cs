using System;
using System.Collections.Generic;

namespace MySQL
{
	public class LoadMySQLData
	{
		//ein Dictionary mit zwei Parametern. string für den Tabellennamen und ein Listen Array 
		//mit den Paramtern für Spalte und Zeile.
		private Dictionary<string, List<string>[]> tables = new Dictionary<string, List<string>[]>();

		public void Fill_Tables()
		{
			MySQLGetData get = new MySQLGetData ();
			List<string> table_names = get.GetTableNames();

//			füllen der Tabellen mit den Daten...

			foreach (string item1 in table_names) {
				tables.Add (item1, get.getTableData (item1));
				//break, damit nur die erste Tabelle befüllt wird. Sonst dauerts zu lange...
				//break;
			}

//			Beispiel für den Zugriff auf Daten einer Tabelle: [tabellenname][spaltennummer][zeilennummer]
//			Rückgabewert ist ein string...
//			Die Länge einer Liste z.B. der Spaltenanzahl bekommt man folgendermaßen:
//			List<string>[] mylist = get.getTableData ("cfy_ort");
//			mylist.Length -> Anzahl der Spalten
//			List<string> mylist2 = get.GetTableNames();
//			Console.WriteLine (mylist[1].Count) -> Anzahl der Zeilen in Spalte 1. Spalte 0 ist die erste Spalte im Bereich.

//			Beispiel1:
//			List<string>[] mylist = get.getTableData ("cfy_ort");
//			for (int i = 0; i < (mylist [1].Count / 2); i++) {
//				Console.WriteLine (tables ["cfy_cluster"] [0] [i].ToString ());
//			}
//			Gibt die Hälfte aller Strings in der Tabelle cfy_cluster aus.




		}

	}
}

