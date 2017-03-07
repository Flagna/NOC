using System;
using System.Collections.Generic;
using MySQL;
using MEClary;

namespace MySQL
{
	//Vergleichsklasse
	public class Compare
	{
		private Dictionary<string, List<string>[]> tables = new Dictionary<string, List<string>[]> ();
		private static bool compared=false;

		public Compare ()
		{
			StartCompare ();

		}

		public void StartCompare()
		{
			
			if (!compared) {

				foreach (string item in LoadMySQLData.KeyList) {
					Console.WriteLine (item);
				}
				compared = true;
			}
		}



	}
}

