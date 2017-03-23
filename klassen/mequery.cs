/*
   *************************************************************************************************************
   /      MEQuery - Modul -                                                                                    /
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
using System.Diagnostics;
using System.Reflection;




namespace MEQuery
{   

   public class Debuger
   {      
   	      private Text text;
   	      private static string routeOrdner; /* Der Route Ordnder ( Erste Ebene ) */
   	      
   	      private string[] arrayWert;
   	      private string debugDaten(int stackID,string was) 
   	      {
   	      	  text = new Text();
   	      	  bool status = false;
   	      	  string ausgebe_path =  string.Empty;
   	      	  string inhalt  = string.Empty;
   	      	  try
          	  {   Array.Clear(arrayWert , 0 , arrayWert.Length );  } /* Arry Löschen zum Neubefühlen */
          	  catch { }
	            
	            try
          	  {   
          	  	if(was == "klasse" || was == "using" )
                {  StackTrace trace = new StackTrace(0);
                     StackFrame debug = trace.GetFrame(stackID);
                     arrayWert = text.split(".", debug.GetMethod().DeclaringType.FullName );  
          	              if(was == "klasse" ) inhalt = arrayWert[1];
          	         else if(was == "using")  inhalt = arrayWert[0];
          	         else {}
          	         status = true;
          	    }
          	    else 
          	    {    
          	      StackFrame debug = new StackFrame(stackID, true);
          	  	  
          	  	  if(was == "dateiName" || was == "routePath" || was == "path" )
	                {
	                	 inhalt = debug.GetFileName().ToString();
	                   if(inhalt != "" ) /* Schutz darf nicht Leer sein */
          	         { 
          	         	  arrayWert = text.split("\\", inhalt ); /* Windows Trennzeichen  \    */
          	  	        if(arrayWert.Length > 2 ) /* Schutz das  min 2 Länge */
          	  	        {  status = true;  }
                        else /* split konnte nicht Trennen Versuche Linux zeichen */
                        {
                           arrayWert = text.split("/", inhalt );  /* Linux Trennzeichen  /   */
                           if(arrayWert.Length > 2 ) /* Schutz das  min 2 Länge */
          	  	           {  status = true;  }
          	  	            else {}
                        }
                        
                        if(was == "path")
                        {
                            for(int i=0;i < arrayWert.Length -1;i++) /* Eins weniger da der Letzte Wert immer die Datei selber ist */
                            { 
              	              if(arrayWert[i] == Debuger.routeOrdner  && ausgebe_path == "" )
              	 	               ausgebe_path = "/";
              	              else if(ausgebe_path != "")
              	                 ausgebe_path += arrayWert[i] + "/"; 	
              	              else {}
                            }	
                        }
                        else {}
                        
                     }
                     else{}
                  }
                  else if(was == "funktion")
	                {   inhalt = debug.GetMethod().ToString();
	                    status = true; }
	                else if(was == "zeile")
	                {  inhalt = debug.GetFileLineNumber().ToString();
	                   status = true; }
	                else  { }
	              }  
              }    
              catch(NullReferenceException)
              {   }
              
              if(status == false )
              {  /* Fehler */
              	 return "FehlerNull";
              }
              else
              {
              	       if(was == "dateiName")return arrayWert[arrayWert.Length - 1]; 
              	  else if(was == "routePath")return arrayWert[arrayWert.Length - 2];
              	  else if(was == "path")     return ausgebe_path;
              	  else if(was == "zeile" || was ==  "funktion" || was == "klasse" || was == "using" )    return inhalt;
              	  else  return "FehlerNull";
              	
              }
              
   	      }
   	       
   	   
   	      public void routeErmitteln()
   	      { /* Route Verzeichnis ermitteln  MUSS in der Main Liegen da diese Datei immer in dem Rout Ordner liegt */ 
   	      	  
   	      	  string ausgabe = string.Empty;
   	      	  ausgabe = debugDaten(2,"routePath"); /* Normal suche wert Steht bei Ebene 2 Was gesucht wird im Normal Fall  */
   	      	  if(ausgabe == "FehlerNull")ausgabe = debugDaten(1,"routePath"); else { } /* Fallback wenn suche 2 Fehlerhaft war ( an stelle 1 schauen ) */
   	      	  if(ausgabe == "FehlerNull")ausgabe = debugDaten(0,"routePath"); else { } /* Fallback wenn suche 1 Fehlerhaft war ( an stelle 0 schauen ) */
             
              Debuger.routeOrdner = ausgabe; 
          }
   	   
   	      public  int zeile()
          {   
          	  string ausgabe = string.Empty;
   	      	  ausgabe = debugDaten(2,"zeile"); /* Normal suche wert Steht bei Ebene 2 Was gesucht wird im Normal Fall  */
   	      	  if(ausgabe == "FehlerNull")ausgabe = debugDaten(1,"zeile"); else { } /* Fallback wenn suche 2 Fehlerhaft war ( an stelle 1 schauen ) */
   	      	  if(ausgabe == "FehlerNull")ausgabe = debugDaten(0,"zeile"); else { } /* Fallback wenn suche 1 Fehlerhaft war ( an stelle 0 schauen ) */
              
              if(ausgabe == "FehlerNull") return 0;
              else return  Convert.ToInt32( ausgabe );
          }
          
          public string block()
          {
          	  string ausgabe = string.Empty;
          	  ausgabe = debugDaten(2,"using"); /* Normal suche wert Steht bei Ebene 2 Was gesucht wird im Normal Fall  */
   	      	  if(ausgabe == "FehlerNull")ausgabe = debugDaten(1,"using"); else { } /* Fallback wenn suche 2 Fehlerhaft war ( an stelle 1 schauen ) */
   	      	  if(ausgabe == "FehlerNull")ausgabe = debugDaten(0,"using"); else { } /* Fallback wenn suche 1 Fehlerhaft war ( an stelle 0 schauen ) */
          	 
          	  return ausgabe;
          }
          
          public string klasse()
          {
          	  string ausgabe = string.Empty;
          	  ausgabe = debugDaten(2,"klasse"); /* Normal suche wert Steht bei Ebene 2 Was gesucht wird im Normal Fall  */
   	      	  if(ausgabe == "FehlerNull")ausgabe = debugDaten(1,"klasse"); else { } /* Fallback wenn suche 2 Fehlerhaft war ( an stelle 1 schauen ) */
   	      	  if(ausgabe == "FehlerNull")ausgabe = debugDaten(0,"klasse"); else { } /* Fallback wenn suche 1 Fehlerhaft war ( an stelle 0 schauen ) */
          	 
          	  return ausgabe;
          }
          
          
          public  string funktion()
          {   
          	  string ausgabe = string.Empty;
          	  ausgabe = debugDaten(2,"funktion"); /* Normal suche wert Steht bei Ebene 2 Was gesucht wird im Normal Fall  */
   	      	  if(ausgabe == "FehlerNull")ausgabe = debugDaten(1,"funktion"); else { } /* Fallback wenn suche 2 Fehlerhaft war ( an stelle 1 schauen ) */
   	      	  if(ausgabe == "FehlerNull")ausgabe = debugDaten(0,"funktion"); else { } /* Fallback wenn suche 1 Fehlerhaft war ( an stelle 0 schauen ) */
             
              return ausgabe;
          }
          
          
          public string dateiName()
          {  
          	  string ausgabe = string.Empty;
   	      	  ausgabe = debugDaten(2,"dateiName"); /* Normal suche wert Steht bei Ebene 2 Was gesucht wird im Normal Fall  */
   	      	  if(ausgabe == "FehlerNull")ausgabe = debugDaten(1,"dateiName"); else { } /* Fallback wenn suche 2 Fehlerhaft war ( an stelle 1 schauen ) */
   	      	  if(ausgabe == "FehlerNull")ausgabe = debugDaten(0,"dateiName"); else { } /* Fallback wenn suche 1 Fehlerhaft war ( an stelle 0 schauen ) */
                   
           	  return ausgabe;         
          }
   	      
   	      public string path()
          {   
          	  string ausgabe = string.Empty;
   	      	  ausgabe = debugDaten(2,"path"); /* Normal suche wert Steht bei Ebene 2 Was gesucht wird im Normal Fall  */
   	      	  if(ausgabe == "FehlerNull")ausgabe = debugDaten(1,"path"); else { } /* Fallback wenn suche 2 Fehlerhaft war ( an stelle 1 schauen ) */
   	      	  if(ausgabe == "FehlerNull")ausgabe = debugDaten(0,"path"); else { } /* Fallback wenn suche 1 Fehlerhaft war ( an stelle 0 schauen ) */
              
              return ausgabe;
          }
   }

	 public class Einstellung
   {   
   	   public static int cfy_rohdaten_port;
   	   public static int http_port;
   	   public static int https_port;
   	   public static int admin_port = 14778;  /* Dieser Port ist nicht Änderbar von ausen */
   	   public static int extern_port;
   	   public static string ip_adresse;
   	   public static bool sound;
   	   
   	   private Protokoll protokoll = new Protokoll();
   	   private Debuger debuger = new Debuger();
	  	 private string  proto_gruppe = Environment.MachineName;
	  	 
	  	 public bool laden( )
   	   {
   	   	 Text  text   = new Text();
   	   	 Datum datum  = new Datum();
   	   	 bool  status = false;
   	   	 int   er     = 0;
   	   	 
   	   	 try
   	   	 {    
   	   	 	 protokoll.erstellen( debuger.block() , proto_gruppe , "config.noc Datei Laden.",debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
   	   	   if( File.Exists(@"config_server.noc")  == false )
           {  /* Datei war noch nicht Vorhanden HTML Kopf Schreiben un als erstes anhängen */ 
              protokoll.erstellen( debuger.block() , proto_gruppe , "config_server.noc Datei war nicht vorhanden erstelle Default config." , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
              
              string[] inhalt_datei  = new string[39];
              inhalt_datei[0]  = "#";
              inhalt_datei[1]  = "#   ************************************************************************************************************* ";
              inhalt_datei[2]  = "#   /      NOC Portal Server Einstellungsdatei                                                                  / ";
              inhalt_datei[3]  = "#   /                                                                                                           / ";
              inhalt_datei[4]  = "#   /                                                                                                           / ";
              inhalt_datei[5]  = "#   /      Cod by Meiko Eichler                                                                                 / ";
              inhalt_datei[6]  = "#   /      Copyright by Meiko Eichler                                                                           / ";
              inhalt_datei[7]  = "#   /      Handy: 0163 7378481                                                                                  / ";
              inhalt_datei[8]  = "#   /      Email: Meiko@Somba.de                                                                                / ";
              inhalt_datei[9]  = "#   /                                                                                                           / ";
              inhalt_datei[10] = "#   /      Datei erstellt am 14.03.2017                                                                         / ";
              inhalt_datei[11] = "#   /      Generiert am " + datum.datum_zeit() + "                                                              / ";
              inhalt_datei[12] = "#   /                                                                                                           / ";
              inhalt_datei[13] = "#   /      Datei Name: config_server.noc                                                                        / ";
              inhalt_datei[14] = "#   /                                                                                                           / ";
              inhalt_datei[15] = "#   /      Werte immer in  \" \" schreiben!                                                                     / ";
              inhalt_datei[16] = "#   /      Alle 4 Felder müssen angegeben werden sonst bricht Programm ab!                                      / ";
              inhalt_datei[17] = "#   /      als Wert kann immer \"default\"  angegeben werden Es werden dann systemeinstellungen genommen vom Pr./ ";
              inhalt_datei[18] = "#   /      bei sound gibt es zwei werte aus und an  ( standart bei default )                                    / ";
              inhalt_datei[19] = "#   /      Tip! Portangabe geht nur bis zirka 65500                                                             / ";
              inhalt_datei[20] = "#   /                                                                                                           / ";
              inhalt_datei[21] = "#   /      Achten Sie auf die \"default\" Werte!                                                                / ";
              inhalt_datei[22] = "#   /      ip_adresse        = System IP Adressen                                                               / ";
              inhalt_datei[23] = "#   /      cfy_rohdaten_port = 4411                                                                             / ";
              inhalt_datei[24] = "#   /      http_port         = 88                                                                               / ";
              inhalt_datei[25] = "#   /      https_port        = 20779                                                                            / ";
              inhalt_datei[26] = "#   /      extern_port       = 31482                                                                            / ";
              inhalt_datei[27] = "#   /      admin_port        = 14778     Tip: admin Port ist nicht änderbar! FIX                                / ";
              inhalt_datei[28] = "#   /                                                                                                           / ";
              inhalt_datei[29] = "#   ************************************************************************************************************* ";
              inhalt_datei[30] = "";
              inhalt_datei[31] = "";
              inhalt_datei[32] = "ip_adresse          =  \"default\" ";
              inhalt_datei[33] = "cfy_rohdaten_port   =  \"default\" ";
              inhalt_datei[34] = "http_port           =  \"default\" ";
              inhalt_datei[35] = "https_port          =  \"default\" ";
              inhalt_datei[36] = "extern_port         =  \"default\" ";
              inhalt_datei[37] = "sound               =  \"default\" ";
              inhalt_datei[38] = "";        
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
   	   	   	      protokoll.erstellen( debuger.block() , proto_gruppe , "IP Adresse in Datei gefunden: " + Einstellung.ip_adresse , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
   	   	   	   }
   	   	   	   else if( text.trim(auswertung[0]) == "cfy_rohdaten_port")
   	   	   	   {
   	   	   	   	  string puffer = text.trim(auswertung[1]);
   	   	   	   	  if(text.trim(puffer , "dophoch" )  == "default" ) /* Default Einstellung nehmen */
   	   	   	   	    Einstellung.cfy_rohdaten_port     =  4411;
   	   	   	   	  else
   	   	   	        Einstellung.cfy_rohdaten_port     =  Convert.ToInt32( text.trim(puffer , "dophoch" ) );
   	   	   	      er++;
   	   	   	      protokoll.erstellen( debuger.block() , proto_gruppe , "CFY Rohdaten Port wurde in Datei gefunden: " + Einstellung.cfy_rohdaten_port , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
   	   	   	   }
   	   	   	   else if( text.trim(auswertung[0]) == "http_port")
   	   	   	   {
   	   	   	   	  string puffer = text.trim(auswertung[1]);
   	   	   	      if(text.trim(puffer , "dophoch" ) == "default" ) /* Default Einstellung nehmen */
   	   	   	        Einstellung.http_port     =  88;
   	   	   	      else
   	   	   	        Einstellung.http_port     =  Convert.ToInt32( text.trim(puffer , "dophoch" ) );
   	   	   	      er++;
   	   	   	      protokoll.erstellen( debuger.block() , proto_gruppe , "http Port wurde gefunden: " + Einstellung.http_port , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
   	   	   	   }
   	   	   	   else if( text.trim(auswertung[0]) == "https_port")
   	   	   	   {
   	   	   	   	  string puffer = text.trim(auswertung[1]);
   	   	   	      if(text.trim(puffer , "dophoch" ) == "default" ) /* Default Einstellung nehmen */
   	   	   	        Einstellung.https_port     =  20779;
   	   	   	      else
   	   	   	        Einstellung.https_port     =  Convert.ToInt32( text.trim(puffer , "dophoch" ) );
   	   	   	      er++;
   	   	   	      protokoll.erstellen( debuger.block() , proto_gruppe , "https Port wurde gefunden: " + Einstellung.https_port , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
   	   	   	   }
   	   	   	   else if( text.trim(auswertung[0]) == "extern_port")
   	   	   	   {
   	   	   	   	  string puffer = text.trim(auswertung[1]);
   	   	   	      if(text.trim(puffer , "dophoch" ) == "default" ) /* Default Einstellung nehmen */
   	   	   	        Einstellung.extern_port     =  31482;
   	   	   	      else
   	   	   	        Einstellung.extern_port     =  Convert.ToInt32( text.trim(puffer , "dophoch" ) );
   	   	   	      er++;
   	   	   	      protokoll.erstellen( debuger.block() , proto_gruppe , "Extern Port wurde gefunden: " + Einstellung.extern_port ,debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
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
   	   	   	      protokoll.erstellen( debuger.block() , proto_gruppe , "Sound Einstellung wurde in Datei gefunden: " + pr_ausgabe , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
   	   	   	   }
   	   	   	   else {}
   	   	   	   
   	   	   }
   	   	   
   	   	   if(er == 6) /* Es müssen alle 6 Parameter gefunden werden in der Ini ansonsten Bricht das System ab */ 
   	   	   {    status = true;  /* einstellungs Daten wurden alle geladen True zurückgeben das system weiter arbeitet */
   	   	   	    protokoll.erstellen( debuger.block() , proto_gruppe , "Alle Einstellungen in Config Datei gefunden." , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
   	   	   }
   	   	   else
   	   	    protokoll.erstellen( debuger.block() , proto_gruppe , "Config Datei wahr Fehlerhaft es wurden nur " + er + " Einstellungen gefunden. 6 Stück sind aber minimum." , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
   	   	   
   	   	 }
   	   	 catch (SocketException e)
         {
               string fehlermeldung = String.Format("SocketException: ", e.Message);
               protokoll.erstellen( debuger.block() , proto_gruppe , "SocketException wurde gewurfen Verbindung wurde getrennt. Fehler: " + fehlermeldung , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
         }  
   	   	 
   	   	 return status;
   	   }
   
   }
	 
	 public class Benutzer
	 {
	 	     private Debuger debuger = new Debuger();
	 	     private Protokoll protokoll = new Protokoll();
	  	   private string  proto_gruppe = "benutzer";
	  	   
	 	     
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
	 	     	  protokoll.erstellen( debuger.block() , proto_gruppe , "Benutzer Zugangs-Daten werden abgefragt." , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
	 	     	  return prufung(p_name ,p_passw, was);
	 	     }
	 	     
	 	
	 }
	 
	 public class MYSQL
	 {     
	 	     private Debuger debuger = new Debuger();
	 	     private Protokoll protokoll = new Protokoll();
	  	   private string  proto_gruppe = "mysqlZugang";
	  	   
	 	   
	 	     private  string benutzer_in()
	 	     {   return "root";   }
	 	     
	 	     private  string password_in()
	 	     {   return "pass";   }
	 	     
	 	     private  string ipadresse_in()
	 	     {   return "127.0.0.1";   }
	 	     
	 	     
	 	     public  string ben()
	 	     {  protokoll.erstellen( debuger.block() , proto_gruppe , "Benutzer für Mysql wurde abgefragt." , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
	 	     	  return benutzer_in();   }
	 	     
	 	     public  string pass()
	 	     {   protokoll.erstellen( debuger.block() , proto_gruppe , "Password für Mysql wurde abgefragt." , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
	 	     	   return password_in();   }
	 	     
	 	     public  string ip()
	 	     {   protokoll.erstellen( debuger.block() , proto_gruppe , "IP Adresse für Mysql wurde abgefragt." , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
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
   	    
   	    private Debuger debuger = new Debuger();
	  	  private string  proto_gruppe = "Protokoll";
	  	  private bool    statusRennen = true;
	  	  
   	    public void rennen()
   	    {  /* Thread  wir din der Main gestartet */
   	    	 
   	    	 bool spezial = true; /* wird benötigt fals der Thread langsam gestoppt wird */
   	    	 
   	    	 try 
   	    	 {
   	    	    while(spezial)
   	    	    {
   	    	    	  try
   	    	    	  {
   	    	    	     if(liste.Count != 0)
   	    	    	     { try
   	    	    	   	   { speichern( liste[0].woher,liste[0].gruppe ,liste[0].inhalt,liste[0].datei,liste[0].klasse,liste[0].funktion,liste[0].fehler,liste[0].path,liste[0].zeile );
                         Thread.Sleep(500); /* Eine Secunde Warten bis zum nächsten durchlauf */
                         liste.RemoveAt(0);
                         Thread.Sleep(500); /* Eine Secunde Warten bis zum nächsten durchlauf */
                       }catch{ }
                       	
   	    	    	     }
   	    	    	     else { }
   	    	    	  }
   	    	    	  catch (SocketException e)
                  {
                       erstellen( debuger.block() , proto_gruppe , "SocketException wurde gewurfen. Fehler: " + e.Message , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
                  }
                  
                  
                  
                  if( this.statusRennen == false && liste.Count == 0 ) /* wenn Protokoll Beendet werden soll alle erstlichen Protokolle schreiben und dann Beenden */
                  	 spezial = false; 
                  else { }
   	    	    }
   	    	 }
   	    	 catch { }    
   	    }
   	    
   	    public void stop()
   	    { /* Protokllierung von außen  Stopen */ 
   	    	
   	    	this.statusRennen = false;   	    	
   	    }
   	    
   	    public class Protokoll_List
   	    {
   	    	   public string  woher;
   	    	   public string  inhalt;
   	    	   public string  datei;
   	    	   public string  path;
   	    	   public string  klasse;
   	    	   public string  funktion;
   	    	   public bool    fehler;
   	    	   public int     unixzeit;
   	    	   public string  gruppe;
   	    	   public int     zeile;
   	    	   
   	    	   public Protokoll_List( ) { }   
   	    	   
   	    	   public Protokoll_List(string woher,string gruppe ,string inhalt,string datei,string klasse,string  funktion,bool fehler,int unixzeit ,string path , int zeile) 
   	    	   {
   	    	   	  this.woher  = woher;
   	    	   	  this.gruppe = gruppe;
   	    	   	  this.inhalt = inhalt;
   	    	   	  this.datei  = datei;
   	    	   	  this.klasse = klasse;
   	    	   	  this.funktion = funktion;
   	    	   	  this.fehler   = fehler;
   	    	   	  this.unixzeit = unixzeit;
   	    	   	  this.zeile = zeile;
   	    	   	  this.path  = path;
   	    	   	  
   	    	   }
   	    }
   	    
   	    /* Protokoll Liste erstellen */
   	    public static List<Protokoll_List> liste = new List<Protokoll_List>();
   	     
   	    public void erstellen(string woher,string gruppe, string inhalt,  string klasse , string path , string datei ,string  funktion , int zeile , bool fehler )
   	    {   
   	    	 
   	    	 try
   	    	 { 
   	    	   Datum datum = new Datum();
   	     	   liste.Add(new Protokoll_List( woher , gruppe , inhalt ,datei ,klasse ,funktion ,fehler , datum.unix() , path , zeile ) );
   	     	 }
   	     	 catch
   	     	 {
   	     	    erstellen(woher,gruppe,inhalt, klasse , path , datei  , funktion , zeile , fehler  );
   	     	 }
   	    }
   	    
   	    
   	    private void speichern(string woher,string gruppe ,string inhalt,string dateiname,string klasse,string  funktion,bool fehler ,string path, int zeile)
   	    {   /* Diese funktion speichert die gesamelten Daten in eine Datei auf dem system wo es gerade läuft beim beenden */

               try
               {   Datum datum = new Datum();
                   Datei datei = new Datei();
                  
               	   int    unixzeit     = datum.unix();   
               	   string td_style     = "<td style=\"background:#8fbc8f;color:#000000;font-weight:bold;text-align:left;font-size:1.0em;height:30px;\" >";
               	   string td_style_nw  = "<td style=\"background:#8fbc8f;color:#000000;font-weight:bold;text-align:left;font-size:1.0em;height:30px;\" nowrap >";
               	   string td_intern    = "<td style=\"color:#000000;text-align:left;font-size:1.0em;height:30px;\" >";
                   string td_intern_nw = "<td style=\"color:#000000;text-align:left;font-size:1.0em;height:30px;\" nowrap >";
                   string spaltenName  = "<tr>" + td_style + "Datum</td>" + td_style + "Uhrzeit</td>" + td_style + "Fehler</td>" + td_style + "Zeile</td>" + td_style + "Modul</td>"+ td_style + "Gruppe</td>" + td_style + "Inhalt</td>"  + td_style_nw + "Dateipfad</td>" + td_style_nw + "Datei</td>" + td_style_nw + "Klasse</td>" + td_style_nw + "Funktion</td></tr>";
                   string spaltenName_csv  = "Unixzeit;Datum;Uhrzeit;Fehler;Zeile;Modul;Gruppe;Inhalt;Dateipfad;Datei;Klasse;Funktion";
               	   
               	  /* HTML Protokoll in Datei Speichern */
               	   if( File.Exists(@"noc_protokoll_server.html")  == false )
               	   {    /* Datei war noch nicht Vorhanden HTML Kopf Schreiben und Datei erstellen */ 
                        
                         string[] kopf_inhalt  = new string[54]; 
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
                         kopf_inhalt[12] = "<meta http-equiv=\"refresh\" content=\"10\">";
                         kopf_inhalt[13] = "<title>NOC Portal - Server Protokoll Datei erstellt am "+ datum.datum_zeit() + "</title>";
                         kopf_inhalt[14] = "<style type=\"text/css\"> ";
                         kopf_inhalt[15] = " /* ";
                         kopf_inhalt[16] = "   *************************************************************************************************************  ";
                         kopf_inhalt[17] = "   /      NOC Portal Server - Protokoll Datei                                                                  /  ";
                         kopf_inhalt[18] = "   /                                                                                                           /  ";
                         kopf_inhalt[19] = "   /                                                                                                           /  ";
                         kopf_inhalt[20] = "   /      Cod by Meiko Eichler                                                                                 /  ";
                         kopf_inhalt[21] = "   /      Copyright by Meiko Eichler                                                                           /  ";
                         kopf_inhalt[22] = "   /      Handy: 0163 7378481                                                                                  /  ";
                         kopf_inhalt[23] = "   /      Email: Meiko@Somba.de                                                                                /  ";
                         kopf_inhalt[24] = "   /                                                                                                           /  ";
                         kopf_inhalt[25] = "   /      Datei erstellt am 14.03.2017                                                                         /  ";
                         kopf_inhalt[26] = "   /      Generiert am " + datum.datum_zeit() + "                                                              /  ";
                         kopf_inhalt[27] = "   /                                                                                                           /  ";
                         kopf_inhalt[28] = "   /      Datei Name: noc_protokoll_server.html                                                                /  ";
                         kopf_inhalt[29] = "   /                                                                                                           /  ";
                         kopf_inhalt[30] = "   /      Protokolle für die Laufzeitumgebung                                                                  /  ";
                         kopf_inhalt[31] = "   /                                                                                                           /  ";
                         kopf_inhalt[32] = "   *************************************************************************************************************  ";
                         kopf_inhalt[33] = " */ ";
                         kopf_inhalt[34] = "@charset \"UTF-8\"; ";
                         kopf_inhalt[35] = "html{ height:100%; width:100%; }    ";
                         kopf_inhalt[36] = "</style> ";
                         kopf_inhalt[37] = "<script type=\"text/javascript\"> ";
                         kopf_inhalt[38] = "function Seitenende() { ";
                         kopf_inhalt[39] = " /* document.getElementById('endeDatei').scrollIntoView(true); */ ";
                         kopf_inhalt[40] = "} ";
                         kopf_inhalt[41] = "window.onload=function(){ ";
                         kopf_inhalt[42] = "Seitenende(); ";
                         kopf_inhalt[43] = "} ";
                         kopf_inhalt[44] = "</script>";
                         kopf_inhalt[45] = "</head> ";
                         kopf_inhalt[46] = "<body bgcolor=\"#707a7d\"> ";
                         kopf_inhalt[47] = "<br /><center><table style=\"width:98%\"  border=\"3\" cellpadding=\"0\" cellspacing=\"0\"  bordercolorlight=\"#8C8E8C\" bordercolordark=\"#000000\">";
                         kopf_inhalt[48] = "<tr><th height=\"25\" style=\"color:#FFFFFF;background-color:#bd0e39;font-size:1.2em;text-align:left;height:35px;\" colspan=\"11\">NOC Portal - Server Protokoll vom "+ datum.datum_zeit() + "</td></tr>";
                         kopf_inhalt[49] = spaltenName;
                         kopf_inhalt[50] = "</table>";
                         kopf_inhalt[51] = "</center><br />";
                         kopf_inhalt[52] = "<div id='endeDatei'>";
                         kopf_inhalt[53] = "</div></body></html>";
                         
                   
                          /* Datei mit Kopf erstellen und Datei erstellen */
                         File.AppendAllLines( @"noc_protokoll_server.html" , kopf_inhalt );
                   }
   	   	           else if(Protokoll.alt_datum ==  "" || Protokoll.alt_datum != datum.unixDatum( unixzeit , "datum") ) 
   	   	           {  /* Server wurde neu gestartet oder ein Neuer Tag hat begonnen  Tabelle Neu erstellen  */
   	   	           	     
   	   	           	     string[] new_tabelle = new string[5];
   	   	           	     new_tabelle[0] = "<br /><center><table style=\"width:98%\"  border=\"3\" cellpadding=\"0\" cellspacing=\"0\"  bordercolorlight=\"#8C8E8C\" bordercolordark=\"#000000\">";
                         new_tabelle[1] = "<tr><th height=\"25\" style=\"color:#FFFFFF;background-color:#bd0e39;font-size:1.2em;text-align:left;height:35px;\" colspan=\"11\">NOC Portal - Server Protokoll vom "+ datum.datum_zeit() + "</td></tr>";
                         new_tabelle[2] = spaltenName;
                         new_tabelle[3] = "</table>";
                         new_tabelle[4] = "</center><br />";
                         
                         datei.einfugen("<div id='endeDatei'>","davor","unten",@"noc_protokoll_server.html", new_tabelle );
   	   	           }
   	   	           else {}
   	   	           
   	   	          /* CSV Protokoll in Datei Speichern */
               	   if( File.Exists(@"noc_protokoll_server.csv")  == false )
   	   	           {
   	   	           	     string[] kopf_inhalt_csv  = new string[27]; 
               	   	     kopf_inhalt_csv[0]  = "NOC Portal - Server Protokoll Datei erstellt am "+ datum.datum_zeit() ;
                         kopf_inhalt_csv[1]  = " ";
                         kopf_inhalt_csv[2]  = " ";
                         kopf_inhalt_csv[3]  = "   *************************************************************************************************************  ";
                         kopf_inhalt_csv[4]  = "   /      NOC Portal Server - CSV Protokoll Datei                                                              /  ";
                         kopf_inhalt_csv[5]  = "   /                                                                                                           /  ";
                         kopf_inhalt_csv[6]  = "   /                                                                                                           /  ";
                         kopf_inhalt_csv[7]  = "   /      Cod by Meiko Eichler                                                                                 /  ";
                         kopf_inhalt_csv[8]  = "   /      Copyright by Meiko Eichler                                                                           /  ";
                         kopf_inhalt_csv[9]  = "   /      Handy: 0163 7378481                                                                                  /  ";
                         kopf_inhalt_csv[10] = "   /      Email: Meiko@Somba.de                                                                                /  ";
                         kopf_inhalt_csv[11] = "   /                                                                                                           /  ";
                         kopf_inhalt_csv[12] = "   /      Datei erstellt am 14.03.2017                                                                         /  ";
                         kopf_inhalt_csv[13] = "   /      Generiert am " + datum.datum_zeit() + "                                                              /  ";
                         kopf_inhalt_csv[14] = "   /                                                                                                           /  ";
                         kopf_inhalt_csv[15] = "   /      Datei Name: noc_protokoll_server.csv                                                                 /  ";
                         kopf_inhalt_csv[16] = "   /                                                                                                           /  ";
                         kopf_inhalt_csv[17] = "   /      Protokolle für die Laufzeitumgebung                                                                  /  ";
                         kopf_inhalt_csv[18] = "   /                                                                                                           /  ";
                         kopf_inhalt_csv[19] = "   *************************************************************************************************************  ";
                         kopf_inhalt_csv[20] = " ";
                         kopf_inhalt_csv[21] = " ";
                         kopf_inhalt_csv[22] = "[Start]";
                         kopf_inhalt_csv[23] = spaltenName_csv;
   	   	           	     kopf_inhalt_csv[24] = "[Ende]";
   	   	           	     kopf_inhalt_csv[25] = " ";
   	   	           	     kopf_inhalt_csv[26] = "[Ende-Datei]";
   	   	           	     
   	   	           	      /* Datei mit Kopf erstellen und Datei erstellen */
                         File.AppendAllLines( @"noc_protokoll_server.csv" , kopf_inhalt_csv );
   	   	           }	
   	   	           else if(Protokoll.alt_datum ==  "" || Protokoll.alt_datum != datum.unixDatum( unixzeit , "datum") ) 
   	   	           {  /* Server wurde neu gestartet oder ein Neuer Tag hat begonnen  Block Neu erstellen  */
   	   	           	  
   	   	           	     string[] new_block_csv = new string[5];
   	   	           	     new_block_csv[0] = " ";
                         new_block_csv[1] = "[Start]";
                         new_block_csv[2] = spaltenName_csv;
   	   	           	     new_block_csv[3] = "[Ende]";
   	   	           	     new_block_csv[4] = " ";
                         
                         datei.einfugen("[Ende-Datei]","davor","unten",@"noc_protokoll_server.csv", new_block_csv );
   	   	           }
   	   	           else {}
   	   	              	   	                    
   	   	          /* Datum Neu setzen wenn sich was geändert hat oder noch nicht vergeben war */
   	   	           if(Protokoll.alt_datum ==  "" || Protokoll.alt_datum != datum.unixDatum( unixzeit , "datum") )
   	   	             Protokoll.alt_datum = datum.unixDatum( unixzeit , "datum"); /* Datum Neu setzen */
   	   	           else {}
   	   	          
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
                   string[] ubergabe_csv = new string[1];
                  
                   /* HTML */
                   ubergabe[0] = "<tr style=\" "+ farbe_ok_fehler + "; \" >" + td_intern + datum.unixDatum( unixzeit , "datum") + "</td>" + td_intern + datum.unixDatum( unixzeit , "uhrzeit") + "</td>" + td_intern + fehler_text + "</td>" + td_intern + zeile + "</td>" + td_intern_nw + woher + "</td>" + td_intern_nw + gruppe + "</td>" + td_intern + inhalt + "</td>" + td_intern_nw + path + "</td>" + td_intern_nw + dateiname + "</td>" + td_intern_nw + klasse + "</td>" + td_intern_nw + funktion + "</td></tr>";
                   /* CSV */
                   ubergabe_csv[0] = unixzeit + ";" + datum.unixDatum( unixzeit , "datum") + ";" + datum.unixDatum( unixzeit , "uhrzeit") + ";" + fehler_text + ";" + zeile + ";" + woher + ";" + gruppe + ";" + inhalt + ";" + path + ";" + dateiname + ";" + klasse + ";" + funktion ;
                  
                   if(Protokoll.durchlauf > 15){  ubergabe[1] = spaltenName;  Protokoll.durchlauf = 0; } else {}
                     
                   /* Protokoll in Datei einfügen */
                   datei.einfugen("</table>","davor","unten",@"noc_protokoll_server.html", ubergabe ); /* HTML */
                   datei.einfugen("[Ende]","davor","unten",@"noc_protokoll_server.csv", ubergabe_csv );   /* CSV Datei */
   	   	           
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
   	   	           
   	   	           Thread.Sleep(1000);
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
   	   	           
   	   	           bool gefundenAnker = false; 
   	   	            
   	   	           int ankerZahler = 1; /* Fängt bei eins an zu Zählen da Anker im einmal da sein muss um dort Daten einzuhängen */
   	   	           for(int i=0;i < dateiInhalt.Length;i++)
   	   	           { 
   	   	           	 try
   	   	           	 {
   	   	           	   if(dateiInhalt[i] == anker  && gefunden == false)
   	   	           	   {  /* Anker wurde gefunden ( Neue Daten darunter einhängen ) */
   	   	           	   	  gefundenAnker = true;
   	   	           	   	  
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
   	   	           
   	   	           /* Anker nicht gefunden Daten Fall Back auslösen  */
   	   	           if(gefundenAnker == false)
   	   	           {   
   	   	           	   /* Nicht Zuordenbare datei in spezial Datei schreiben für weiter auswertung später  */
   	   	               File.AppendAllLines( @"anker_nicht_gefunden.noc" , zeilenInhalt );
   	   	           }
   	   	           else
   	   	           {
   	   	           	   /* Datei Komplett Überschreiben  Wenn alles OK war */
   	   	               try
   	   	               { 
   	   	                	File.WriteAllLines( pathDatei , neueDaten );   
   	   	               }
   	   	               catch( PathTooLongException e)
   	   	               {  Console.WriteLine("\n Fehler von Daten Einfügen in Daten() - Datei Pfad / Name ist zu Lang! Info:" +  e.Message + "\n" ); }
   	   	               catch (UnauthorizedAccessException e)
   	   	               {  Console.WriteLine("\n Fehler von Daten Einfügen in Daten() - Keine Berechtigung! Info:" +  e.Message + "\n" );    }
   	   	               catch (IOException e)
   	   	               {  /* Versuche noch einmal die Daten zu Speichern */
   	   	               	  try { File.WriteAllLines( pathDatei , neueDaten ); } catch { }
   	   	               	  /* Gib aber Info an Konsole aus das es ein Problem gab */
   	   	               	  Console.WriteLine("\n Fehler von Daten Einfügen in Daten() - Lese Schreib Fehler! Info:" +  e.Message + "\n" );    }
   	   	           }
   	   	           
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
   	   private Debuger debuger = new Debuger();
   	   private Protokoll protokoll = new Protokoll();
	  	 private string  proto_gruppe = Environment.MachineName;
	  	
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
       	    StringSplitOptions  option = new StringSplitOptions();
       	    
       	   
       	    try
       	    { 
       	        wasSplit[0] =  Convert.ToChar(zeichen);
       	        if(optional == "andere")
   	               option = StringSplitOptions.RemoveEmptyEntries;
   	            else
   	              option = StringSplitOptions.None;  /* Standart Zerschneiden an diesem Punkt und ein Array daraus machen */
   	        
   	        }
   	        catch(FormatException e)
            {  
            	   protokoll.erstellen( debuger.block() , proto_gruppe , "Falches Format! Info: " + e.Message , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
            }
            
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
   	     private Debuger debuger = new Debuger();
   	     private Protokoll protokoll = new Protokoll();
	  	   private string  proto_gruppe = "portklasse";
	  	   
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
             
              
   	     	      port_daten.Add(new Port_List( Einstellung.cfy_rohdaten_port , "cfy_rohdaten"   , 1  )  );
   	     	      port_daten.Add(new Port_List( Einstellung.http_port         , "http"           , 500  ) );
   	     	      port_daten.Add(new Port_List( Einstellung.https_port        , "https"          , 500  ) );
   	     	      port_daten.Add(new Port_List( Einstellung.admin_port        , "admin"          , 10  ) );
   	     	      port_daten.Add(new Port_List( Einstellung.extern_port       , "extern"         , 500  ) );
   	     	    
   	     	    liste = port_daten;
   	     }
   	     
   	     public void portlist()
   	     {
   	     	  protokoll.erstellen( debuger.block() , proto_gruppe , "PortListe wird erstellt." , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
   	     	  erstellen();
   	     }
   	    
   }
   
   
   public class Host
   {   
   	   public   string  name;
   	   
   	   private Debuger debuger = new Debuger();
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
   	   
   	   
   	   /* Netzwerk Liste */
   	   public static List<Neztwerk_List> netzwDaten;
   	   
   	   
   	   public void netzwerk_daten_holen()
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
   	      
   	      protokoll.erstellen( debuger.block() , "Neztwerk_List" , "Neztwerk Daten werden bereitgestellt." , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
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
                
                if( adapter.Supports(NetworkInterfaceComponent.IPv4) && adapter.Supports(NetworkInterfaceComponent.IPv6) )ip4_or_ip6 = "IPv4-IPv6"; else {}
                
                 
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
                        {   protokoll.erstellen( debuger.block() , "Neztwerk_List" , "Maximale Lebensdauer Fehler: " + e.Message , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */   }
                        catch(NotImplementedException e)
                        {   protokoll.erstellen( debuger.block() , "Neztwerk_List" , "Maximale Lebensdauer Fehler: Angeforderte Methode oder Operation nicht implementiert! " + e.Message , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */  }

                        try  
                        { offen_lebenszeit = UnicastIPAddressInformation.AddressValidLifetime.ToString(); } /* Verbleibende lebenszeit der IP Adresse in Secunden */
                        catch(SocketException e)
                        {   protokoll.erstellen( debuger.block() , "Neztwerk_List" , "Verbleibende lebenszeit Fehler: " + e.Message , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */  }
                        catch(NotImplementedException e)
                        {   protokoll.erstellen( debuger.block() , "Neztwerk_List" , "Maximale Lebensdauer Fehler: Angeforderte Methode oder Operation nicht implementiert! " + e.Message , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */  }

                
                        if(ip_adresse != "") /* Schutz */
                        {  
                           bool statusDa = false;
                           foreach (Host.Neztwerk_List netz in netzw_daten)
                           {
                           	  if(netz.ip_adresse == ip_adresse)
                           	  { statusDa = true; break; } else { }
                           }
                           string  janein = "";
                           if(statusDa == false) /* IP Adresse war noch nicht da Neue eintragen */
                           { if(aktiv_ip == "ja") janein =  "IP ist Aktiv."; else  janein =  "IP ist inaktiv!";
                           	 protokoll.erstellen( debuger.block() , "Neztwerk_List" , "IP Adresse " + ip_adresse + " auf " + name + " gefunden. "+ janein + ". Es ist eine: " + ip4_or_ip6 + ". Mit Max Lebenszeit: " + max_lebenszeit , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
                           	 netzw_daten.Add(new Neztwerk_List( name , type , status , ip4_or_ip6 , physical_adresse , max_lebenszeit , offen_lebenszeit , ip_adresse , aktiv_ip  ) );  
                           	 Thread.Sleep(250);  } else { }  
                        }
                        else{}	
                 
                }               
                
             }
   	    
   	      }
   	      catch(SocketException e)
          {  protokoll.erstellen( debuger.block() , "Neztwerk_List" , "Fataler Fehler beim holen der Netzwerkdaten: " +  e.Message , debuger.klasse() , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */   }
        
   	      netzwDaten = netzw_daten;
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