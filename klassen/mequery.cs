/*
   *************************************************************************************************************
   /      MEQuery - Modul -                                                                                    /
   /                                                                                                           /
   /                                                                                                           /
   /      Cod by Meiko Eichler                                                                                 /
   /      Copyright by Meiko Eichler                                                                           /
   /                                                                                                           /
   /      Datei erstellt am 01.03.2017                                                                         /
   /                                                                                                           /
   /      Ordner: /klassen/                                                                                    /
   /      Datei Name: mequery.cs                                                                               /
   /                                                                                                           /
   /      Beschreibung: Dieses modul ist eine Sammlung für mehrer Klassen                                      /
   /                    - "Benutzer" Klasse liefert Benutzer und Password                                      /
   /                    - "MYSQL" Klasse diese liefert den zugang zur localen mysql Datenbank                  /
   /                    - "EventObjekt" Klasse reagiert auf Tastatur eingaben ( eventhändler )                 /
   /                    - "Protokoll"  Klasse protokolliert den Programverlauf und legt statuse in List        /
   /                    - "Text" Klasse filtern von steuerzeichen, vergleichen usw.                            /
   /                    - "Datum" Klasse liefert Datum,Unixzeitstempel usw.                                    /
   /                    - "BeepSong" Klasse liefert Lieder für den Speaker ;-)                                 /
   /                    - "PortZuweisung" Klasse  sucht alle Netzwerkkomponenetn zb IP Adresse, Status,        /
   /                       Lebenszeit, Welche art usw und Speichert diese in ein List                          /
   /                    - "AsciiPic" Klasse hier sind Bilder in Ascii Format hinterlegt                        /
   /                    - "ConsolenAnimation" Klasse zb Ladestatus ausgabe auf einer Zeile usw.                /
   /                                                                                                           /
   *************************************************************************************************************  
*/


using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.IO;



namespace MEQuery
{   
	 
	 public class Einstellung
   {   
   	   public static int cfy_rohdaten_port;
   	   public static int http_port;
   	   public static int https_port;
   	   public static int admin_port;
   	   public static string ip_adresse;
   	   public static bool sound;
   	   
   	   private string  proto_woher  = "Server_Einstellung";
	  	 private string  proto_datei  = "/klassen/mequery.cs";
	  	 private string  proto_klasse = "Einstellung";
	  	 private string  proto_gruppe = Environment.MachineName;
	  	 Protokoll protokoll = new Protokoll();
	  	 
   	   
   	   public bool laden( )
   	   {
   	   	 Text  text   = new Text();
   	   	 Datum datum  = new Datum();
   	   	 bool  status = false;
   	   	 int   er     = 0;
   	   	 
   	   	 try
   	   	 {    
   	   	 	 protokoll.erstellen( proto_woher , proto_gruppe , "config.noc Datei Laden.", proto_datei ,proto_klasse,"laden()" , false);
   	   	   if( File.Exists(@"config_server.noc")  == false )
           {  /* Datei war noch nicht Vorhanden HTML Kopf Schreiben un als erstes anhängen */ 
              protokoll.erstellen( proto_woher , proto_gruppe , "config_server.noc Datei war nicht vorhanden erstelle Default config." , proto_datei ,proto_klasse,"laden()" , false);
              
              string[] inhalt_datei  = new string[28];
              inhalt_datei[0]  = "#";
              inhalt_datei[1]  = "#   ************************************************************************************************************* ";
              inhalt_datei[2]  = "#   /      NOC Portal Server Einstellungsdatei                                                                  / ";
              inhalt_datei[3]  = "#   /                                                                                                           / ";
              inhalt_datei[4]  = "#   /                                                                                                           / ";
              inhalt_datei[5]  = "#   /      Cod by Meiko Eichler                                                                                 / ";
              inhalt_datei[6]  = "#   /      Copyright by Meiko Eichler                                                                           / ";
              inhalt_datei[7]  = "#   /                                                                                                           / ";
              inhalt_datei[8]  = "#   /      Datei erstellt am 14.03.2017                                                                         / ";
              inhalt_datei[9]  = "#   /      Generiert am " + datum.datum_zeit() + "                                                              / ";
              inhalt_datei[10] = "#   /                                                                                                           / ";
              inhalt_datei[11] = "#   /      Datei Name: config_server.noc                                                                        / ";
              inhalt_datei[12] = "#   /                                                                                                           / ";
              inhalt_datei[13] = "#   /      Werte immer in  \" \" schreiben!                                                                     / ";
              inhalt_datei[14] = "#   /      Alle 4 Felder müssen angegeben werden sonst bricht Programm ab!                                      / ";
              inhalt_datei[15] = "#   /      als Wert kann immer \"default\"  angegeben werden Es werden dann systemeinstellungen genommen vom Pr./ ";
              inhalt_datei[16] = "#   /      bei sound gibt es zwei werte aus und an  ( standart bei default )                                    / ";
              inhalt_datei[17] = "#   /                                                                                                           / ";
              inhalt_datei[18] = "#   ************************************************************************************************************* ";
              inhalt_datei[19] = "";
              inhalt_datei[20] = "";
              inhalt_datei[21] = "ip_adresse          =  \"default\" ";
              inhalt_datei[22] = "cfy_rohdaten_port   =  \"default\" ";
              inhalt_datei[23] = "http_port           =  \"default\" ";
              inhalt_datei[24] = "https_port          =  \"default\" ";
              inhalt_datei[25] = "admin_port          =  \"default\" ";
              inhalt_datei[26] = "sound               =  \"default\" ";
              inhalt_datei[27] = "";        
              /* Config Datei in Datei Schreiben */
              File.AppendAllLines( @"config_server.noc" , inhalt_datei );
   	   	   }
   	   	   else {}
   	   	   
   	   	   string[] inhalt = System.IO.File.ReadAllLines(@"config_server.noc");
   	   	   foreach (string daten in inhalt)
   	   	   {
   	   	   	   string[] auswertung = text.split( "=" , daten );
   	   	   	  
   	   	   	   if( text.trim(auswertung[0]) == "ip_adresse")
   	   	   	   {
   	   	   	   	  string puffer = text.trim(auswertung[1]);
   	   	   	   	  if(text.trim(puffer , "dophoch" ) == "default") /* Default Einstellung nehmen */
   	   	   	   	    Einstellung.ip_adresse =  "alle";
   	   	   	   	  else
   	   	   	        Einstellung.ip_adresse =  text.trim(puffer , "dophoch" );
   	   	   	      er++;
   	   	   	      protokoll.erstellen( proto_woher , proto_gruppe , "IP Adresse in Datei gefunden: " + Einstellung.ip_adresse , proto_datei ,proto_klasse,"laden()" , false);
   	   	   	   }
   	   	   	   else if( text.trim(auswertung[0]) == "cfy_rohdaten_port")
   	   	   	   {
   	   	   	   	  string puffer = text.trim(auswertung[1]);
   	   	   	   	  if(text.trim(puffer , "dophoch" )  == "default" ) /* Default Einstellung nehmen */
   	   	   	   	    Einstellung.cfy_rohdaten_port     =  4411;
   	   	   	   	  else
   	   	   	        Einstellung.cfy_rohdaten_port     =  Convert.ToInt32( text.trim(puffer , "dophoch" ) );
   	   	   	      er++;
   	   	   	      protokoll.erstellen( proto_woher , proto_gruppe , "CFY Rohdaten Port wurde in Datei gefunden: " + Einstellung.cfy_rohdaten_port , proto_datei ,proto_klasse,"laden()" , false);
   	   	   	   }
   	   	   	   else if( text.trim(auswertung[0]) == "http_port")
   	   	   	   {
   	   	   	   	  string puffer = text.trim(auswertung[1]);
   	   	   	      if(text.trim(puffer , "dophoch" ) == "default" ) /* Default Einstellung nehmen */
   	   	   	        Einstellung.http_port     =  88;
   	   	   	      else
   	   	   	        Einstellung.http_port     =  Convert.ToInt32( text.trim(puffer , "dophoch" ) );
   	   	   	      er++;
   	   	   	      protokoll.erstellen( proto_woher , proto_gruppe , "http Port wurde gefunden: " + Einstellung.http_port , proto_datei ,proto_klasse,"laden()" , false);
   	   	   	   }
   	   	   	   else if( text.trim(auswertung[0]) == "https_port")
   	   	   	   {
   	   	   	   	  string puffer = text.trim(auswertung[1]);
   	   	   	      if(text.trim(puffer , "dophoch" ) == "default" ) /* Default Einstellung nehmen */
   	   	   	        Einstellung.https_port     =  88779;
   	   	   	      else
   	   	   	        Einstellung.https_port     =  Convert.ToInt32( text.trim(puffer , "dophoch" ) );
   	   	   	      er++;
   	   	   	      protokoll.erstellen( proto_woher , proto_gruppe , "https Port wurde gefunden: " + Einstellung.https_port , proto_datei ,proto_klasse,"laden()" , false);
   	   	   	   }
   	   	   	   else if( text.trim(auswertung[0]) == "admin_port")
   	   	   	   {
   	   	   	   	  string puffer = text.trim(auswertung[1]);
   	   	   	      if(text.trim(puffer , "dophoch" ) == "default" ) /* Default Einstellung nehmen */
   	   	   	        Einstellung.admin_port     =  771482;
   	   	   	      else
   	   	   	        Einstellung.admin_port     =  Convert.ToInt32( text.trim(puffer , "dophoch" ) );
   	   	   	      er++;
   	   	   	      protokoll.erstellen( proto_woher , proto_gruppe , "Admin Port wurde gefunden: " + Einstellung.admin_port , proto_datei ,proto_klasse,"laden()" , false);
   	   	   	   }
   	   	   	   else if( text.trim(auswertung[0]) == "sound") 
   	   	   	   {
   	   	   	   	  string puffer = text.trim(auswertung[1]);
   	   	   	   	  string pr_ausgabe =  "an";
   	   	   	   	  if( text.trim(puffer , "dophoch" ) == "default" )  /* Default Einstellung nehmen */
   	   	   	   	    Einstellung.sound = true;
   	   	   	   	  else
   	   	   	   	  {
   	   	   	   	  	pr_ausgabe = text.trim(puffer , "dophoch" );
   	   	   	        if(text.trim(puffer , "dophoch" ) == "aus" ) Einstellung.sound = false;  else  Einstellung.sound = true; 
   	   	   	      }
   	   	   	      er++; 
   	   	   	      protokoll.erstellen( proto_woher , proto_gruppe , "Sound Einstellung wurde in Datei gefunden: " + pr_ausgabe , proto_datei ,proto_klasse,"laden()" , false);
   	   	   	   }
   	   	   	   else {}
   	   	   	   
   	   	   }
   	   	   
   	   	   if(er == 6) /* Es müssen alle 6 Parameter gefunden werden in der Ini ansonsten Bricht das System ab */ 
   	   	   {    status = true;  /* einstellungs Daten wurden alle geladen True zurückgeben das system weiter arbeitet */
   	   	   	    protokoll.erstellen( proto_woher , proto_gruppe , "Alle Einstellungen in Config Datei gefunden." , proto_datei ,proto_klasse,"laden()" , false); 
   	   	   }
   	   	   else
   	   	    protokoll.erstellen( proto_woher , proto_gruppe , "Config Datei wahr Fehlerhaft es wurden nur " + er + " Einstellungen gefunden. 6 Stück sind aber minimum." , proto_datei ,proto_klasse,"laden()" , true); 
   	   	   
   	   	 }
   	   	 catch (SocketException e)
         {
               string fehlermeldung = String.Format("SocketException: ", e.Message);
               protokoll.erstellen( proto_woher , proto_gruppe , "SocketException wurde gewurfen Verbindung wurde getrennt. Fehler: " + fehlermeldung , proto_datei ,proto_klasse,"laden()" , true);
         }  
   	   	 
   	   	 return status;
   	   }
   
   }
	 
