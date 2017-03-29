/*
   *************************************************************************************************************
   /      MEClary - Modul -                                                                                    /
   /                                                                                                           /
   /                                                                                                           /
   /      Cod by Meiko Eichler                                                                                 /
   /      Copyright by Meiko Eichler                                                                           /
   /      Handy: 0163 7378481                                                                                  /
   /      Email: Meiko@Somba.de                                                                                /
   /                                                                                                           /
   /      Datei erstellt am 01.03.2017                                                                         /
   /                                                                                                           /
   /      Ordner: /klassen/                                                                                    /
   /      Datei Name: meclary.cs                                                                               /
   /                                                                                                           /
   /      Beschreibung: Hier werden die gesammelten Daten vom Portlistner in ein List Array gelegt.            /
   /                    und bereitgestellt zum weiterverarbeiten.                                              /
   /                    Die Klasse leifert ein status wo sich das List gerade befindet.                         /
   /                    - "Clary" Klasse verarbeiet Datenimport vom Listener und stellt in einem List Daten    /
   /                      Bereit zum weiterverarbeiten                                                         /
   /                                                                                                           /
   *************************************************************************************************************  
*/


using System;
using System.Net;
using System.Collections.Generic;
using MEQuery;

namespace MEClary
{
    
    public class Clary
    {
    	 /* Protokolle */
    	 private  Protokoll protokoll = new Protokoll();
    	 private Debuger debuger = new Debuger();
	  	
	  	 
	  	 /* Datensatz von CFY Rohdaten */
   	   public static List<Clary_List> cfy_rohdaten;
   	   
   	   /* Status Variable wo sich der TCPListener gerade befindet */
   	   /* empfange  = "Daten werden gerade von TCP Listener in List gefühlt Bitte Warten */
   	   /* komplett  = "Daten wurden kommtlet geladen und sind zum weiter verarbeiten Bereit */
   	   /* leer      = "So wird es geboren und erhält den status nach einer bearbeitung */
   	   /* mysql     = "Daten werden gerade an mysql weitergeleitet */
   	   public static string cfy_port_status = "leer";
   	   public static string cfy_port_gruppe = "cfy"; /* Gruppen Zuweisung */
	  	    
       public class Clary_List
       {
    	   public string signalquelle_id;
    	   public string cluster_id;
    	   public string type;
    	   public string kategorie;
    	   public int    region;
    	   public string plz;
    	   public string ort;  
    	   public string strasse;
    	   public string hnr;
    	   public string gestatt_name;
    	   public string gestatt_vertragsn;
    	   public int    objekt_id;
    	   public int    soll_we;
    	   public int    subscriber;
    	   public int    subscriber_int;
    	   public int    subscriber_tel;
    	   
    	   public string status;
    	   public string cmts;
    	   public string gps_langengrad;
    	   public string gps_breitengrad;
    	   public string signallieferant;
    	   
    	   public Clary_List() {}
    	   
    	   public Clary_List(string signalquelle_id , string cluster_id , string type , string kategorie , int region , string plz ,
    	                     string ort          , string strasse , string hnr  , string gestatt_name  , string gestatt_vertragsn ,
    	                     int    objekt_id    , int    soll_we , int    subscriber, int    subscriber_int , int    subscriber_tel ,
    	                     string status       , string cmts    , string gps_langengrad  , string gps_breitengrad ,  string signallieferant )
    	   {

    	   	   this.signalquelle_id = signalquelle_id;
    	   	   this.cluster_id      = cluster_id;
    	   	   this.type         = type;
    	   	   this.kategorie    = kategorie;
    	   	   this.region       = region;
    	   	   this.plz          = plz;
    	   	   this.ort          = ort;  
    	   	   this.strasse      = strasse;
    	   	   this.hnr          = hnr;
    	   	   this.gestatt_name = gestatt_name;
    	   	   this.gestatt_vertragsn = gestatt_vertragsn;
    	   	   this.objekt_id        = objekt_id;
    	   	   this.soll_we          = soll_we;
    	   	   this.subscriber       = subscriber;
    	   	   this.subscriber_int   = subscriber_int;
    	   	   this.subscriber_tel   = subscriber_tel;
    	   	   this.status           = status;
    	   	   this.cmts             = cmts;
    	   	   this.gps_langengrad   = gps_langengrad;
    	   	   this.gps_breitengrad  = gps_breitengrad;
    	   	   this.signallieferant  = signallieferant; 	
    	   }
    	
       }
    
       
   	   
