/*
   *************************************************************************************************************
   /      NOC Portal Backend Hauptdatei                                                                        /
   /                                                                                                           /
   /                                                                                                           /
   /      Cod by Meiko Eichler                                                                                 /
   /      Copyright by Meiko Eichler                                                                           /
   /      Handy: 0163 7378481                                                                                  /
   /      Email: Meiko@Somba.de                                                                                /
   /                                                                                                           /
   /      Datei erstellt am 01.03.2017                                                                         /
   /                                                                                                           /
   /      Ordner: /                                                                                            /
   /      Datei Name: noc_main.cs                                                                              /
   /                                                                                                           /
   /      Beschreibung: In der Hauptdate startet und endet das kommplete Backend.                              /
   /                    Hier werden die gesamten Haupt-Threads gestartet ( Module )  und Kontroliert.          /
   /                    - "NocBackend" Klasse startet die Main und die Threads werden dort gestartet           /
   /                    - "NocRun" Klasse steuert die gesamten Threads und Programm bis zum finalen ende       /
   /                                                                                                           /
   *************************************************************************************************************  
*/


using System;
using MEQuery;
using MEClary;
using MEPort;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MySQL;



namespace NOCPortal
{  
	  public class NocBackend
	  {  
	  	  /* Die Hauptfunktion / Main  vom backend NOC Portal */
        public static void Main()
        {   
        	  string version   = "1.0";
        	  Einstellung  einstellung = new Einstellung();
        	  bool status_einstellung  =  einstellung.laden();
        	  
        	  string  proto_woher  = "NOC_Backend_Main";
	  	      string  proto_datei  = "/noc_main.cs";
	  	      string  proto_klasse = "NocBackend";
	  	      string  proto_gruppe = "main";
	  	      Protokoll protokoll = new Protokoll();
	  	      
        	  AsciiPic asciipic = new AsciiPic();
        	  
        	  protokoll.erstellen( proto_woher , proto_gruppe , "Noc Portal Backend Version "+ version + " wird gestartet.  Auf Rechner: " + Environment.MachineName  , proto_datei ,proto_klasse,"Main()" , false ); /* Protokoll Schreibe */
        	  Console.WriteLine( "----------------------------------------------------- \n"); 	
        	  Console.WriteLine( "-- Willkommen im NOC Portal Backend Version "+ version +  " -- \n"); 
        	  Console.WriteLine( "----------------------------------------------------- \n\n"); 	
            
            if(status_einstellung == true)
            {
        	     Console.WriteLine( " Tastenkombination:  \n");
        	     Console.WriteLine( " -> AltGr + C = Clary Thread ausschalten zur DB. ( es erfolgt kein neuer Durchlauf Aktueller wird noch abgearbeitet. )  \n");
        	     Console.WriteLine( " -> AltGr + B = Programm beenden.  \n\n");
        	  
        	     main_run();
        	  }
        	  else
        	  {
        	  	      protokoll.erstellen( proto_woher , proto_gruppe , "Config Datei vom Server war fehlerhaft. Programm wurde abgebrochen." , proto_datei ,proto_klasse,"Main()" , true ); /* Protokoll Schreibe */
        	  	      Console.BackgroundColor  =  ConsoleColor.Magenta; /* Hintergrund Farbe zuweisen */
                    Console.ForegroundColor  =  ConsoleColor.Black; /* Text Frabe zuweisen */
                    Console.WriteLine("\n\n Fehler in der Config datei! Programm wurde abgebrochen. \n");
                    Console.ResetColor(); /* auf Standart Farbzuweisung gehen zurückgehen */
        	  }
        	  
        	  Console.WriteLine( "\n\n  Bitte nicht Ausschalten sichere Daten! Danke. \n\n"); 	
        	  
        	  protokoll.erstellen( proto_woher , proto_gruppe , "Noc Portal Backend Version "+ version + " wurde beendet.  Auf Rechner: " + Environment.MachineName  , proto_datei ,proto_klasse,"Main()" , false ); /* Protokoll Schreibe */
        	  Thread protokoll_speicherung     = new Thread( protokoll.rennen ); 
        	  protokoll_speicherung.Name       = "Ende_vom_Protokoll_Schreiben"; /* Thread Namen geben */
        	  protokoll_speicherung.Priority   = ThreadPriority.Highest; /* Höchste Priorität vergeben was Thread hat */
        	  protokoll_speicherung.IsBackground = true;
        	  protokoll_speicherung.Start();
        	  protokoll.stop();
        	  protokoll_speicherung.Join(); /* Warte bis Protokolle gespeichert wurden */
        	  Console.WriteLine( "\n----------------------------------------------------- \n"); 	
        	  Console.WriteLine(   "--------  Daten wurden gesichert.-------------------- \n");
        	  Console.WriteLine(   "-------- NOC Portal Backend wurde Beendet!  --------- \n"); 
        	  Console.WriteLine(   "----------------------------------------------------- \n\n"); 	
        	  Console.WriteLine( asciipic.computer()	);
        	  
        } 
        
       
        /* Globales Portlistener und thread  Array Inizialiesieren ( Direkt über Klasse Ansprechbar ) */
        public static PortListener[] portlistener;
        public static Thread[]       noc_thread;
        public static MySQLDatenImport mysqldatenimport = new MySQLDatenImport();
        