	 public class Benutzer
	 {
	 	     private string  proto_woher  = "NOC_Benutzer_Verwaltung";
	   	   private string  proto_datei  = "/klassen/mequery.cs";
	  	   private string  proto_klasse = "Benutzer";
	  	   private string  proto_gruppe = "benutzer";
	  	   Protokoll protokoll = new Protokoll();
	 	     
	 	     private bool prufung(string p_name ,string p_passw = null ,string was = null )
	 	     {
	 	     	   bool aussage = false;
	 	     	        if(p_name == "meiko"   && p_passw == "dell" || 
	 	     	           p_name == "meiko"   && was     == "name" ) aussage = true;
	 	     	   else if(p_name == "martin"  && p_passw == "dell" || 
	 	     	           p_name == "martin"  && was     == "name" ) aussage = true;
	 	     	   else if(p_name == "cfy"     && p_passw == "cfy#77&17" || 
	 	     	           p_name == "cfy"     && was     == "name" ) aussage = true;
	 	     	   else { }
	 	     	   
	 	     	   return aussage;
	 	     }
	 	     
	 	     public bool liste(string p_name ,string p_passw = null ,string was = null )
	 	     {  
	 	     	  protokoll.erstellen( proto_woher , proto_gruppe , "Benutzer Zugangs-Daten werden abgefragt." , proto_datei ,proto_klasse,"liste(string p_name ,string p_passw = null ,string was = null )" , false );  /* Protokoll Schreibe */
	 	     	  return prufung(p_name ,p_passw, was);
	 	     }
	 	     
	 	
	 }
	 
	 public class MYSQL
	 {     
	 	     private string  proto_woher  = "NOC_Mysql_Zugang";
	   	   private string  proto_datei  = "/klassen/mequery.cs";
	  	   private string  proto_klasse = "MYSQL";
	  	   private string  proto_gruppe = "mysqlZugang";
	  	   Protokoll protokoll = new Protokoll();
	 	   
	 	     private  string benutzer_in()
	 	     {   return "root";   }
	 	     
	 	     private  string password_in()
	 	     {   return "pass";   }
	 	     
	 	     private  string ipadresse_in()
	 	     {   return "127.0.0.1";   }
	 	     
	 	     
	 	     public  string ben()
	 	     {  protokoll.erstellen( proto_woher , proto_gruppe , "Benutzer für Mysql wurde abgefragt." , proto_datei ,proto_klasse,"ben()" , false );  /* Protokoll Schreibe */  
	 	     	  return benutzer_in();   }
	 	     
	 	     public  string pass()
	 	     {   protokoll.erstellen( proto_woher , proto_gruppe , "Password für Mysql wurde abgefragt." , proto_datei ,proto_klasse,"pass()" , false );  /* Protokoll Schreibe */  
	 	     	   return password_in();   }
	 	     
	 	     public  string ip()
	 	     {   protokoll.erstellen( proto_woher , proto_gruppe , "IP Adresse für Mysql wurde abgefragt." , proto_datei ,proto_klasse,"ip()" , false );  /* Protokoll Schreibe */  
	 	     	   return ipadresse_in();   }
	 }
	 
   public class EventObjekt
   {
   	    
         public bool tastatur(ConsoleKeyInfo taste,string was )
	  	   {  /*  Eventhändler */
	  	  	
	  	  	 bool ruckgabe = false;
	  	  	 if(was == "altgr+s" && ( taste.Modifiers & ConsoleModifiers.Control) != 0  && taste.Key == ConsoleKey.S) ruckgabe = true; else {}
	  	  	 if(was == "altgr+b" && ( taste.Modifiers & ConsoleModifiers.Control) != 0  && taste.Key == ConsoleKey.B) ruckgabe = true; else {}
	  	  	 if(was == "altgr+1" && ( taste.Modifiers & ConsoleModifiers.Control) != 0  && taste.Key == ConsoleKey.D1) ruckgabe = true; else {}
	  	  	 if(was == "altgr+2" && ( taste.Modifiers & ConsoleModifiers.Control) != 0  && taste.Key == ConsoleKey.D2) ruckgabe = true; else {}
	  	  	 if(was == "altgr+3" && ( taste.Modifiers & ConsoleModifiers.Control) != 0  && taste.Key == ConsoleKey.D3) ruckgabe = true; else {}
	  	  	 if(was == "altgr+4" && ( taste.Modifiers & ConsoleModifiers.Control) != 0  && taste.Key == ConsoleKey.D4) ruckgabe = true; else {}
	  	  	 if(was == "altgr+5" && ( taste.Modifiers & ConsoleModifiers.Control) != 0  && taste.Key == ConsoleKey.D5) ruckgabe = true; else {}
	  	  	 if(was == "altgr+c" && ( taste.Modifiers & ConsoleModifiers.Control) != 0  && taste.Key == ConsoleKey.C) ruckgabe = true; else {}
	  	  	
	  	  	 return ruckgabe;
	  	   }
   }
   
   
   public class Protokoll
   {
   	    public static string alt_datum;
   	    public static int durchlauf;
   	    private string  proto_woher  = "NOC_Protokoll";
	   	  private string  proto_datei  = "/klassen/mequery.cs";
	      private string  proto_klasse = "Protokoll";
	  	  private string  proto_gruppe = "Protokoll";
	  	  