       public void rohdaten(string daten)
       {
       	   /* Beispiel:
       	   
       	       string clary_ausgabe = "";
       	       foreach (Clary.Clary_List cla in Clary.cfy_rohdaten)
               {
                   clary_ausgabe += " Ort: " + cla.ort+ " Gestattungsgeber: " + cla.gestatt_name + "\n";
                   Console.WriteLine("\n ->" + clary_ausgabe);
               }
           
           */
    	      Text text = new Text();
    	      
        	  /*  Daten Verarbeiten welche vom Portlistener gekommen sind  - Start - */
       	    string[] rohdaten_zeilen = text.split( "|tr|" , daten ); /*  Zeilen ermitteln */
       	    protokoll.erstellen( debuger.block() , Clary.cfy_port_gruppe , "Es wird begonnen die List zu befühlen mit "+ rohdaten_zeilen.Length +" Einträgen. Status steht weiter auf -> empfange <- " , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
       	    foreach(string  rohdatenZ in rohdaten_zeilen)
            {  /* in dieser schleife werden die CFY Rohdaten übernohmen und in List übernohmen */
              
              try
              { 
                string[] rohdaten = text.split( "|td|" , rohdatenZ );
                /*  0 = signalquelle 
                   1 = cluster
                   2 = typ
                   3 = kategorie
                   4 = region
                   5 = plz
                   6 = ort
                   7 = strasse
                   8 = hnr
                   9 = gestatt_name
                   10 = gestatt_vertragsn
                   11 = objekt_id
                   12 = soll_we
                   13 = subscriber
                   14 = subscriber_int
                   15 = subscriber_tel
                   16 = status
                   17 = cmts
                   18 = gps_langengrad
                   19 = gps_breitengrad
                   20 = signallieferant  
                   
                 LIST =  (string signalquelle_id , string cluster_id , string type , string kategorie , int region , string plz ,
    	                     string ort          , string strasse , string hnr  , string gestatt_name  , string gestatt_vertragsn ,
    	                     int    objekt_id    , int    soll_we , int    subscriber, int    subscriber_int , int    subscriber_tel ,
    	                     string status       , string cmts    , string gps_langengrad  , string gps_breitengrad ,  string signallieferant )   */
    	                     
    	                     
                Clary.cfy_rohdaten.Add( new Clary_List( rohdaten[0]  , rohdaten[1] , rohdaten[2] , rohdaten[3] , Convert.ToInt32( rohdaten[4] ) , rohdaten[5] , rohdaten[6] , rohdaten[7] ,  rohdaten[8]  ,  rohdaten[9]  ,
       	                                               rohdaten[10] , Convert.ToInt32( rohdaten[11] ), Convert.ToInt32( rohdaten[12] ) , Convert.ToInt32( rohdaten[13] ) ,  Convert.ToInt32( rohdaten[14] ) , Convert.ToInt32( rohdaten[15] ) ,
    	                                                 rohdaten[16] , rohdaten[17] , rohdaten[18] ,  rohdaten[19] ,  rohdaten[20] ) );
       	     
       	      }
   	    	    catch (Exception e)
              {
                       protokoll.erstellen( debuger.block() ,  Clary.cfy_port_gruppe , "Exception wurde gewurfen. Fehler: " + e.Message , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  );  /* Protokoll erstellen */
              }
                     
            }
            
       	    protokoll.erstellen( debuger.block() , Clary.cfy_port_gruppe , "List wurde erfolgreich erstellt und Status wird auf -> mysql <- gesetzt." , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
       	   
       	    Clary.cfy_port_status = "mysql";  /* Daten wurden Komplett Übernohmen Status setezen und zur weiterverarbeitung Frei geben */ 
       } 
        
    }
}