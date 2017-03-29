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
    private string proto_gruppe;
	  
	  

		/* Die Hauptfunktion / Main  vom backend NOC Portal */
		public void mainClaryDatenImport()
		{

			/* hier erst reingehen wenn Status komplett oder empfange geliefert wird von Klasse */
			if(Clary.cfy_port_status == "mysql")//MEClary.Clary.cfy_port_status ==  "empfange") 
			{ 
				try
				{
				   string clary_ausgabe = "";
       	   foreach (Clary.Clary_List cla in Clary.cfy_rohdaten)
           {
                   clary_ausgabe += " Ort: " + cla.ort+ " Gestattungsgeber: " + cla.gestatt_name + "\n";
                   Console.WriteLine("\n ->" + clary_ausgabe);
           }
				
				}
				catch(NullReferenceException e)
				{
					  protokoll.erstellen( debuger.block() , proto_gruppe , "Fehler bei der DatenSchleife. Info: "  + e.Message , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
				}
				
				Clary.cfy_port_status = "leer";
        Console.WriteLine ( "-------- Erledigt CFY Daten wurden in DB Geschrieben ---- ");
				 
				//MEQuery.Protokoll protokolll = new MEQuery.Protokoll ();

				 protokoll.erstellen( debuger.block() , proto_gruppe , "Neue daten sind angekommen vom Listener Status: " + MEClary.Clary.cfy_port_status , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
				

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
			     protokoll.erstellen( debuger.block() , proto_gruppe , "Daten wurden erfolgreich in MYSQL integriert ( CFY) warte auf neue Daten." , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
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
			 protokoll.erstellen( debuger.block() , proto_gruppe , "CFY MYSQL Datenimport Modul wurde gestartet." , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
 			 while(status)
			 {
				  mainClaryDatenImport();
			 }
			 /* Protokoll erstellen */
			 protokoll.erstellen( debuger.block() , proto_gruppe , "CFY MYSQL Datenimport Modul wurde beendet." ,debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
		}
		
	  public void anhalten()
    { /* Thread wird gestoppt wenn dieser Funktion aufegrufen wird */
    	
       status = false;
    }
		 
	}
}