   	    public void rennen()
   	    {
   	    	    while(true)
   	    	    {
   	    	    	  try
   	    	    	  {
   	    	    	  	
   	    	    	     if(liste.Count > 0 && liste[0].woher != "")
   	    	    	     {
   	    	    	   	    speichern(liste[0].woher,liste[0].gruppe ,liste[0].inhalt,liste[0].datei,liste[0].klasse,liste[0].funktion,liste[0].fehler );
                        liste.RemoveAt(0);
   	    	    	     }
   	    	    	     else { }
   	    	    	  }
   	    	    	  catch (SocketException e)
                  {
                        string fehlermeldung = String.Format("SocketException: {0}", e.Message);
                        erstellen( proto_woher , proto_gruppe , "SocketException wurde gewurfen. Fehler: " + fehlermeldung , proto_datei ,proto_klasse,"rennen()" , true );
                  }
   	    	    	
   	    	    }
   	    	     
   	    }
   	    
   	    public class Protokoll_List
   	    {
   	    	   public string  woher;
   	    	   public string  inhalt;
   	    	   public string  datei;
   	    	   public string  klasse;
   	    	   public string  funktion;
   	    	   public bool    fehler;
   	    	   public int     unixzeit;
   	    	   public string  gruppe;
   	    	   
   	    	   public Protokoll_List( ) { }   
   	    	   
   	    	   public Protokoll_List(string woher,string gruppe ,string inhalt,string datei,string klasse,string  funktion,bool fehler,int unixzeit ) 
   	    	   {
   	    	   	  this.woher  = woher;
   	    	   	  this.gruppe = gruppe;
   	    	   	  this.inhalt = inhalt;
   	    	   	  this.datei  = datei;
   	    	   	  this.klasse = klasse;
   	    	   	  this.funktion = funktion;
   	    	   	  this.fehler   = fehler;
   	    	   	  this.unixzeit = unixzeit;
   	    	   	  
   	    	   }
   	    }
   	    
   	    /* Protokoll Liste erstellen */
   	    public static List<Protokoll_List> liste = new List<Protokoll_List>();
   	     
   	    public void erstellen(string woher,string gruppe,string inhalt,string datei,string klasse ,string  funktion ,bool fehler )
   	    {   
   	    	 try
   	    	 { 
   	    	   Datum datum = new Datum();
   	     	   liste.Add(new Protokoll_List( woher , gruppe , inhalt ,datei ,klasse ,funktion ,fehler , datum.unix() ) );
   	     	 }
   	     	 catch{}
   	    }
   	    
   	    
   	    private void speichern(string woher,string gruppe ,string inhalt,string dateiname,string klasse,string  funktion,bool fehler )
   	    {   /* Diese funktion speichert die gesamelten Daten in eine Datei auf dem system wo es gerade läuft beim beenden */

               try
               {   Datum datum = new Datum();
                   Datei datei = new Datei();
                  
               	   int    unixzeit     = datum.unix();   
               	   string td_style     = "<td style=\"background:#8fbc8f;color:#000000;font-weight:bold;text-align:left;font-size:1.0em;height:30px;\" >";
               	   string td_intern    = "<td style=\"color:#000000;text-align:left;font-size:1.0em;height:30px;\" >";
                   string td_intern_nw = "<td style=\"color:#000000;text-align:left;font-size:1.0em;height:30px;\" nowrap >";
                   string spaltenName  = "<tr>" + td_style + "Datum</td>" + td_style + "Uhrzeit</td>" + td_style + "Fehler</td>" + td_style + "Woher</td>"+ td_style + "Gruppe</td>" + td_style + "Inhalt</td>" + td_style + "Datei</td>" + td_style + "Klasse</td>" + td_style + "Funktion</td></tr>";
               	   
               	  /* HTML Protokoll in Datei Speichern */
               	   if( File.Exists(@"noc_protokoll_server.html")  == false )
               	   {    /* Datei war noch nicht Vorhanden HTML Kopf Schreiben und Datei erstellen */ 
                         
                         Protokoll.alt_datum = datum.unixDatum( unixzeit , "datum"); /* Datum Neu setzen */
                         string[] kopf_inhalt  = new string[52]; 
               	   	     kopf_inhalt[0] = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> ";
                         kopf_inhalt[1] = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\"> ";
                         kopf_inhalt[2] = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\"              \"http://www.w3.org/TR/html4/strict.dtd\"> ";
                         kopf_inhalt[3] = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Frameset//EN\"     \"http://www.w3.org/TR/html4/frameset.dtd\"> ";
                         kopf_inhalt[4] = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\"       \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\"> ";
                         kopf_inhalt[5] = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> ";
                         kopf_inhalt[6] = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Frameset//EN\"     \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd\"> ";
                         kopf_inhalt[7] = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\"              \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\"> ";
                         kopf_inhalt[8] = "<html>";
                         kopf_inhalt[9] = "<head>";
	                       kopf_inhalt[10] = "<meta charset=\"UTF-8\">";
                         kopf_inhalt[11] = "<meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\"> ";
                         kopf_inhalt[12] = "<meta http-equiv=\"refresh\" content=\"5\">";
                         kopf_inhalt[13] = "<title>NOC Portal - Server Protokoll Datei erstellt am "+ datum.datum_zeit() + "</title>";
                         kopf_inhalt[14] = "<style type=\"text/css\"> ";
                         kopf_inhalt[15] = " /* ";
                         kopf_inhalt[16] = "   *************************************************************************************************************  ";
                         kopf_inhalt[17] = "   /      NOC Portal Server - Protokoll Datei                                                                  /  ";
                         kopf_inhalt[18] = "   /                                                                                                           /  ";
                         kopf_inhalt[19] = "   /                                                                                                           /  ";
                         kopf_inhalt[20] = "   /      Cod by Meiko Eichler                                                                                 /  ";
                         kopf_inhalt[21] = "   /      Copyright by Meiko Eichler                                                                           /  ";
                         kopf_inhalt[22] = "   /                                                                                                           /  ";
                         kopf_inhalt[23] = "   /      Datei erstellt am 14.03.2017                                                                         /  ";
                         kopf_inhalt[24] = "   /      Generiert am " + datum.datum_zeit() + "                                                              /  ";
                         kopf_inhalt[25] = "   /                                                                                                           /  ";
                         kopf_inhalt[26] = "   /      Datei Name: noc_protokoll_server.html                                                                /  ";
                         kopf_inhalt[27] = "   /                                                                                                           /  ";
                         kopf_inhalt[28] = "   /      Protokolle für die Laufzeitumgebung                                                                  /  ";
                         kopf_inhalt[29] = "   /                                                                                                           /  ";
                         kopf_inhalt[30] = "   *************************************************************************************************************  ";
                         kopf_inhalt[31] = " */ ";
                         kopf_inhalt[32] = "@charset \"UTF-8\"; ";
                         kopf_inhalt[33] = "html{ height:100%; width:100%; }    ";
                         kopf_inhalt[34] = "</style> ";
                         kopf_inhalt[35] = "<script type=\"text/javascript\"> ";
                         kopf_inhalt[36] = "function Seitenende() { ";
                         kopf_inhalt[37] = " /* document.getElementById('endeDatei').scrollIntoView(true); */ ";
                         kopf_inhalt[38] = "} ";
                         kopf_inhalt[39] = "window.onload=function(){ ";
                         kopf_inhalt[40] = "Seitenende(); ";
                         kopf_inhalt[41] = "} ";
                         kopf_inhalt[42] = "</script>";
                         kopf_inhalt[43] = "</head> ";
                         kopf_inhalt[44] = "<body bgcolor=\"#707a7d\"> ";
                         kopf_inhalt[45] = "<br /><center><table style=\"width:98%\"  border=\"3\" cellpadding=\"0\" cellspacing=\"0\"  bordercolorlight=\"#8C8E8C\" bordercolordark=\"#000000\">";
                         kopf_inhalt[46] = "<tr><th height=\"25\" style=\"color:#FFFFFF;background-color:#bd0e39;font-size:1.2em;text-align:left;height:35px;\" colspan=\"9\">NOC Portal - Server Protokoll vom "+ datum.datum_zeit() + "</td></tr>";
                         kopf_inhalt[47] = spaltenName;
                         kopf_inhalt[48] = "</table>";
                         kopf_inhalt[49] = "</center><br />";
                         kopf_inhalt[50] = "<div id='endeDatei'>";
                         kopf_inhalt[51] = "</div></body></html>";
                         
                   
                          /* Datei mit Kopf erstellen und Datei erstellen */
                         File.AppendAllLines( @"noc_protokoll_server.html" , kopf_inhalt );
                   }
   	   	           else if(Protokoll.alt_datum ==  "" || Protokoll.alt_datum != datum.unixDatum( unixzeit , "datum") ) 
   	   	           {  /* Server wurde neu gestartet oder ein Neuer Tag hat begonnen  Tabelle Neu erstellen  */
   	   	           	     
   	   	           	     Protokoll.alt_datum = datum.unixDatum( unixzeit , "datum"); /* Datum Neu setzen */
   	   	           	     string[] new_tabelle = new string[5];
   	   	           	     new_tabelle[0] = "<br /><center><table style=\"width:98%\"  border=\"3\" cellpadding=\"0\" cellspacing=\"0\"  bordercolorlight=\"#8C8E8C\" bordercolordark=\"#000000\">";
                         new_tabelle[1] = "<tr><th height=\"25\" style=\"color:#FFFFFF;background-color:#bd0e39;font-size:1.2em;text-align:left;height:35px;\" colspan=\"9\">NOC Portal - Server Protokoll vom "+ datum.datum_zeit() + "</td></tr>";
                         new_tabelle[2] = spaltenName;
                         new_tabelle[3] = "</table>";
                         new_tabelle[4] = "</center><br />";
                         
                         datei.einfugen("<div id='endeDatei'>","davor","unten",@"noc_protokoll_server.html", new_tabelle );
   	   	           }
   	   	          
   	   	           string farbe_ok_fehler = string.Empty; 
   	   	           string fehler_text     = string.Empty;
   	   	           if(fehler == true)
                   {  farbe_ok_fehler = "background-color:#FE2E2E";
                      fehler_text = "Ja";  }
                   else 
                   {  farbe_ok_fehler = "background-color:#efefef";
                      fehler_text = "Nein"; }
                      
                               	   
                   /* Prüfen ob Durchlauf schon 15 Erreicht hat wenn ja ein Array mehr erstellen und Spaltennamen einfügen */
                   int str_lange = 1;
                   if(Protokoll.durchlauf > 15)str_lange = 2; else {}
                   string[] ubergabe = new string[str_lange];
                  
                   ubergabe[0] = "<tr style=\" "+ farbe_ok_fehler + "; \" >" + td_intern + Protokoll.durchlauf + "-" + datum.unixDatum( unixzeit , "datum") + "</td>" + td_intern + datum.unixDatum( unixzeit , "uhrzeit") + "</td>" + td_intern + fehler_text + "</td>" + td_intern_nw + woher + "</td>" + td_intern_nw + gruppe + "</td>" + td_intern + inhalt + "</td>" + td_intern + dateiname + "</td>" + td_intern + klasse + "</td>" + td_intern + funktion + "</td></tr>";
                   if(Protokoll.durchlauf > 15){  ubergabe[1] = spaltenName;  Protokoll.durchlauf = 0; } else {}
                     
                   /* Protokoll in Datei einfügen */
                   datei.einfugen("</table>","davor","unten",@"noc_protokoll_server.html", ubergabe );
   	   	           
   	   	           /* Durchlauf zählen */
   	   	           Protokoll.durchlauf++;
               }
               catch{}
               
   	    }
   	    
   	    
   }
   
