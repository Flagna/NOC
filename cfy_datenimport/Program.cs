using System;
using System.Collections.Generic;
using MEClary;
using MEQery;

namespace MySQL
{
	
	public class MySQLDatenImport
	{  
	   /* variable für Steuerung  Threads   */
	  private  bool status = true;

    /* protokoll Klasse & datum  einbinden */
    private static Protokol protokol = new Protokol();
    private static Datum  datum = new Datum();
    private static string proto_woher = "Mysql-CFY-Datenimport";
	  private static string proto_datei = "Program.cs";
    private static string proto_gruppe;
	  
		
		/* Die Hauptfunktion / Main  vom backend NOC Portal */
		public void mainClaryDatenImport()
		{

			/* hier erst reingehen wenn Status komplett oder empfange geliefert wird von Klasse */
			if( MEClary.Clary.cfy_port_status ==  "komplett") 
			{   
				   /* Protokoll erstellen */
				   protokol.erstellen( proto_woher , proto_gruppe , "Neue daten sind angekommen vom Listener Status: " + MEClary.Clary.cfy_port_status , proto_datei ,"MySQLDatenImport","mainClaryDatenImport()" , false );
				   
			     LoadMySQLData load = new LoadMySQLData ();
		 	
			     load.Fill_Tables ();
			     /*
			        if (MEClary.Clary.cfy_port_status == "empfangen") {
				
		         	}

			        while (true) {
		       		if (MEClary.Clary.cfy_port_status == "komplett") {
					    break;
				        }
			        }
			     */


			     //Daten laden WICHTIG

			     //Vergleichen und Update oder Insert in die Rohdaten-Tabelle machen.
			     //mysqlcompare.Compare ();
			
			     /* Status ändern und Listener wieder frei geben das dieser neue Daten empfangen kann */
			     MEClary.Clary.cfy_port_status = "leer";
			     /* Protokoll erstellen */
			     protokol.erstellen( proto_woher , proto_gruppe , "Daten wurden erfolgreich in MYSQL integriert ( CFY) warte auf neue Daten." , proto_datei ,"MySQLDatenImport","mainClaryDatenImport()" , false );
			     /* neue gruppen Nummer generieren aus unix zeitstempel */
			     proto_gruppe = "" + datum.unix();
			}
			else {}

		} 
		
		public void rennen()
		{  
			 
			  
			 Console.WriteLine ( "-------- NOC Portal Clary Daten Import Modul wurde gestartet.---------");
			 /* neue gruppen Nummer generieren aus unix zeitstempel */
			 proto_gruppe = "" + datum.unix();
			 /* Protokoll erstellen */
			 protokol.erstellen( proto_woher , proto_gruppe , "CFY MYSQL Datenimport Modul wurde gestartet." , proto_datei ,"MySQLDatenImport","rennen()" , false );
 			 while(status)
			 {
				  mainClaryDatenImport();
			 }
			 /* Protokoll erstellen */
			 protokol.erstellen( proto_woher , proto_gruppe , "CFY MYSQL Datenimport Modul wurde beendet." , proto_datei ,"MySQLDatenImport","rennen()" , false );
		}
		
	  public void anhalten()
    { /* Thread wird gestoppt wenn dieser Funktion aufegrufen wird */
    	
       status = false;
    }
		 
	}
}
