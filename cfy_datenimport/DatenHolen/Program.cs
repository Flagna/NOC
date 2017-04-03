using System;
using System.Collections.Generic;
using MEClary;
using MEQuery;


namespace MySQL
{
	
	public class MySQLDatenImport
	{  
	   /* variable für Steuerung  Threads   */
	  private  bool status = true;

    /* protokoll Klasse & datum  einbinden */
    private static Protokoll protokoll = new Protokoll();
    private static Datum  datum = new Datum();
    private static string proto_woher = "Mysql-CFY-Datenimport";
	  private static string proto_datei = "Program.cs";
    private static string proto_gruppe;
		private static bool geladen = false;
	  

		/* Die Hauptfunktion / Main  vom backend NOC Portal */
		public void mainClaryDatenImport()
		{

			/* hier erst reingehen wenn Status komplett oder empfange geliefert wird von Klasse */
			if(MEClary.Clary.cfy_port_status ==  "empfange") 
			{   

			}
			else if(MEClary.Clary.cfy_port_status ==  "komplett") 
			{
				if (geladen){
				MEClary.Clary.cfy_port_status = "mysql";
					NeueDatenLaden ();
					DatenVergleichen ();
					MEClary.Clary.cfy_port_status = "leer";
				} else {
					DatenLaden ();
				}
			}

		} 

		private void DatenLaden()
		{
			if (!geladen) {
				Protokoll protokoll = new Protokoll ();

				try {
					protokoll.erstellen ("MySQL Program.cs (MySql-Main)", MEClary.Clary.cfy_port_gruppe, "MySQL Daten werden geladen:","Program.cs", "MySQLDatenImport", "mainClaryDatenImport()", false);
					LoadMySQLData load = new LoadMySQLData ();
					protokoll.erstellen ("MySQL Program.cs (MySql-Main)", MEClary.Clary.cfy_port_gruppe, "MySQL Daten fertig geladen:","Program.cs", "MySQLDatenImport", "mainClaryDatenImport()", false);
					geladen = true;
				} catch (Exception e) {
					protokoll.erstellen ("MySQL Program.cs (MySql-Main)", MEClary.Clary.cfy_port_gruppe, "Datenladefehler: /n " + e, "Program.cs","MySQLDatenImport", "mainClaryDatenImport()", false);
				}
			}
		}

		private void NeueDatenLaden()
		{
			try {
				//Neue Daten laden...
				Clary newData = new Clary ();
				newData.rohdaten ("");
			} catch (Exception e) {
				//protokoll.erstellen("Rohdaten holen", );


			}
		}

		private void DatenVergleichen()
		{
			try {
				//Neue mit alten Daten vergleichen
				Compare compare = new Compare ();
			} catch (Exception e) {
				Console.WriteLine (e);
			}
		}

		public void rennen()
		{  
			 
			  
			 Console.WriteLine ( "-------- NOC Portal Clary Daten Import Modul wurde gestartet.---------");
			 /* neue gruppen Nummer generieren aus unix zeitstempel */
			 proto_gruppe = "" + datum.unix();
			 /* Protokoll erstellen */
			 protokoll.erstellen( proto_woher , proto_gruppe , "CFY MYSQL Datenimport Modul wurde gestartet." , proto_datei ,"MySQLDatenImport","rennen()" , false );
 			 while(status)
			 {
				  mainClaryDatenImport();
			 }
			 /* Protokoll erstellen */
			 protokoll.erstellen( proto_woher , proto_gruppe , "CFY MYSQL Datenimport Modul wurde beendet." , proto_datei ,"MySQLDatenImport","rennen()" , false );
		}
		
	  public void anhalten()
    { /* Thread wird gestoppt wenn dieser Funktion aufegrufen wird */
    	
       status = false;
    }
		 
	}
}