   public class Datei
   {  /* Datei Behandlung */
         private Text text = new Text();
          	
   	     public bool einfugen(string anker,string wo,string suche,string pathDatei , string[] zeilenInhalt )
   	     {  /* Array String an Besimmter Stelle danach einfügen in Datei  */
   	     	
   	     	     bool ruckgabe = true;
   	     	     try
   	     	     {
       	         /* Kompletten daten von Datei holen */
   		           string[] dateiInhalt = File.ReadAllLines(pathDatei);
   	   	           
   	             /* Gesamtgröße von neuer Datei ermitteln */
   	   	         int neuDateiZeilen = dateiInhalt.Length + zeilenInhalt.Length;
   	   	           
   	   	         /* neues String Array für Übergabe der Datei erstellen */
   	   	         string[] neueDaten = new string[neuDateiZeilen];
   	   	         int neuZeile = 0;
   	   	         bool gefunden = false; /* Schutz das nur einmal Zeile eingefügt wird */
   	   	        
   	   	         int gesamtAnker = 1; /* Fängt bei eins an zu Zählen da Anker im einmal da sein muss um dort Daten einzuhängen */
   	   	         if(suche == "unten") /* Suche von unten beginnen */
   	   	         {
   	   	         	  /* Anker ermitteln wieviel vorhanden sind wenn Daten beim letzten Anker eingehangen werden sollen */
   	   	         	  gesamtAnker = text.wieOftZeile(dateiInhalt,anker);
                 }
   	   	         else{ }  
   	   	       
   	   	           int ankerZahler = 1; /* Fängt bei eins an zu Zählen da Anker im einmal da sein muss um dort Daten einzuhängen */
   	   	           for(int i=0;i < dateiInhalt.Length;i++)
   	   	           { 
   	   	           	 try
   	   	           	 {
   	   	           	   if(dateiInhalt[i] == anker  && gefunden == false)
   	   	           	   {  /* Anker wurde gefunden ( Neue Daten darunter einhängen ) */
   	   	           	   	  
   	   	           	   	  if(ankerZahler == gesamtAnker) /* richtigen Anker gefunden */
   	   	           	   	  {
   	   	           	   	  	   gefunden = true; /* Als gefunden deklarieren hier muss nicht noch einmal reingegangen werden */
   	   	           	   	       if(wo == "danach")  /* Inhalt wird  nach dem Anker eingefügt  */
   	   	           	   	       {  neueDaten[neuZeile] = dateiInhalt[i]; /* Alte Daten in neuen Datenbestand schreiben */
   	   	           	   	          neuZeile++; }
   	   	           	   	       else {}
   	   	           	   	  
   	   	           	   	       for(int n=0;n<zeilenInhalt.Length;n++) /* Neue Daten in Datenbestand schreiben */
   	   	           	   	       {
   	   	           	   	  	      neueDaten[neuZeile] = zeilenInhalt[n];
   	   	           	   	  	      neuZeile++;
   	   	           	   	       }
   	   	           	   	  
   	   	           	   	       if(wo == "davor") /* Inhalt wird vor dem Anker eingefügt */
   	   	           	   	       {  neueDaten[neuZeile] = dateiInhalt[i]; /* Alte Daten in neuen Datenbestand schreiben */
   	   	           	   	          neuZeile++; }
   	   	           	   	       else {}
   	   	           	   	  }
   	   	           	   	  else
   	   	           	   	  {
   	   	           	   	  	 ankerZahler++; /* Es war nicht der richtige Anker zum nächsten gehen */
   	   	           	   	  	 neueDaten[neuZeile] = dateiInhalt[i]; /* Alte Daten in neuen Datenbestand schreiben */
   	   	           	   	     neuZeile++;
   	   	           	   	  }
   	   	           	   }
   	   	           	   else 
   	   	           	   {
   	   	           	   	   neueDaten[neuZeile] = dateiInhalt[i]; /* Alte Daten in neuen Datenbestand schreiben */
   	   	           	   	   neuZeile++;
   	   	           	   }
   	   	           	 }
   	   	           	 catch{}
   	   	           }
   	   	           /* Datei Komplett Überschreiben */
   	   	           File.WriteAllLines( pathDatei , neueDaten );   
   	   	       }
   	   	       catch
   	   	       {
   	   	       	  ruckgabe = false;
   	   	       	
   	   	       }    
   	   	       
   	   	       return ruckgabe;
   	     }
   	
   }   
   
