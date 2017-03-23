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

    
    /* Datum */
    private static Datum  datum = new Datum();
    
    /* protokoll  */
    private static Protokoll protokoll = new Protokoll();
    private Debuger debuger = new Debuger();
    private  string proto_woher   = "Mysql-CFY-Datenimport";
    private  string  proto_klasse = "MySQLDatenImport";
    private string proto_gruppe;
	  
	  

		/* Die Hauptfunktion / Main  vom backend NOC Portal */
		public void mainClaryDatenImport()
		{

			/* hier erst reingehen wenn Status komplett oder empfange geliefert wird von Klasse */
			if(Clary.cfy_port_status == "mysql")//MEClary.Clary.cfy_port_status ==  "empfange") 
			{ 
				
				 Clary.cfy_port_status = "leer";
         Console.WriteLine ( "-------- Erledigt CFY Daten wurden in DB Geschrieben ---- ");
				 
				//MEQuery.Protokoll protokolll = new MEQuery.Protokoll ();

				 protokoll.erstellen( proto_woher , proto_gruppe , "Neue daten sind angekommen vom Listener Status: " + MEClary.Clary.cfy_port_status , proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
				

				/*
				try {
					//Neue Daten laden...
					Clary newData = new Clary ();
					newData.rohdaten ("");
				} catch (Exception e) {
					//protokoll.erstellen("Rohdaten holen", );

					
				}
				try {
					//Datenbankdaten laden
					LoadMySQLData load = new LoadMySQLData ();
				} catch (Exception e) {
					Console.WriteLine (e);
				}
				try {
					//Neue mit alten Daten vergleichen
					Compare compare = new Compare ();
				} catch (Exception e) {
					Console.WriteLine (e);
				}
			 
			
			     // Status ändern und Listener wieder frei geben das dieser neue Daten empfangen kann

			*/     
			     /* Protokoll erstellen */
			     protokoll.erstellen( proto_woher , proto_gruppe , "Daten wurden erfolgreich in MYSQL integriert ( CFY) warte auf neue Daten." , proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
			     /* neue gruppen Nummer generieren aus unix zeitstempel */
			     //proto_gruppe = "" + datum.unix();
			}
			else if(MEClary.Clary.cfy_port_status ==  "komplett") 
			{
				//Vergleichen und einfügen
			}
			else
			{   }
		} 
		
		public void rennen()
		{  
			 
			  
			 Console.WriteLine ( "-------- NOC Portal Clary Daten Import Modul wurde gestartet.---------");
			 /* neue gruppen Nummer generieren aus unix zeitstempel */
			 proto_gruppe = "" + datum.unix();
			 /* Protokoll erstellen */
			 protokoll.erstellen( proto_woher , proto_gruppe , "CFY MYSQL Datenimport Modul wurde gestartet." , proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
 			 while(status)
			 {
				  mainClaryDatenImport();
			 }
			 /* Protokoll erstellen */
			 protokoll.erstellen( proto_woher , proto_gruppe , "CFY MYSQL Datenimport Modul wurde beendet." ,proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
		}
		
	  public void anhalten()
    { /* Thread wird gestoppt wenn dieser Funktion aufegrufen wird */
    	
       status = false;
    }
		 
	}
}