        public static void main_run()
        {   
        	  string  proto_woher  = "NOC_Backend_Main";
	  	      string  proto_datei  = "/noc_main.cs";
	  	      string  proto_klasse = "NocBackend";
	  	      string  proto_gruppe = "main";
	  	      Protokoll protokoll = new Protokoll(); 
        	
        	  /* Hier werden die gesamten Thread gstartet was benötigt werden im Backend */
        	   Host host       = new Host();
        	   NocRun noc_run  = new NocRun();
        	   
        	   PortZuweisung portzuweisung       = new PortZuweisung();
        	   portzuweisung.portlist(); /* Port Liste erstellen */ 
        	   
        	   List<Host.Neztwerk_List> netzw_daten = host.netzwerk_daten(); /* Netzwerk Daten  ( Schnittstellen holen ) */
        	   
        	   int port_anzahl = PortZuweisung.liste.Count; /* Anzahl ermitteln wieviel Port es gibt */
        	   
        	   int th_anzahl = 0; /* Variable spiegelt die Anzahl der Threads wieder */
        	   if(Einstellung.ip_adresse == "alle")
        	   { /* jetzt werden alle gefundenen IP adressen genommen was im System vorhanden waren */
        	      th_anzahl = ( netzw_daten.Count + 3 ) * port_anzahl; /* Wieviel Thread erzeugt werden sollen ermitteln plus standart Threads die immer aktiviert werden * der Port Adressen die benötigt werden  */
        	   }
        	   else /* Es wurde eine IP Adresse über die Config Datei eingetragen */
        	    th_anzahl = ( 2 + 3 ) * port_anzahl;  /* Wieviel Thread erzeugt werden sollen ermitteln plus standart Threads die immer aktiviert werden * der Port Adressen die benötigt werden  */
        	     
        	   portlistener  = new PortListener[th_anzahl];
        	   noc_thread    = new Thread[th_anzahl];  /* Threads erstellen */
        	   
        	   
        	   protokoll.erstellen( proto_woher , proto_gruppe , "Thread werden vorbereitet." , proto_datei ,proto_klasse,"main_run()" , false );  /* Protokoll Schreibe */
        	   
        	   noc_thread[0]          = new Thread( noc_run.rennen); /* Thread Objekt erzeugen  aus Klasse - Standart Thread - */ 
        	   noc_thread[0].Name     = "Hauptfunktion_main_run"; /* Thread Namen geben */
        	   noc_thread[0].Priority = ThreadPriority.Highest; /* Höchste Priorität vergeben was Thread hat */
        	   noc_thread[0].IsBackground = true;   /* Thread läuft im Backround  deklarieren ( System räumt bei nicht beenden des Thread selber diesen auf )  */
        	   noc_thread[1]          = new Thread( mysqldatenimport.rennen ); /* Thread Objekt erzeugen  aus Klasse - Standart Thread - */ 
        	   noc_thread[1].Name     = "CFY_zu_Mysql_Datenimport"; /* Thread Namen geben */
        	   noc_thread[1].Priority = ThreadPriority.BelowNormal; /* eine stufe unter Normale Priorität vergeben was Thread hat */
        	   noc_thread[1].IsBackground = true;   /* Thread läuft im Backround  deklarieren ( System räumt bei nicht beenden des Thread selber diesen auf )  */
        	   noc_thread[2]          = new Thread(  protokoll.rennen ); /* Thread Objekt erzeugen  aus Klasse - Standart Thread - */ 
        	   noc_thread[2].Name     = "Protokoll_in_Daten_Schreiben"; /* Thread Namen geben */
        	   noc_thread[2].Priority = ThreadPriority.Highest; /* Höchste Priorität vergeben was Thread hat */
        	   noc_thread[2].IsBackground = true;   /* Thread läuft im Backround  deklarieren ( System räumt bei nicht beenden des Thread selber diesen auf )  */
        	   
        	   int port_i = 0;
        	   int pos = 3;
             string thread_name = string.Empty;
             
             /* Port Schleife */
             foreach (PortZuweisung.Port_List port_liste in PortZuweisung.liste)
             {
             	  if(Einstellung.ip_adresse == "alle")
             	  {  /* Alle gefundene IP Adressen vom System nehmen */
             	  	
             	  	 /* Aktive Verbindungen lauschen lassen  - Start - */
       	           foreach (Host.Neztwerk_List netz in netzw_daten)
                   {  
             	       if(netz.aktiv_ip == "ja")
                     { 
                     	   thread_name = "PortListener_" + netz.ip_adresse +":"+ port_liste.port;  /* Thread Namen zuweisen */
                 	       portlistener[port_i] = new PortListener();
                 	       portlistener[port_i].ipport(netz.ip_adresse , port_liste.port , port_liste.bezeichnung , port_liste.max_verbindung , thread_name  );
                 	       noc_thread[pos] = new Thread( portlistener[port_i].rennen);
                 	       noc_thread[pos].Name = thread_name;
                 	       noc_thread[pos].Priority = ThreadPriority.AboveNormal; /* eine Stufe unter Höchste Priorität vergeben was Thread hat */
                 	       noc_thread[pos].IsBackground = true;   /* Thread läuft im Backround  deklarieren ( System räumt bei nicht beenden des Thread selber diesen auf )  */
                 	       pos++;
                 	       port_i++;
                     }
                     else {}
                  }
        	        /* Aktive Verbindungen lauschen lassen  - Ende - */
        	        
        	      }
        	      else
        	      {
        	         	     thread_name = "PortListener_" + Einstellung.ip_adresse +":"+ port_liste.port;  /* Thread Namen zuweisen */
                 	       portlistener[port_i] = new PortListener();
                 	       portlistener[port_i].ipport(Einstellung.ip_adresse , port_liste.port , port_liste.bezeichnung , port_liste.max_verbindung , thread_name  );
                 	       noc_thread[pos] = new Thread( portlistener[port_i].rennen);
                 	       noc_thread[pos].Name = thread_name;
                 	       noc_thread[pos].Priority = ThreadPriority.AboveNormal; /* eine Stufe unter Höchste Priorität vergeben was Thread hat */
                 	       noc_thread[pos].IsBackground = true;   /* Thread läuft im Backround  deklarieren ( System räumt bei nicht beenden des Thread selber diesen auf )  */
                 	       pos++;
                 	       port_i++;
        	      }
        	      
        	   }
        	 
        	   
        	   protokoll.erstellen( proto_woher , proto_gruppe , "Thread werden jetzt alle gestartet. Anzahl: " + pos , proto_datei ,proto_klasse,"main_run()" , false );  /* Protokoll Schreibe */
        	           	   
        	   for(int i =0; i < pos;i++)
        	   {
        	      noc_thread[i].Start();   /* Alle Thread starten */ 
        	   }
        	   
        	   protokoll.erstellen( proto_woher , proto_gruppe , "Thread werden alle jetzt Überwacht bis diese Beendet werden. ( Join() )" , proto_datei ,proto_klasse,"main_run()" , false );  /* Protokoll Schreibe */
        	   
        	   Thread.Sleep(2000); /* 2 Secunden Warden bis Überwachung erneut gestartet wird */
        	   
        	   for(int i =0; i < pos;i++)
        	   {
        	   	  if(noc_thread[i].IsAlive == false )
        	   	  { /* Fehlerhafte Thread finden und Protokolieren und Info ausgeben */ 
        	   	  	
        	   	  	  Console.BackgroundColor  =  ConsoleColor.Magenta; /* Hintergrund Farbe zuweisen */
                    Console.ForegroundColor  =  ConsoleColor.Black; /* Text Frabe zuweisen */
                    Console.WriteLine("\n\n Thread wure unerwartet Beendet. Name: " + noc_thread[i].Name + "\n");
                    Console.ResetColor(); /* auf Standart Farbzuweisung gehen zurückgehen */
                    protokoll.erstellen( proto_woher , proto_gruppe , "Thread wure unerwartet Beendet. Name: " + noc_thread[i].Name , proto_datei ,proto_klasse,"main_run()" , true );  /* Protokoll Schreibe */
        	      }
        	      else { }
        	   }
        	   
        	   for(int i =0; i < pos;i++)
        	   {
        	   	  if(noc_thread[i].IsAlive)
        	   	  { /* Nur Aktive überwachen wo alles OK war */ 
        	   	     noc_thread[i].Join(); /* Prüft ob Thread beendet wurden wenn nicht Wartet System bis Threads ALLE beendet wurden  */ 
        	      }
        	      else { }
        	   }
        	   
        }
        
        
    }
       