   /* String KLasse um Inhalt zu bearbeiten */
   public class Text 
   {   
   	   public  int wieOftZeile(string[] daten,string gesuchteZeile)
   	   {
   	   	    int ruckgabe = 0;
   	   	    foreach(string suche in daten)
   	   	    {
   	   	       if(suche == gesuchteZeile)
   	   	        ruckgabe++;
   	   	       else {}
   	   	    }
   	   	    return ruckgabe;
   	   }
   	   
   	   
   	   public string steuerzeichen(string inhalt,string welche)
   	   {
   	   	  if( welche == "n" || welche == "alle" )inhalt = inhalt.Replace("\n", String.Empty); else {} /* Zeilenumbruch entfernen */
   	   	  if( welche == "t" || welche == "alle" )inhalt = inhalt.Replace("\t", String.Empty); else {} /* horizontaler Tabulator */
   	   	  if( welche == "a" || welche == "alle" )inhalt = inhalt.Replace("\a", String.Empty); else {} /* Klingelzeichen */
   	   	  if( welche == "b" || welche == "alle" )inhalt = inhalt.Replace("\b", String.Empty); else {} /* Backspace */
   	   	  if( welche == "r" || welche == "alle" )inhalt = inhalt.Replace("\r", String.Empty); else {} /* Wagenrücklauf */
   	   	  if( welche == "v" || welche == "alle" )inhalt = inhalt.Replace("\v", String.Empty); else {} /* vertikaler Tabulator */
   	   	  if( welche == "f" || welche == "alle" )inhalt = inhalt.Replace("\f", String.Empty); else {} /* Seitenvorschub */
   	   	  
   	   	  return inhalt;
   	   }
   	   
   	   public string trim(string inhalt , string was = null )
   	   { /* schneidet vorne und Hinten alles ab was defieniert wird */    
   	   	     
   	   	     char[]  wasTrim = new char[1];
   	         if(was == "dophoch" )
   	           wasTrim[0] =  '"';
   	         else if(was != null)
   	            wasTrim[0] =  Convert.ToChar(was);
   	         else
   	            wasTrim[0] =  ' ';

             return inhalt.Trim(wasTrim);
       }
       
       public string[] split( string zeichen , string inhalt , string optional = null)
       { /* string zerschneiden */
       	  
       	     char[]  wasSplit = new char[1];
   	         wasSplit[0] =  Convert.ToChar(zeichen);
   	      
   	         StringSplitOptions  option = new StringSplitOptions();
   	         if(optional == "kk")
   	           option = StringSplitOptions.RemoveEmptyEntries;
   	         else
   	           option = StringSplitOptions.None;  /* Standart Zerschneiden an diesem Punkt und ein Array daraus machen */
   	      
             return inhalt.Split( wasSplit , option );
       }
   	   
   	   public string gross(string inhalt)
   	   {   return inhalt.ToUpper();  }
   	   
   	   public string klein(string inhalt)
   	   {   return inhalt.ToLower();  }
   	   
   	   public byte[] byte_stream(string inhalt,string zeichencode = null) 
   	   { /* Funktion wandeln einen String in einem Byte Strom um */  
   	   	
   	   	  byte[] stream;
   	   	  if( zeichencode == "ascii")stream = System.Text.Encoding.ASCII.GetBytes(inhalt);
   	   	  else                   	   stream = System.Text.Encoding.UTF8.GetBytes(inhalt); /* UTF8 Standart */
   	   	  
   	   	  return stream;
   	   }
   	   
   	   public string text_stream(byte[] inhalt, int i , string zeichencode = null) 
   	   { /* Funktion wandelt einen Byte Strom in einem String */
   	   	
   	       string ausgabe;
   	       if( zeichencode == "ascii")ausgabe = System.Text.Encoding.ASCII.GetString( inhalt , 0, i);
   	   	   else                   	  ausgabe = System.Text.Encoding.UTF8.GetString( inhalt , 0, i); /* UTF8 Standart */
   	   	   
          return ausgabe;  	   	
   	   }
   	   
   	    public bool gleich( string inhalt , string  verg_inhalt )
  	  	{
  	  	  	   bool ruckgabe = false;
  	  	  	   
  	  	       if ( inhalt.Equals ( verg_inhalt ) )
  	  	        ruckgabe = true;
  	  	       else {}
 	       
  	  	       return ruckgabe;
  	    }
   	   
   }
  
  
   
   public class Datum
   {
	
	     public string  datum_zeit( )
	     {
	     	     return  DateTime.Now.ToString ();  // -> gibt aus: 16.02.2017 09:32:35
	     }  
	     
	     public int unix()
	     {
	     	   return  (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;  // -> gibt aus: 1487235150
	     }
	     
	     public string unixDatum(int unixStempel,string was = null)
       {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);  /*  gerechnet wird ab der UNIX Epoche  - Startpunkt - */
            dateTime = dateTime.AddSeconds(unixStempel); /* Die Zeit was vergangen ist auf den Startpunkt addieren */
            
            /* INFO:  Unix Zeit stimmt bei Umrechnung es fehlen aber trodsdem eine Stunde ?? Fehler wird mit berechnung winter sommer behoben hier */
            if( TimeZoneInfo.Local.IsDaylightSavingTime( dateTime ) ) /* Sommer / Winterzeit Prüfen wo sich Datum befindet */
             { }   /* Es ist Sommer Zeitangabe Stimmt  */
            else 
              dateTime = dateTime.AddSeconds(3600);  /* Es ist Winter Einse Stunde Vorstellen */
              
            string ausgabe = string.Empty;
            if(was == "datum")
              ausgabe = dateTime.ToShortDateString();
            else if(was == "uhrzeit")
              ausgabe = dateTime.ToLongTimeString();
            else if(was == "br")
              ausgabe = dateTime.ToShortDateString() +"<br />"+ dateTime.ToLongTimeString();
            else
              ausgabe = dateTime.ToShortDateString() +", "+ dateTime.ToLongTimeString();
           
            return ausgabe;
       }
	      
   }

   public class BeepSong
   {
   	
   	     public void denahy()
   	     {
   	     	         Console.Beep(704,750);
   	     	         Console.Beep(792,250);
   	     	         Console.Beep(880,500);
   	     	         Console.Beep(792,500);
   	     	         Console.Beep(940,500);
   	     	         Console.Beep(880,500);
   	     	         Console.Beep(792,250);
   	     	         Console.Beep(660,250);
   	     	         Console.Beep(704,500);
   	     	         Console.Beep(1188,500);
   	     	         Console.Beep(1056,500);
   	     	         Console.Beep(940,500);
   	     	         Console.Beep(880,500);
   	     	         Console.Beep(792,500);
   	     	         Console.Beep(880,250);
   	     	         Console.Beep(704,250);
   	     	         Console.Beep(1056,1000);
   	     	         Console.Beep(704,750);
   	     	         Console.Beep(792,250);
   	     	         Console.Beep(880,500);
   	     	         Console.Beep(792,500);
   	     	         Console.Beep(940,500);
   	     	         Console.Beep(880,500);
   	     	         Console.Beep(792,250);
   	     	         Console.Beep(660,250);
   	     	         Console.Beep(704,500);
   	     	         Console.Beep(1188,500);
   	     	         Console.Beep(1056,500);
   	     	         Console.Beep(940,500);
   	     	         Console.Beep(880,500);
   	     	         Console.Beep(792,500);
   	     	         Console.Beep(880,250);
   	     	         Console.Beep(704,250);
   	     	         Console.Beep(1056,1000);
   	     	         Console.Beep(792,500);
   	     	         Console.Beep(880,500);
   	     	         Console.Beep(792,250);
   	     	         Console.Beep(660,250);
   	     	         Console.Beep(528,500);
   	     	         Console.Beep(940,500);
   	     	         Console.Beep(880,500);
   	     	         Console.Beep(792,250);
   	     	         Console.Beep(660,250);
   	     	         Console.Beep(528,500);
   	     	         Console.Beep(1056,500);
   	     	         Console.Beep(940,500);
   	     	         Console.Beep(880,750);
   	     	         Console.Beep(880,250);
   	     	         Console.Beep(990,500);
   	     	         Console.Beep(940,250);
   	     	         Console.Beep(1056,250);
   	     	         Console.Beep(1056,1000);
   	     	         Console.Beep(1408,750);
   	     	         Console.Beep(1320,250);
   	     	         Console.Beep(1320,250);
   	     	         Console.Beep(1188,250);
   	     	         Console.Beep(1056,500);
   	     	         Console.Beep(1188,750);
   	     	         Console.Beep(1056,250);
   	     	         Console.Beep(1056,250);
   	     	         Console.Beep(940,250);
   	     	         Console.Beep(880,500);
   	     	         Console.Beep(792,750);
   	     	         Console.Beep(880,125);
   	     	         Console.Beep(940,125);
   	     	         Console.Beep(1056,250);
   	     	         Console.Beep(1188,250);
   	     	         Console.Beep(940,250);
   	     	         Console.Beep(792,250);
   	     	         Console.Beep(704,500);
   	     	         Console.Beep(880,250);
    	     	       Console.Beep(792,250);
   	     	         Console.Beep(704,1000);
   	     	         Console.Beep(1408,750);
   	     	         Console.Beep(1320,250);
   	     	         Console.Beep(1320,250);
   	     	         Console.Beep(1188,250);
   	     	         Console.Beep(1056,500);
   	     	         Console.Beep(1188,750);
   	     	         Console.Beep(1056,250);
   	     	         Console.Beep(1056,250);
   	     	         Console.Beep(940,250);
   	     	         Console.Beep(880,500);
   	     	         Console.Beep(792,750);
   	     	         Console.Beep(880,125);
   	     	         Console.Beep(940,125);
   	     	         Console.Beep(1056,250);
   	     	         Console.Beep(1188,250);
   	     	         Console.Beep(940,250);
   	     	         Console.Beep(792,250);
   	     	         Console.Beep(704,500);
   	     	         Console.Beep(880,250);
   	     	         Console.Beep(792,250);
   	     	         Console.Beep(704,1000);
   	     }
   	     
