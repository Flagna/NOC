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
    	 private  string  proto_woher  = "Clary_Daten_List_Erstellung";
	  	 private  string  proto_klasse = "Clary";
	  	 
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
       	   
       	       List<Clary.Clary_List> clary_daten = clary.rohdaten();
       	  
       	       string clary_ausgabe = "";
       	       foreach (Clary.Clary_List cla in clary_daten)
               {
                   clary_ausgabe += " Ort: " + cla.ort+ " Gestattungsgeber: " + cla.gestatt_name + "\n";
               }
           
           */
             
            
       	    
       	    string signalquelle_id = string.Empty;
    	      string cluster_id   = string.Empty;
    	      string type      = string.Empty;
    	      string kategorie = string.Empty;
    	      int    region;
    	      string plz      = string.Empty;
    	      string ort      = string.Empty;  
    	      string strasse  = string.Empty;
    	      string hnr      = string.Empty;
    	      string gestatt_name       = string.Empty;
    	      string gestatt_vertragsn  = string.Empty;
    	      int    objekt_id;
    	      int    soll_we;
    	      int    subscriber;
    	      int    subscriber_int;
    	      int    subscriber_tel;
    	      string status          = string.Empty;
    	      string cmts            = string.Empty;
    	      string gps_langengrad  = string.Empty;
    	      string gps_breitengrad = string.Empty;
    	      string signallieferant = string.Empty;
    	          	      
        	  List<Clary_List> clary_daten = new List<Clary_List>();
       	
       	    /*  Daten Verarbeiten welche vom Portlistener gekommen sind  - Start - */
       	    protokoll.erstellen( proto_woher , Clary.cfy_port_gruppe , "Es wird begonnen die List zu befühlen mit den Daten vom Portlistener." , proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
       	    
       	    
       	    Console.WriteLine( "\n Daten Was CFY Klasse zum Befühllen erhalten hat: " + daten + " \n Bitte Taste Drücken. " );  
       	    Console.ReadKey();
       	    
       	    
       	    /*  Daten Verarbeiten welche vom Portlistener gekommen sind  - Ende - */
        	   
       	    
       	    
       	    /* Testdaten  Start */
       	     
       	    signalquelle_id = "58268";  	      cluster_id   = "005865";
    	      type      = "eigen";    	      kategorie = "sonstiges";
    	      region   =  1;     	            plz      = "04435";
    	      ort      = "Schkeuditz";        strasse  = "teststraße";
    	      hnr      = "15";         	      gestatt_name       = "Wogetra";
    	      gestatt_vertragsn  = "jj88899222ll";    objekt_id       = 584848484;
    	      soll_we         = 10;      	      subscriber      = 5;
    	      subscriber_int  = 2;    	        subscriber_tel  = 3;
    	      status          = "online";      cmts            = "l1-he2";
    	      gps_langengrad  = "8458484.965895489"; 	      gps_breitengrad = "1122121.3333333";
    	      signallieferant = "Primacom";
    	      
       	    clary_daten.Add( new Clary_List(signalquelle_id , cluster_id , type , kategorie , region , plz ,  ort , strasse ,  hnr  ,  gestatt_name  ,
       	                                     gestatt_vertragsn , objekt_id , soll_we ,  subscriber,  subscriber_int , subscriber_tel ,
    	                                       status            , cmts      ,  gps_langengrad  ,  gps_breitengrad ,   signallieferant ) );
    	                                       
            signalquelle_id = "23236";  	      cluster_id   = "1125582";
    	      type      = "eigen";    	      kategorie = "kopf";
    	      region   =  6;     	            plz      = "23546";
    	      ort      = "Berlin";            strasse  = "berlinstrase";
    	      hnr      = "55";         	      gestatt_name       = "Berliner";
    	      gestatt_vertragsn  = "KKK9922222";    objekt_id       = 1112255333;
    	      soll_we         = 10;      	      subscriber      = 5;
    	      subscriber_int  = 2;    	        subscriber_tel  = 3;
    	      status          = "online";      cmts            = "be-he6";
    	      gps_langengrad  = "8458484.965895489"; 	      gps_breitengrad = "1122121.3333333";
    	      signallieferant = "Telecolumbus";
    	      
       	    clary_daten.Add( new Clary_List(signalquelle_id , cluster_id , type , kategorie , region , plz ,  ort , strasse ,  hnr  ,  gestatt_name  ,
       	                                     gestatt_vertragsn , objekt_id , soll_we ,  subscriber,  subscriber_int , subscriber_tel ,
    	                                       status            , cmts      ,  gps_langengrad  ,  gps_breitengrad ,   signallieferant ) );
    	                                       
            signalquelle_id = "1122233";  	    cluster_id   = "889977";
    	      type      = "fremd";    	      kategorie = "hallo";
    	      region   =  3;     	            plz      = "47866";
    	      ort      = "Dresden";            strasse  = "dresdenstrase";
    	      hnr      = "77h";         	      gestatt_name       = "Gestt Dresden GmbH";
    	      gestatt_vertragsn  = "DE778skk";    objekt_id       = 556565656;
    	      soll_we         = 15;      	      subscriber      = 15;
    	      subscriber_int  = 12;    	        subscriber_tel  = 13;
    	      status          = "online";      cmts            = "de-he2";
    	      gps_langengrad  = "45484884.55555"; 	      gps_breitengrad = "148544845.5985959";
    	      signallieferant = "Telekom";
    	      
       	    clary_daten.Add( new Clary_List(signalquelle_id , cluster_id , type , kategorie , region , plz ,  ort , strasse ,  hnr  ,  gestatt_name  ,
       	                                     gestatt_vertragsn , objekt_id , soll_we ,  subscriber,  subscriber_int , subscriber_tel ,
    	                                       status            , cmts      ,  gps_langengrad  ,  gps_breitengrad ,   signallieferant ) );
       	    
       	    /* Testdaten  Ende */
       	    
       	    
       	    
       	    cfy_rohdaten = clary_daten; /* Daten in Klassen List laden und zum verarbeiten bereitstellen */
       	    protokoll.erstellen( proto_woher , Clary.cfy_port_gruppe , "List wurde erfolgreich erstellt mit daten und Status wurde auf Komplett gesetzt." , proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
       	    Clary.cfy_port_status = "mysql";  /* Clray List Status auf Komplett setezen und zur weiterverarbeitung Frei geben */ 
       } 
        
    }
}