using System;
using MEQery;
using MEClary;
using MEPort;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MySQL;



namespace NOCPortal
{  
	  public class NocBackend
	  {  
	  	  
	      /* Die Hauptfunktion / Main  vom backend NOC Portal */
        public static void Main ()
        {   
        	
        	  AsciiPic asciipic = new AsciiPic();
        	  
        	  Console.WriteLine( "----------------------------------------------------- \n"); 	
        	  Console.WriteLine( "--------Willkommen im NOC Portal Backend  ----------- \n"); 
        	  Console.WriteLine( "----------------------------------------------------- \n\n"); 	
        	  Console.WriteLine( " Bitte drücken Sie eine Taste um das NOC Portal Backend zu starten. \n\n"); 	
        	  Console.ReadKey();
        	  
        	  main_run();
        	  
        	  Console.WriteLine( "\n----------------------------------------------------- \n"); 	
        	  Console.WriteLine(   "-------- NOC Portal Backend wurde Beendet!  --------- \n"); 
        	  Console.WriteLine(   "----------------------------------------------------- \n\n"); 	
        	  Console.WriteLine( asciipic.computer()	);
        } 
        
       
        /* Globales Portlistener und thread  Array Inizialiesieren ( Direkt über Klasse Ansprechbar ) */
        public static PortListener[] portlistener;
        public static Thread[]       noc_thread;
        public static MySQLDatenImport mysqldatenimport = new MySQLDatenImport();
        
        public static void main_run()
        {    /* Hier werden die gesamten Thread gstartet was benötigt werden im Backend */
        	   Host host       = new Host();
        	   NocRun noc_run  = new NocRun();
        	   
        	   PortZuweisung portzuweisung       = new PortZuweisung();
        	   portzuweisung.portlist();
        	   
        	   List<Host.Neztwerk_List> netzw_daten = host.netzwerk_daten(); /* Netzwerk Daten  ( Schnittstellen holen ) */
        	   int port_anzahl = PortZuweisung.liste.Count;
        	   int th_anzahl = ( netzw_daten.Count + 2 ) * port_anzahl; /* Wieviel Thread erzeugt werden sollen ermitteln plus standart Threads die immer aktiviert werden * der Port Adressen die benötigt werden  */
        	   portlistener  = new PortListener[th_anzahl];
        	   noc_thread    = new Thread[th_anzahl]; 
        	   
        	   noc_thread[0] = new Thread( noc_run.rennen); /* Thread Objekt erzeugen  aus Klasse - Standart Thread - */ 
        	   noc_thread[1] = new Thread( mysqldatenimport.rennen ); /* Thread Objekt erzeugen  aus Klasse - Standart Thread - */ 
        	   
        	   int port_i = 0;
        	   int pos = 2;

             /* Port Schleife */
             foreach (PortZuweisung.Port_List port_liste in PortZuweisung.liste)
             {
             	    /* Aktive Verbindungen lauschen lassen  - Start - */
       	          foreach (Host.Neztwerk_List netz in netzw_daten)
                  {  
             	       if(netz.aktiv_ip == "ja")
                     { 
                 	       portlistener[port_i] = new PortListener();
                 	       portlistener[port_i].ipport(netz.ip_adresse , port_liste.port , port_liste.bezeichnung  );
                 	       noc_thread[pos] = new Thread( portlistener[port_i].rennen);
                 	       pos++;
                 	       port_i++;
                     }
                     else {}
                  }
        	        /* Aktive Verbindungen lauschen lassen  - Ende - */
        	   }
        	 
        	           	   
        	   for(int i =0; i < pos;i++)
        	   {
        	      noc_thread[i].Start();   /* Alle Thread starten */ 
        	   }
        	   
        	      
        	   for(int i =0; i < pos;i++)
        	   {
        	      noc_thread[i].Join(); /* Prüft ob Thread beendet wurden wenn nicht Wartet System bis Threads ALLE beendet wurden  */ 
        	   }
        	   
        }
        
        
    }
       
	  public class NocRun
	  {
	  	  /* variable für Steuerung gleichzeitiger Threads deklarieren und für jeden Threads mit true versehen  */
	  	  private volatile bool status = true;
	  	  
	  	  public void rennen()
	  	  {    
	  	  	  EventObjekt eventobjekt = new EventObjekt(); /* Eventobjekt erstellen */
	  	  	  ConsoleKeyInfo taste    = new ConsoleKeyInfo(); /* Tastaturabfrage Objekt erstellen */
	  	  	  
	  	  	  Console.WriteLine ( "-------- NOC Portal Backend  Hauptfunktion Main  wurde gestartet   ---------\n"  );
	  	  	  while(status)
	  	  	  {   
	  	  	  	  /* Auswerung zurücksetzten und auf neue Eingabe lauschen von Tastatur */
	  	  	  	  taste = Console.ReadKey(true);
	  	  	  	 
	  	  	  	  if( eventobjekt.tastatur(taste,"altgr+s") )
	  	  	  	  { /* Programm beenden  Alles Stoppen */
	  	  	  	  	 
	  	  	  	  	  this.anhalten();      /* HauptThread  ( Main ) anhalten */ 
	  	  	  	  	  for(int i=0;i<NocBackend.portlistener.Length;i++)
	  	  	  	  	  {  try{ NocBackend.portlistener[i].anhalten();  } catch { }  }  /* Allen Portlistner den Befehl geben anzuhalten */ 
	  	  	  	  	  Console.WriteLine ( "-------- NOC Portal wurde beendet! ---------\n");
	  	  	  	  }
	  	  	  	  else if( eventobjekt.tastatur(taste,"altgr+c") ) /* PortListener 5 Stoppen */
	  	  	  	  {
	  	  	  	     try{ NocBackend.mysqldatenimport.anhalten(); } catch { }  	
	  	  	  	  }
	  	  	  	  else if( eventobjekt.tastatur(taste,"altgr+1") ) /* PortListener 1 Stoppen */
	  	  	  	  {
	  	  	  	     try{ NocBackend.portlistener[0].anhalten(); } catch { }
	  	  	  	  }
	  	  	  	  else if( eventobjekt.tastatur(taste,"altgr+2") ) /* PortListener 2 Stoppen */
	  	  	  	  {
	  	  	  	     try{ NocBackend.portlistener[1].anhalten(); } catch { } 
	  	  	  	  }
	  	  	  	  else if( eventobjekt.tastatur(taste,"altgr+3") ) /* PortListener 3 Stoppen */
	  	  	  	  {
	  	  	  	    try{ NocBackend.portlistener[2].anhalten(); } catch { }  	
	  	  	  	  }
	  	  	  	  else if( eventobjekt.tastatur(taste,"altgr+4") ) /* PortListener 4 Stoppen */
	  	  	  	  {
	  	  	  	     try{ NocBackend.portlistener[3].anhalten(); } catch { }  	
	  	  	  	  }
	  	  	  	  else if( eventobjekt.tastatur(taste,"altgr+5") ) /* PortListener 5 Stoppen */
	  	  	  	  {
	  	  	  	     try{ NocBackend.portlistener[4].anhalten(); } catch { }  	
	  	  	  	  }
	  	  	  	  else{}
	  	  	  	 
	  	  	  }
	  	  	  Console.WriteLine ( "-------- NOC Portal Backend  Hauptfunktion Main wurde beendet.  ---------\n");
	  	  }
	  	  
	  	  
	  	  public void anhalten()
        {
           status = false;
        }
	  	
	  }
}