   	     public void mario(string block)
   	     {
   	     	   if(block == "alle" || block == "1")
   	     	   {
   	     	         Console.Beep(480,200);
   	     	         Console.Beep(1568,200);
   	     	         Console.Beep(1568,200);
   	     	         Console.Beep(1568,200);
   	     	         Console.Beep(739,200);
   	     	         Console.Beep(783,200);
   	     	         Console.Beep(783,200);
   	     	         Console.Beep(783,200);
   	     	         Console.Beep(369,200);
                   Console.Beep(392,200);
                   Console.Beep(369,200);
                   Console.Beep(392,200);
                   Console.Beep(392,400);
                   Console.Beep(196,400);
             }
             else {}
             
             if(block == "alle" || block == "2")
   	     	   {

                   Console.Beep(739,200);
                   Console.Beep(783,200);
                   Console.Beep(783,200);
                   Console.Beep(739,200);
                   Console.Beep(783,200);
                   Console.Beep(783,200);
                   Console.Beep(739,200);
                   Console.Beep(83,200);
                   Console.Beep(880,200);
                   Console.Beep(830,200);
                   Console.Beep(880,200);
                   Console.Beep(987,400);
             }
             else {}
             
             if(block == "alle" || block == "3")
   	     	   {
                   Console.Beep(880,200);
                   Console.Beep(783,200);
                   Console.Beep(698,200);
                   Console.Beep(739,200);
                   Console.Beep(783,200);
                   Console.Beep(783,200);
                   Console.Beep(739,200);
                   Console.Beep(783,200);
                   Console.Beep(783,200);
                   Console.Beep(739,200);
                   Console.Beep(783,200);
                   Console.Beep(880,200);
                   Console.Beep(830,200);
                   Console.Beep(880,200);
                   Console.Beep(987,400);
             }
             else 
             {  }

   	     	   	
   	     }
   	     	
   	     
   	     public void tetris(string block)
   	     {  
   	     	  if(block == "alla" ||block == "1")
   	     	  {  /* Normaler Song */
   	     	         Console.Beep(1320,500);
                   Console.Beep(990,250);
                   Console.Beep(1056,250);
                   Console.Beep(1188,250);
                   Console.Beep(1320,125);
                   Console.Beep(1188,125);
                   Console.Beep(1056,250);
                   Console.Beep(990,250);
                   Console.Beep(880,500);
                   Console.Beep(880,250);
                   Console.Beep(1056,250);
                   Console.Beep(1320,500);
                   Console.Beep(1188,250);
                   Console.Beep(1056,250);
                   Console.Beep(990,750);
                   Console.Beep(1056,250);
                   Console.Beep(1188,500);
                   Console.Beep(1320,500);
                   Console.Beep(1056,500);
                   Console.Beep(880,500);
                   Console.Beep(880,500);
                   Thread.Sleep(250);
            }
            else {}

            if(block == "alla" ||block == "2")
   	     	  {   /* Normaler Song */
                   Console.Beep(1188,500);
                   Console.Beep(1408,250);
                   Console.Beep(1760,500);
                   Console.Beep(1584,250);
                   Console.Beep(1408,250);
                   Console.Beep(1320,750);
                   Console.Beep(1056,250);
                   Console.Beep(1320,500);
                   Console.Beep(1188,250);
                   Console.Beep(1056,250);
                   Console.Beep(990,500);
                   Console.Beep(990,250);
                   Console.Beep(1056,250);
                   Console.Beep(1188,500);
                   Console.Beep(1320,500);
                   Console.Beep(1056,500);
                   Console.Beep(880,500);
                   Console.Beep(880,500);
                   Thread.Sleep(500);
            }
            else {}
            
            if(block == "alla" ||block == "3")
   	     	  {    /* etwas schneller */
                   Console.Beep(1320,500);
                   Console.Beep(990,250);
                   Console.Beep(1056,250);
                   Console.Beep(1188,250);
                   Console.Beep(1320,125);
                   Console.Beep(1188,125);
                   Console.Beep(1056,250);
                   Console.Beep(990,250);
                   Console.Beep(880,500);
                   Console.Beep(880,250);
                   Console.Beep(1056,250);
                   Console.Beep(1320,500);
                   Console.Beep(1188,250);
                   Console.Beep(1056,250);
                   Console.Beep(990,750);
                   Console.Beep(1056,250);
                   Console.Beep(1188,500);
                   Console.Beep(1320,500);
                   Console.Beep(1056,500);
                   Console.Beep(880,500);
                   Console.Beep(880,500);
                   Thread.Sleep(250);
            }
            else{ }
            
            if(block == "alla" ||block == "4")
   	     	  {    /* normal */
                   Console.Beep(1188,500);
                   Console.Beep(1408,250);
                   Console.Beep(1760,500);
                   Console.Beep(1584,250);
                   Console.Beep(1408,250);
                   Console.Beep(1320,750);
                   Console.Beep(1056,250);
                   Console.Beep(1320,500);
                   Console.Beep(1188,250);
                   Console.Beep(1056,250);
                   Console.Beep(990,500);
                   Console.Beep(990,250);
                   Console.Beep(1056,250);
                   Console.Beep(1188,500);
                   Console.Beep(1320,500);
                   Console.Beep(1056,500);
                   Console.Beep(880,500);
                   Console.Beep(880,500);
                   Thread.Sleep(500);
            }
            else { }
            
            if(block == "alla" ||block == "5")
   	     	  {   /* etwas Tiefer und lang */
                   Console.Beep(660,1000);
                   Console.Beep(528,1000);
                   Console.Beep(594,1000);
                   Console.Beep(495,1000);
                   Console.Beep(528,1000);
                   Console.Beep(440,1000);
                   Console.Beep(419,1000);
                   Console.Beep(495,1000);
                   Console.Beep(660,1000);
                   Console.Beep(528,1000);
                   Console.Beep(594,1000);
                   Console.Beep(495,1000);
                   Console.Beep(528,500);
                   Console.Beep(660,500);
                   Console.Beep(880,1000);
                   Console.Beep(838,2000);
                   Console.Beep(660,1000);
                   Console.Beep(528,1000);
                   Console.Beep(594,1000);
                   Console.Beep(495,1000);
                   Console.Beep(528,1000);
                   Console.Beep(440,1000);
                   Console.Beep(419,1000);
                   Console.Beep(495,1000);
                   Console.Beep(660,1000);
                   Console.Beep(528,1000);
                   Console.Beep(594,1000);
                   Console.Beep(495,1000);
                   Console.Beep(528,500);
                   Console.Beep(660,500);
                   Console.Beep(880,1000);
                   Console.Beep(838,2000); 
                   Thread.Sleep(500);
   	     	  }
   	     	  else { }
   	     	
   	     }
   	
   }
 
   
   public class PortZuweisung
   {    
   	     private string  proto_woher  = "NOC_Mysql_Zugang";
	   	   private string  proto_datei  = "/klassen/mequery.cs";
	  	   private string  proto_klasse = "PortZuweisung";
	  	   private string  proto_gruppe = "portklasse";
	  	   Protokoll protokoll = new Protokoll();
   	
   	     public class Port_List 
         {   
       	    /* Klasse ist für List   */
       	    
       	    public int port;
            public string bezeichnung;
            public int max_verbindung;
            
            
            
            public Port_List(){}
            