	  public class NocRun
	  {
	  	  /* variable für Steuerung gleichzeitiger Threads deklarieren und für jeden Threads mit true versehen  */
	  	  private volatile bool status = true;
	  	  private string  proto_woher  = "NOC_Backend_Main";
	  	  private string  proto_datei  = "/noc_main.cs";
	  	  private string  proto_klasse = "NocRun";
	  	  private string  proto_gruppe = "main";
	  	  Protokoll protokoll = new Protokoll();
	  	  
	  	  public void rennen()
	  	  {    
	  	  	  EventObjekt eventobjekt = new EventObjekt(); /* Eventobjekt erstellen */
	  	  	  ConsoleKeyInfo taste    = new ConsoleKeyInfo(); /* Tastaturabfrage Objekt erstellen */
	  	      
	  	      protokoll.erstellen( proto_woher , proto_gruppe , "Haupt Thread wird gestartet." , proto_datei ,proto_klasse,"rennen()" , false );  /* Protokoll Schreibe */
	  	      while(status)
	  	  	  {   
	  	  	  	  /* Auswerung zurücksetzten und auf neue Eingabe lauschen von Tastatur */
	  	  	  	  taste = Console.ReadKey(true);
	  	  	  	 
	  	  	  	  if( eventobjekt.tastatur(taste,"altgr+b") )
	  	  	  	  { /* Programm beenden  Alles Stoppen */
	  	  	  	  	  
	  	  	  	  	  protokoll.erstellen( proto_woher , proto_gruppe , "Benutzer Stoppt das komplette Backend mit ( Abort() )." , proto_datei ,proto_klasse,"rennen()" , false );  /* Protokoll Schreibe */
	  	  	  	  	  for(int i=1;i< NocBackend.noc_thread.Length;i++)
	  	  	  	  	  {  
	  	  	  	  	  	  try
	  	  	  	  	  	  {
	  	  	  	  	  	      if(NocBackend.noc_thread[i].IsAlive)
	  	  	  	  	  	      {  
	  	  	  	  	  	      	  foreach (TCP_Verwaltung.TCP_List tcpli in TCP_Verwaltung.liste)
                                {
              	                       if(tcpli.thread_name == NocBackend.noc_thread[i].Name ) /* TCP Listener suchen anhand des Thred Namen */
              	                       {
              	                          tcpli.listener.Stop();  /* TCP Listener Stoppen */
              	                          break;
              	                       }else {}
                                }
                                
                                protokoll.erstellen( proto_woher , proto_gruppe , "Stoppe Thread: " + NocBackend.noc_thread[i].Name , proto_datei ,proto_klasse,"rennen()" , false );  /* Protokoll Schreibe */
	  	  	  	  	  	      	  NocBackend.noc_thread[i].Abort();
	  	  	  	  	  	      	  NocBackend.noc_thread[i].Join(); /* Warten bis Thread beendet wurde */
	  	  	  	  	  	      }
	  	  	  	  	  	      else {  } 
	  	  	  	  	  	      
	  	  	  	  	  	  }
	  	  	  	  	  	  catch
	  	  	  	  	  	  {  } 
	  	  	  	  	  	   
	  	  	  	  	  	  
	  	  	  	  	  }
	  	  	  	  	  protokoll.erstellen( proto_woher , proto_gruppe , "Stoppe HauptThread." , proto_datei ,proto_klasse,"rennen()" , false );  /* Protokoll Schreibe */
	  	  	  	  	  this.anhalten();      /* HauptThread  ( Main ) anhalten -- erst zum schluss ;-)  */ 
	  	  	  	  	  
	  	  	  	  }
	  	  	  	  else if( eventobjekt.tastatur(taste,"altgr+c") ) /* MYSQL Datenimport Thread Beenden  */
	  	  	  	  {
	  	  	  	     try{ NocBackend.mysqldatenimport.anhalten(); } catch { }  	
	  	  	  	     protokoll.erstellen( proto_woher , proto_gruppe , "Thread - Datenimport zu MYSQL wurde gestoppt." , proto_datei ,proto_klasse,"rennen()" , false );  /* Protokoll Schreibe */
	  	  	  	  }
	  	  	  	  else{}
	  	  	  	 
	  	  	  }
	  	  	  
	  	  }
	  	  
	  	  
	  	  public void anhalten()
        {
           status = false;
        }
	  	
	  }
}