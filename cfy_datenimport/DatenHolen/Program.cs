﻿using System;
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
	  

		/* Die Hauptfunktion / Main  vom backend NOC Portal */
		public void mainClaryDatenImport()
		{

			/* hier erst reingehen wenn Status komplett oder empfange geliefert wird von Klasse */
			if(true )//MEClary.Clary.cfy_port_status ==  "empfange") 
			{   
				//MEQuery.Protokoll protokolll = new MEQuery.Protokoll ();

				   //protokoll.erstellen( proto_woher , proto_gruppe , "Neue daten sind angekommen vom Listener Status: " + MEClary.Clary.cfy_port_status , proto_datei ,"MySQLDatenImport","mainClaryDatenImport()" , false );
				//protokoll.erstellen()

				
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
			 
			
			     /* Status ändern und Listener wieder frei geben das dieser neue Daten empfangen kann */

			     
			     /* Protokoll erstellen */
			     //protokoll.erstellen( proto_woher , proto_gruppe , "Daten wurden erfolgreich in MYSQL integriert ( CFY) warte auf neue Daten." , proto_datei ,"MySQLDatenImport","mainClaryDatenImport()" , false );
			     /* neue gruppen Nummer generieren aus unix zeitstempel */
			     //proto_gruppe = "" + datum.unix();
			}
			else if(MEClary.Clary.cfy_port_status ==  "komplett") 
			{
				//Vergleichen und einfügen
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