            public Port_List(int port, string bezeichnung , int max_verbindung )
            {
            	  this.port           = port; 
   	            this.bezeichnung    = bezeichnung;
   	            this.max_verbindung = max_verbindung;
   	        }
   	        
   	     }
   	     
   	     /* Port Liste */
   	     public static List<Port_List> liste;
   	     
   	     private void erstellen()
   	     {   
   	     	    List<Port_List> port_daten = new List<Port_List>();
             
   	     	    port_daten.Add(new Port_List( 4411 , "cfy_rohdaten" , 1  )  );
   	     	    port_daten.Add(new Port_List( 88   , "http"         , 500  ) );
   	     	    
   	     	    liste = port_daten;
   	     }
   	     
   	     public void portlist()
   	     {
   	     	  protokoll.erstellen( proto_woher , proto_gruppe , "PortListe wird erstellt." , proto_datei ,proto_klasse,"portlist()" , false );  /* Protokoll Schreibe */  
   	     	  erstellen();
   	     }
   	    
   }
   
   
   public class Host
   {   
   	   public   string  name;
   	   private  string  proto_woher  = "Clary_Daten_List_Erstellung";
	  	 private  string  proto_datei  = "/klassen/mequery.cs";
	  	 private  string  proto_klasse = "Host";
	  	 private  Protokoll protokoll = new Protokoll();
   	   
   	   public Host()
   	   {  /* Konstruktur für Host  /-> Name für Maschiene raussuchen und in Variable legen  */
   	      this.name   = Environment.MachineName;
       }
   	   
   	   public IPAddress[] daten(string webname = null )
   	   {  /* Diese Funktion hollt die gesamten Daten vom Host  */ 
   	   	  
   	   	  if(webname != null )name = webname; else {}
   	   	  
   	   	  return  System.Net.Dns.GetHostEntry( name ).AddressList;
   	   }
   	   
   	   
   	   public class Neztwerk_List 
       {   
       	    /* Klasse ist für List  netzwerk_daten() */
       	    
       	    public string name;
            public string type;
            public string status;
            public string ip4_or_ip6;
            public string physical_adresse;
            public string max_lebenszeit;
            public string offen_lebenszeit;
            public string ip_adresse;
            public string aktiv_ip;
            
            public Neztwerk_List(){}
            
            public Neztwerk_List(string name, string type, string status , string ip4_or_ip6 , string physical_adresse , 
                                 string max_lebenszeit , string offen_lebenszeit , string	ip_adresse , string aktiv_ip  )
            {
            	  this.name    = name;
            	  this.type    = type;
            	  this.status  = status;
            	  this.ip4_or_ip6       = ip4_or_ip6;
            	  this.physical_adresse = physical_adresse;
            	  this.max_lebenszeit   = max_lebenszeit;
            	  this.offen_lebenszeit = offen_lebenszeit;
            	  this.ip_adresse       = ip_adresse;
            	  this.aktiv_ip         = aktiv_ip;
            }
           
    
       }
   	   
   	   
   	   
   	   
   	   public List<Neztwerk_List> netzwerk_daten()
   	   { 
   	   	  
   	   	  /*  Anzahl der Zeilen: 
   	   	      
   	   	        netzw_daten.Count 
   	   	    
   	   	     Beispiel wie Werte abgerufen werden:
   	   	      
   	   	      List<Host.Neztwerk_List> netzw_daten = host.netzwerk_daten(); 
       	     
       	      string zw_ausgabe = "";
       	   
       	      foreach (Host.Neztwerk_List netz in netzw_daten)
              {
                 zw_ausgabe += " Name: " + netz.name + " Type: " + netz.type + "\n";
              }
   	   	   */
   	   	   
   	      List<Neztwerk_List> netzw_daten = new List<Neztwerk_List>();
           
   	      string name   = string.Empty;   /* Name der Schnittstelle */
   	      string type   = string.Empty;   /* Typ der Schnittstelle */
   	      string status = string.Empty;   /* Schnittstellen Status ob diese Aktiv oder nicht ist */
   	      string ip4_or_ip6       = string.Empty;   /* Welche IP Schnittstelle IPV4 oder IPV6 */
   	      string physical_adresse = string.Empty;  /* Physiche Adresse für Netzwerkkarte / Schnittstelle */
   	      string max_lebenszeit   = string.Empty;  /* Maximale Lebenszeit von IP ( wurde vom DNS Server zugewiesen )  */ 
   	      string offen_lebenszeit = string.Empty;  /* in secunden was noch offen ist von der Lebenszeit */ 
   	      string ip_adresse       = string.Empty;  /* IP Adresse der Schnittstelle */
   	      string aktiv_ip         = string.Empty;  /* Aktive IP Adresse */ 
   	      
   	      protokoll.erstellen( proto_woher , "Neztwerk_List" , "Neztwerk Daten werden bereitgestellt." , proto_datei ,proto_klasse,"netzwerk_daten()" , false );
   	      try
   	      {
   	         NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
             foreach (NetworkInterface adapter in interfaces)
             { 
                name = adapter.Name;
                type = adapter.NetworkInterfaceType.ToString();
                status = adapter.OperationalStatus.ToString();
                physical_adresse = adapter.GetPhysicalAddress().ToString();
               
                ip4_or_ip6 = "";  aktiv_ip       = "";
                ip_adresse = "";  max_lebenszeit = "";
                
                if (adapter.Supports(NetworkInterfaceComponent.IPv4))ip4_or_ip6 = "IPv4";
                else if (adapter.Supports(NetworkInterfaceComponent.IPv6))ip4_or_ip6 = "IPv6"; else {}
                 
                 IPInterfaceProperties fIPInterfaceProperties = adapter.GetIPProperties();
                UnicastIPAddressInformationCollection  UnicastIPAddressInformationCollection = fIPInterfaceProperties.UnicastAddresses;
                foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                {        
                	      /* IP Adresse ermitteln */
                        ip_adresse =  UnicastIPAddressInformation.Address.ToString(); // Ip Address
                        
                        /* Prüfen ob IP Aktiv ist (ob die IP (Internet Protocol) Adresse gültig in einer Datenbank (DNS, Domain Name System)  ) */
                        if(UnicastIPAddressInformation.IsDnsEligible == true || ip_adresse == "127.0.0.1")
                         aktiv_ip = "ja";  else  aktiv_ip = "nein";
                         
                        try
                        {  max_lebenszeit = UnicastIPAddressInformation.DhcpLeaseLifetime.ToString(); }  /* Maximale Lebensdauer ermitteln was vom DNS zugewiesen wurde in Secunden */  
                        catch(SocketException e)
                        {   string fehlermeldung = String.Format("SocketException: {0}", e.Message);                        	  
                        	  protokoll.erstellen( proto_woher , "Neztwerk_List" , "Maximale Lebensdauer Fehler: " + fehlermeldung , proto_datei ,proto_klasse,"netzwerk_daten()" , true );
                        }

                        try  
                        { offen_lebenszeit = UnicastIPAddressInformation.AddressValidLifetime.ToString(); } /* Verbleibende lebenszeit der IP Adresse in Secunden */
                        catch(SocketException e)
                        {   string fehlermeldung = String.Format("SocketException: {0}", e.Message);                        	  
                        	  protokoll.erstellen( proto_woher , "Neztwerk_List" , "Verbleibende lebenszeit Fehler: " + fehlermeldung , proto_datei ,proto_klasse,"netzwerk_daten()" , true );
                        }

                }               
             
                netzw_daten.Add(new Neztwerk_List( name , type , status , ip4_or_ip6 , physical_adresse , max_lebenszeit , offen_lebenszeit , ip_adresse , aktiv_ip  ) ); 
             }
   	    
   	      }
   	      catch(SocketException e)
          {   string fehlermeldung = String.Format("SocketException: {0}", e.Message);                        	  
              protokoll.erstellen( proto_woher , "Neztwerk_List" , "Fataler Fehler beim holen der Netzwerkdaten: " + fehlermeldung , proto_datei ,proto_klasse,"netzwerk_daten()" , true );
          }
        
   	      return netzw_daten;   	   
       }
   	   
   	   
   	
   }
    
   public class AsciiPic
   {
   	
   	    public string computer()
   	    {
   	    	  string ausgabe = string.Empty;
   	    	  ausgabe += "\n"; ausgabe += @"                                                    ";
            ausgabe += "\n"; ausgabe += @"                       ___..-.---.---.--..___       ";
            ausgabe += "\n"; ausgabe += @"               _..-- `.`.   `.  `.  `.      --.._     ";
            ausgabe += "\n"; ausgabe += @"              /    ___________\   \   \______    \    ";
            ausgabe += "\n"; ausgabe += @"              |   |.-----------`.  `.  `.---.|   |    ";
            ausgabe += "\n"; ausgabe += @"              |`. |'  \`.        \   \   \  '|   |    ";
            ausgabe += "\n"; ausgabe += @"              |`. |'   \ `-._     `.  `.  `.'|   |    ";
            ausgabe += "\n"; ausgabe += @"             /|   |'    `-._o)\  /(o\   \   \|   |\   ";
            ausgabe += "\n"; ausgabe += @"           .' |   |'  `.     .'  '.  `.  `.  `.  | `. ";
            ausgabe += "\n"; ausgabe += @"          /  .|   |'    `.  (_.==._)   \   \   \ |.  \         _.--.    ";
            ausgabe += "\n"; ausgabe += @"        .' .' |   |'      _.-======-._  `.  `.  `. `. `.    _.-_.-'\\   ";
            ausgabe += "\n"; ausgabe += @"       /  /   |   |'    .'   |_||_|   `.  \   \   \  \  \ .'_.'     ||  ";
            ausgabe += "\n"; ausgabe += @"      / .'    |`. |'   /_.-'========`-._\  `.  `-._`._`. \(.__      :|  ";
            ausgabe += "\n"; ausgabe += @"     ( '      |`. |'.______________________.'\      _.) ` )`-._`-._/ /  ";
            ausgabe += "\n"; ausgabe += @"      \\      |   '.------------------------.'`-._-'    //     `-._.'   ";
            ausgabe += "\n"; ausgabe += @"      _\\_    \    | NOC Ende  O O O O  `.`.|    '     //  ";
            ausgabe += "\n"; ausgabe += @"     (_  _)    '-._|________________________|_.-'|   _//_  ";
            ausgabe += "\n"; ausgabe += @"     /  /      /`-._      |`-._     / /      /   |  (_  _) ";
            ausgabe += "\n"; ausgabe += @"   .'   \     |`-._ `-._   `-._`-._/ /      /    |    \  \ ";
            ausgabe += "\n"; ausgabe += @"  /      `.   |    `-._ `-._   `-._|/      /     |    /   `.    ";
            ausgabe += "\n"; ausgabe += @" /  / / /. )  |  `-._  `-._ `-._          /     /   .'      \   ";
            ausgabe += "\n"; ausgabe += @"| | | \ \|/   |  `-._`-._  `-._ `-._     /     /.  ( .\ \ \  \  ";
            ausgabe += "\n"; ausgabe += @" \ \ \ \/     |  `-._`-._`-._  `-._ `-._/     /  \  \|/ / | | | ";
            ausgabe += "\n"; ausgabe += @"  `.\_\/       `-._  `-._`-._`-._  `-._/|    /|   \   \/ / / /  ";
            ausgabe += "\n"; ausgabe += @"              /    `-._  `-._`-._`-._  ||   / |    \   \/_/.'   ";
            ausgabe += "\n"; ausgabe += @"            .'         `-._  `-._`-._  ||  /  |     \     ";
            ausgabe += "\n"; ausgabe += @"   ME      /           / . `-._  `-._  || /   |      \    ";
            ausgabe += "\n"; ausgabe += @"          '\          / /      `-._    ||/'._.'       \   ";
            ausgabe += "\n"; ausgabe += @"           \`.      .' /           `-._|/              \  ";
            ausgabe += "\n"; ausgabe += @"            `.`-._.' .'               \               .'  ";
            ausgabe += "\n"; ausgabe += @"              `-.__\/                 `\            .' '  ";
            ausgabe += "\n"; ausgabe += @"                                       \`.       _.' .'   ";
            ausgabe += "\n"; ausgabe += @"                                        `.`-._.-' _.'     ";
            ausgabe += "\n"; ausgabe += @"                                          `-.__.-'        ";
              
              
              
              return ausgabe;
   	    	
   	    	
   	    }
   	    
   	    public string warten()
   	    {
   	    	    
            string ausgabe = string.Empty;
            ausgabe += "\n"; ausgabe += @"                                    ";
   	    	  ausgabe += "\n"; ausgabe += @"                     .--. ";
   	    	  ausgabe += "\n"; ausgabe += @"                    (, , )           .-----._  ___ ";
   	    	  ausgabe += "\n"; ausgabe += @"                     <  /)           | NOC | ||==|| ";
   	    	  ausgabe += "\n"; ausgabe += @"                   _(())\)           |     | /|==||  __ ";
   	    	  ausgabe += "\n"; ausgabe += @"                .-' (()/  '-.        :_____:/ |__|/)  /| ";
   	    	  ausgabe += "\n"; ausgabe += @"              _/     ()      \    / .-------.  __.'  / | ";
   	    	  ausgabe += "\n"; ausgabe += @"             oo)__/   ()  \  |   / '======='  ()    /  | ";
   	    	  ausgabe += "\n"; ausgabe += @"             :~    \_  ) _/ _/  /__________________/   | ";
   	    	  ausgabe += "\n"; ausgabe += @"             |      |- (--|(,/ |           [___o___]   | ";
   	    	  ausgabe += "\n"; ausgabe += @"             |     /   )   \   |   /       [___o___]   / ";
   	    	  ausgabe += "\n"; ausgabe += @"             |     |  (     \  |  /        [___o___]  / ";
   	    	  ausgabe += "\n"; ausgabe += @"         ME  |     |        (  | /                 | / ";
   	    	  ausgabe += "\n"; ausgabe += @"             |     /  .     |  |/                  |/ ";
   	    	  ausgabe += "\n"; ausgabe += @"             |     |  :     |         ";
   	    	  ausgabe += "\n"; ausgabe += @"             |     |__/_____\         ";
            
   	    	  return ausgabe;
   	    }
   	
   	
   }
   
   public class ConsolenAnimation
   {  /* Konsolen Animationen */
   	      
   	    	public class LadeStatus
   	      {           private double aktuellProzent;
   	    	            private double gesamtWert;        /* Gesamt Wert von dem alles aus berechnet wird ( 100% ) */
   	    	            private double einWert;           /* ein Wert was 1 % darstellt  */
   	    	            private double prozentProLauf;      /* ein Wert was pro Durchlauf darstellt */
   	    	            private int top;
   	    	            private int left;
   	    	            private ConsoleColor farbe = new ConsoleColor();
   	    	           
   	    	            public LadeStatus( double gesamtWert , int left , int top)
   	    	            {
   	    	     	          this.gesamtWert  = gesamtWert;
   	    	     	          this.einWert     = gesamtWert / 100.00;   /* ein Wert was 1 % darstellt  */
   	    	     	          this.prozentProLauf =  100.00 / gesamtWert;  /* Prozent pro Lauf pro Durchlauf darstellt */
   	    	     	          this.left        = left;
   	    	     	          this.top         = top;
   	    	            }
   	    	      
   	    	            public void statusEins( )
   	    	            {    
   	    	            	
   	    	            	      this.aktuellProzent += this.prozentProLauf;  /* Pro Durchlauf  Prozentwert hinzuaddieren */
   	    	          	     	  
                              /* farbe defienieren für Hintergrund */
                                   if( this.aktuellProzent < 25.00 )farbe =  ConsoleColor.Red;
                              else if( this.aktuellProzent < 50.00 )farbe =  ConsoleColor.Blue;
                              else if( this.aktuellProzent < 75.00 )farbe =  ConsoleColor.Yellow;
                              else                                  farbe =  ConsoleColor.Green;

                              
                              Console.CursorVisible = false;
                              Console.SetCursorPosition( this.left , this.top ); /* Position festlegen */
                              Console.BackgroundColor  =  farbe; /* Hintergrund Farbe zuweisen */
                              Console.ForegroundColor  =  ConsoleColor.Black; /* Text Frabe zuweisen */
                              if(this.aktuellProzent > 100)this.aktuellProzent = 100; else {} /* Schutz das nicht größer als 100 */ 
   	    	          	     	  Console.Write( " Sende " + this.gesamtWert + " Zeilen. Bitte Warten Sie. Aktuell sind " +Convert.ToInt16(Math.Ceiling(this.aktuellProzent))  + "% erledigt.");
   	    	          	     	  Console.ResetColor(); /* auf Standart Farbzuweisung gehen zurückgehen */
   	    	          	    
   	    	            } 
   	    	 
   	    	}
   	       	
   	
   }
}