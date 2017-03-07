/*
   *************************************************************************************************************
   /      Port Steuerung  - Modul                                                                              /
   /                                                                                                           /
   /                                                                                                           /
   /      Cod by Meiko Eichler                                                                                 /
   /      Copyright by Meiko Eichler                                                                           /
   /                                                                                                           /
   /      Datei erstellt am 01.03.2017                                                                         /
   /                                                                                                           /
   /      Ordner: /port_steuerung/                                                                             /
   /      Datei Name: port_steuerung.cs                                                                        /
   /                                                                                                           /
   /      Beschreibung: Mit diesem Modul werden die Ports auf dem server Überwacht und Verarbeitet.            /
   /                    Es können mehrere Anfragen an einem Port abgegeben werden da jeder Verbindung          /
   /                    in einem Thread läuft so wie jeder Port mit den gefundenen IP Adressen.                /
   /                    - "PortListener" Klasse stellt eine Verbindung zum Port mir IpAdresse her              /
   /                    - "Client_Instanz" Klasse werden alle Anfragen Verarbeitet die an den jeweiligen       /
   /                       Port gestellt werden                                                                /
   /                    - "PortZahler" Klasse Überwacht den Port auf maximale Verbindungen                     /
   /                                                                                                           /
   *************************************************************************************************************  
*/

using System;
using System.Net;
using System.Collections;
using System.Threading;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;
using MEQuery;
using MEClary;


namespace MEPort
{
	   public class PortZahler
	   {    
	   	    public static  List<Zahlwerk_List> zahlwerk = new List<Zahlwerk_List>();
	   	
	   	    public class Zahlwerk_List
	   	    {
	   	    	  public int port;
	   	    	  public int laufend;
	   	    	  
	   	    	  public Zahlwerk_List(){}
	   	    	  
	   	    	  public Zahlwerk_List( int port , int laufend )
	   	    	  {
	   	    	  	 this.port = port;
	   	    	  	 this.laufend = laufend;
	   	    	  }
	   	    	
	   	    }
	   	
	   	    public bool kontrolle(int max_verbindung , int port , bool plus_minus )
	   	    {
	   	    	  bool status_vorhanden = false;
	   	    	  bool status_frei      = false;
	   	    	       
	   	    	  /* Prüfen ob Port noch Verbindungen entgegen nehmen kann oder nicht */
              foreach (Zahlwerk_List zahl in zahlwerk)
              {
              	   if(zahl.port == port) /* wenn Port gefunden wurde hier rein gehen */
              	   {   
              	   	  
              	   	   if(plus_minus == true ) /* Es soll eine neue Verbindungen aufgebaut werden */
              	   	   {   
              	   	   	     if(zahl.laufend == max_verbindung )
              	   	         {}  /* Es ist die Maximale Verbindung auf diesem Port Vorhanden false zurückgeben */
              	   	         else
              	   	         {  status_frei = true;  zahl.laufend++; }  /*  Es sind noch Verbindung übrig zähler hochsetzten und true zurückgeben  */ 
              	   	   }
              	   	   else 
              	   	  	 if(zahl.laufend > 0)zahl.laufend--; else {}  /* Die Verbindung auf dem Port wurde geändert Zähler reduzoeren und Verbindungen wieder Frei geben */
              	   	   
              	   	   
              	   	   
              	   	   status_vorhanden = true; /* Status auf True setzen da Port schon in liste ist */ 
              	   	   break;
              	   }
                   else {}
              }
              
              /* Es wurde noch kein eintrag für Port gemacht neuen hinzufügen mit Zählwerk */
	   	    	  if(status_vorhanden == false)
	   	    	  {
	   	    	  	  zahlwerk.Add( new Zahlwerk_List( port , 1 ) ); /* Zählwerk Inizialiesiern mit 1 ( 1 verbindung ) */
	   	    	  	  status_frei = true;
	   	    	  }else{}
	   	    	   
	   	    	  return status_frei;
	   	    }
	   	
	   }
	   
	   
	   public class TCP_Verwaltung
	   {     
	   	     public static List<TCP_List> liste = new List<TCP_List>();
	   	    
	   	     public class TCP_List
	   	     {
	   	     	    public TcpListener listener;
	   	     	    public string thread_name;
	   	     	    
	   	     	    public TCP_List() {}
	   	     	    
	   	     	    public TCP_List( TcpListener listener, string thread_name )
	   	     	    {
	   	     	    	  this.listener    = listener;
	   	     	    	  this.thread_name = thread_name;
	   	     	    }
	   	     }
	   	     
	   	     public void tcp_eintrag(string ip_adresse,int port,string thread_name)
	   	     {
	   	     	  bool status = false;
	   	     	
	   	        /* Prüfen ob Port noch Verbindungen entgegen nehmen kann oder nicht */
              foreach (TCP_List tcpli in liste)
              {
              	    if(tcpli.thread_name == thread_name)
              	    {
              	       status = true; /* Kein neuer eintrag TCP Listener war schon vorhanden */
              	    }
              }
              
              if(status == false) /* Es war noch kein TCP Listener Vorhanden */ 
              {     
              	    IPAddress ipadress = IPAddress.Parse(ip_adresse);
                    TcpListener listener = new TcpListener ( ipadress , port );
                    liste.Add( new TCP_List( listener , thread_name  ) );
              }
              else {} 
              
           }
	   }
	   
	

     public class PortListener
     {
          /* variable für Steuerung gleichzeitiger Threads deklarieren und für jeden Threads mit true versehen  */
	  	    private volatile bool status = true;
	  	    private volatile string ip_adresse;
	  	    private volatile int port;
	  	    private volatile int max_verbindung;
	  	    private volatile string port_bezeichnung;
	  	    private volatile string thread_name;
	  	    private volatile TcpListener listener;
	  	    public  string  proto_woher = "Portsteuerung";
	  	    public  string  proto_datei = "port_steuerung.cs";
	  	    public volatile string proto_gruppe;
	  	    private Protokol  protokol = new Protokol();
	  	    private static ArrayList clientThread = new ArrayList();  /* Die Liste der laufenden Server-Threads */
    
	  	    public void rennen( )
	  	    {    
	  	    	 proto_gruppe = ip_adresse + ":" + port + "|" + port_bezeichnung;  /* Gruppe für durchlauf defienieren für Protokoll */
	  	    	 
	  	    	 try
	  	       { 
	  	       	 /* verbindung zur Netzwerk Schnittstelle herstellen ( Listener Vorbereiten und in List legen ) */
	  	       	 TCP_Verwaltung tcp_verwaltung = new TCP_Verwaltung();
	  	       	 tcp_verwaltung.tcp_eintrag( ip_adresse , port , thread_name );
	  	       	 
	  	       	
	  	       	 /* Listener aus List holen  für diese Thread zum Starten */
	  	       	 foreach (TCP_Verwaltung.TCP_List tcpli in TCP_Verwaltung.liste)
               {
              	    if(tcpli.thread_name == thread_name )
              	    {
              	       listener = tcpli.listener;
              	       break;
              	    }else {}
               }
	  	       	 
	  	       	 listener.Start (); /* Listener Starten */
	  	    	   
	  	    	   protokol.erstellen( proto_woher , proto_gruppe , "PortListener wurde gestartet." , proto_datei ,"PortListener","rennen()" , false ); /* Protokoll erstellen */
	  	  	     while(status)
	  	  	     {
	  	  	     	   try
	  	  	     	   {
	  	  	     	       protokol.erstellen( proto_woher , proto_gruppe , "Warte auf Eingehende Verbindung." , proto_datei ,"PortListener","rennen()" , false );
	  	  	     	  
	  	  	     	       /* Auf Verbindung Lauschen ob Jemand was möchte - System wartet hier bis sich jemand meldet */   
	  	  	  	         TcpClient client = listener.AcceptTcpClient();    
                       
                       clientThread.Add( new Client_Instanz( client , proto_woher , proto_gruppe , proto_datei , port_bezeichnung , max_verbindung , port ) ); /* übergebe Verbindung um eigenen Thread zu erstellen für Verbindung */
	  	  	  	     }
	  	  	  	     catch(ThreadAbortException e) /* Thread wird von der Main aus sofort abgebrochen - Programm wurde beendet */
	  	  	         {
	  	  	   	          protokol.erstellen( proto_woher , proto_gruppe , "Thread wurde von der Main sofort Beendet da Programm geschlossen wurde ( .Abort() )."  + e.Message , proto_datei ,"PortListener","rennen()" , false );
	  	  	   	          break;
	  	  	         }
	  	  	  	     catch (System.IO.IOException)
                   {
                        protokol.erstellen( proto_woher , proto_gruppe , "Client hat Verbindung beendet.", proto_datei ,"PortListener","rennen()" , true );
                   }
                   catch (SocketException e)
                   {
                        string fehlermeldung = String.Format("SocketException: {0}", e.Message);
                        protokol.erstellen( proto_woher , proto_gruppe , "SocketException wurde gewurfen Verbindung wurde getrennt. Fehler: " + fehlermeldung , proto_datei ,"PortListener","rennen()" , true );
                   }
	  	  	  	    
	  	  	  	     Thread.Sleep(2000); /* 2 Secunden Warden bis Überwachung erneut gestartet wird */
	  	  	     }
	  	  	     
	  	  	     protokol.erstellen( proto_woher , proto_gruppe , "PortListener wurde beendet.", proto_datei ,"PortListener","rennen()" , false );
	  	  	     listener.Stop ();   
	  	  	   }
	  	  	   catch(ThreadAbortException e) /* Thread wird von der Main aus sofort abgebrochen - Programm wurde beendet */
	  	  	   {
	  	  	   	  protokol.erstellen( proto_woher , proto_gruppe , "Thread wurde von der Main sofort Beendet da Programm geschlossen wurde ( .Abort() )."  + e.Message , proto_datei ,"PortListener","rennen()" , false );
	  	  	   	  listener.Stop ();  	       
	  	  	   }
	  	  	   catch (SocketException e)
             {
                string fehlermeldung = String.Format("SocketException: {0}", e.Message);
                protokol.erstellen( proto_woher , proto_gruppe , "SocketException wurde gewurfen Portlistener wurde beendet. Fehler: " + fehlermeldung , proto_datei ,"PortListener","" , true );
             } 
             
	  	  	     
	  	    }
	  	  
	  	    
	  	    public void anhalten()
          {
              status = false;
          }
          
          public void ipport(string adresse,int port_liste, string port_bezeichnung_liste , int max_verbindung_liste , string thread_name_liste )
          {
              ip_adresse       = adresse;
              port             = port_liste;
              port_bezeichnung = port_bezeichnung_liste;
              max_verbindung   = max_verbindung_liste;
              thread_name      = thread_name_liste;
          }
          
         
	  	
	   }
	   
	   public class Client_Instanz
     { 
	  	    private  TcpClient clientInstanz;
	  	    private  string  proto_woher;
	  	    private  string  proto_datei;
	  	    private  string  proto_gruppe;
	  	    private  int byte_lange;            /* wert wie viel in Byte gescrieben wurde Antwort erhäkt man aus dem stream */
	  	    private  string  port_bezeichnung;
	  	    private  Protokol  protokol = new Protokol();
	  	    private  string anmeldung;
	  	    private  string benutzername;
	  	    private  string puffer_string;     /* wird verwendet um den String zu speichern was von der Umwandlung aus Byte zu string kommt */
	  	    private  int max_verbindung;
	  	    private  int port;
	  	         
	  	    private byte[] client_antwort;    /* strem was zum Clienten gesendet wird */
	  	    private byte[] client_eingang;    /* Stream vom Clienten Datenstrom */
	  	    private MemoryStream memstream;
	  	    private int komplett_byte;             /* Int wert  wie Viele Byte für memstream benötigt werden wenn alle daten vorhanden sind */
          private byte[] komplett_byte_stream;   /* Kompletter Strem im Byte Arry liegend -  byte*/
	  	    	   
	  	    private List<PortPuffer> port_stream; /* In diesem List werden die Byte Blöcke gespeichert */
	  	    	   
	  	    public Client_Instanz( TcpClient client  ,string proto_woher , string proto_gruppe ,string proto_datei ,string  port_bezeichnung  , int max_verbindung , int port )
	  	    {    
	  	    	      this.clientInstanz = client;
	  	    	      this.proto_woher   = proto_woher;
	  	    	      this.proto_datei   = proto_datei;
	  	    	      this.proto_gruppe  = proto_gruppe;
	  	    	      this.port_bezeichnung = port_bezeichnung;
	  	    	      this.max_verbindung   = max_verbindung;
	  	    	      this.port             = port;
	  	    	      
	  	    	      protokol.erstellen( proto_woher , proto_gruppe , "Thread für Verbindung wurde hergestellt." , proto_datei ,"Client_Instanz","konstruktor" , false );
	  	    	  	  new Thread( new ThreadStart( rennen_client ) ).Start();
	  	    }
	  	    	  
	  	    	  
	  	    /*  Der eigentliche Thread */
          public void rennen_client()
          {       
          	  PortZahler portzahler = new PortZahler();
          	  bool verbindung_zulassen = portzahler.kontrolle( max_verbindung , port , true );
          	    
              Text     text     = new Text();
        	    AsciiPic asciipic = new AsciiPic();
          	  
          	  /* Speziele regelung bei CFY Rohdaten empfang */
          	  if(this.port_bezeichnung == "cfy_rohdaten" &&  Clary.cfy_port_status != "leer" ) /* Datensatz wird gerade noch bearbeitet Verbindung sollange sperren bis dieser abgearbeitet wurde */
          	    verbindung_zulassen = false;
          	  else {} 
          	   
              if(verbindung_zulassen == true)
       	      {
          	       try
          	       {    
          	          Benutzer benutzer = new Benutzer();
	  	    	          port_stream = new List<PortPuffer>();
	  	    	   
	  	    	          client_eingang = new Byte[256]; /* Länge vom Stream defienieren ( 256 byte lang )  */
                
                      protokol.erstellen( this.proto_woher , this.proto_gruppe , "Verbindung wurde Hergestellt." , this.proto_datei ,"PortListener","rennen_client()" , false );
              	     
                  
	  	  	  	        /* Daten für nächsten durchlauf wieder leeren */
	  	  	  	        anmeldung     = string.Empty;
	  	  	  	        puffer_string = string.Empty;
	  	  	  	    
	  	  	  	        NetworkStream stream = this.clientInstanz.GetStream(); /* Datenstrom vom Clienten holen und im Stream legen */ 
	  	  	  	    
	  	  	  	        byte_lange = 0; /* Stream Wie lang dieser gefühlt ist max 256 byte */
	  	  	  	        while( ( byte_lange = stream.Read( client_eingang , 0, client_eingang.Length)  )  !=0   ) 
                      {   /* Unendlich Schleife - Es wird erst hier durchgelaufen  wenn stream.Read byte daten hat 
                  	      ( Liefert int zahlen für Byte stellen belegt sind  )  bis max 256 Bit wie oben defieniert */
                  	 
                  	     puffer_string = text.text_stream( client_eingang , byte_lange );  /* client_eingang Byte Array  mit angabe der länge was befühlt ist in  UTF8 string Übersetzten */
                         puffer_string = text.steuerzeichen( puffer_string , "alle" ); 
                      
                         /* nur zu testzwecken  alles durchlassen Start */
                            //anmeldung = "ok";
                         /* nur zu testzwecken  alles durchlassen ende */
                      
                         if( text.klein(puffer_string) == "exit" )
                         {
                           	 protokol.erstellen( this.proto_woher , this.proto_gruppe , "Verbindung wurde beendet. ( exit wurde empfangen )" , this.proto_datei ,"Client_Instanz","rennen_client()" , false );
                            	client_antwort = text.byte_stream("Die Verbindung wurde beendet. Have a Nice Day :-) \n"); 
                      	     /* Info an Client senden */ 
                             stream.Write(client_antwort, 0, client_antwort.Length);
                       	     break;
                         }
                         else if( text.klein(puffer_string) == "hallo" && anmeldung != "ok")  /* Verbindung wird mit "hallo" inisialiesiert  Vorher reagiert Port auf Datenstrom nicht */
                         {
                      	     protokol.erstellen( this.proto_woher , this.proto_gruppe , "Benutzer wird aufgefordert Name einzugeben." ,  this.proto_datei ,"Client_Instanz","rennen_client()" , false );
                      	     client_antwort = text.byte_stream("Hallo Benutzer Bitte gebe deinen Namen ein.\n");
                      	     anmeldung = "name";
                      	     /* Info an Client senden */ 
                             stream.Write( client_antwort , 0 , client_antwort.Length);
                              
                         }
                         else if(anmeldung ==  "name") /* Dann wird Benutzername geprüft */
                         {
                      	     if( benutzer.liste( text.klein( puffer_string ) , "" , "name" ) )
                      	     {
                      	 	        benutzername =  puffer_string;
                      	 	        protokol.erstellen( this.proto_woher , this.proto_gruppe , "Benutzer wird aufgefordert Name einzugeben." ,  this.proto_datei ,"Client_Instanz","rennen_client()" , false );
                      	 	        client_antwort = text.byte_stream("Bitte gebe dein Password ein.\n");
                      	 	        anmeldung    =  "password";
                      	     } 
                      	     else
                      	     {    protokol.erstellen( this.proto_woher , this.proto_gruppe , "Benutzer "+ benutzername +" wurde nicht gefunden." , this.proto_datei ,"Client_Instanz","rennen_client()" , false );
                      	 	        client_antwort = text.byte_stream("Benutzername wurde nicht gefunden Bitte Neu eingeben!\n");  }
                      	 
                             /* Info an Client senden */ 
                             stream.Write(client_antwort, 0, client_antwort.Length);
                         }
                         else if(anmeldung ==  "password") /* Wenn Benutzername OK ist Wird nach Password geprüft */
                         {
                      	     if( benutzer.liste( text.klein(benutzername) , text.klein(puffer_string) )  )
                      	     {
                      	 	        protokol.erstellen( this.proto_woher , this.proto_gruppe , "Benutzer "+ benutzername +" wurde erfolgreich angemeldet." , this.proto_datei ,"Client_Instanz","rennen_client()" , false );
                      	 	        client_antwort = text.byte_stream("Willkommen im NOC Portal Backend. Ich warte jetzt auf Daten.\n");
                      	 	        anmeldung =  "ok";
                      	     } 
                      	     else
                      	     {    protokol.erstellen( this.proto_woher , this.proto_gruppe , "Benutzer "+ benutzername +" hat Fehlerhaftes Password eingetragen." , this.proto_datei ,"Client_Instanz","rennen_client()" , false );
                      	          client_antwort = text.byte_stream("Dein Password war Fehlerhaft. Bitte Neu eingeben!\n");  }
                      	   
                      	      /* Info an Client senden */ 
                              stream.Write(client_antwort, 0, client_antwort.Length);
                      	 }
                         else if( anmeldung == "ok") /* Anmeldung war OK Datenstrom empfangen und in MSStream legen als bitstrom */
                         { 
                      	     if(this.port_bezeichnung == "cfy_rohdaten")
                      	     {   /* CFY status nur ändern wenn Mit Port für CFY_Rohdaten gesprochen wird */
                      	      	 Clary.cfy_port_status = "empfange"; /* Clary Status auf empfangn setzen und Sperren zur weiterverarbeitung */ 
                      	     }
                      	     else { }
                      	   
                      	    /*  Empfangene byte aus Stream in List legen und Sammeln bis zum ende des Emfangs */
                      	     byte[] daten_wei = (byte[]) client_eingang.Clone();  /* Byte Array Klonen für Speicherung im List Objekt */
                      	     port_stream.Add(new PortPuffer( daten_wei , byte_lange  ) );  /*  geklontes Byte in List legen */
                          
                         }else {}
                      
	  	  	  	        }	
	  	  	  	    
	  	  	  	       this.clientInstanz.Close(); /* Verbindung wurde getrennt */
	  	  	  	       protokol.erstellen( this.proto_woher , this.proto_gruppe , "Verbindung wurde beendet." , this.proto_datei ,"Client_Instanz","rennen_client()" , false );
	  	  	  	    
	  	  	  	    
	  	  	          /*  Datenstrom Bauen und in einem String legen -- Start -- */
	  	  	  	       komplett_byte = 0;
	  	  	  	       foreach (PortPuffer byte_daten in port_stream)  /* Byte stream Länge ermitteln */
                     {    komplett_byte += byte_daten.bitpuffer.Length;   } 
                  
	  	  	  	       memstream = new MemoryStream(new byte[komplett_byte], 0, komplett_byte , true, true);
	  	  	  	       foreach (PortPuffer byte_daten in port_stream)   /* Byte Pakete aus List holen und im MemoryStream zusammenfügen */ 
                     {   memstream.Write(byte_daten.bitpuffer, 0, byte_daten.bytelange);  }  
                  
                     komplett_byte_stream = memstream.GetBuffer(); /* Byte vom stream zusammenfügen und  in ein Gesamtes Array Byte legen  */
                  
                     puffer_string = string.Empty;
                     puffer_string = text.text_stream( komplett_byte_stream , komplett_byte_stream.Length );  /* client_eingang Byte Array  mit angabe der länge was befühlt ist in  UTF8 string Übersetzten */
                     puffer_string = text.steuerzeichen( puffer_string , "alle" ); 
                    /*  Datenstrom Bauen und in einem String legen -- Ende -- */  
                 
                    
	  	  	  	       if(this.port_bezeichnung == "cfy_rohdaten")
                     {   /* CFY status nur ändern wenn Mit Port für CFY_Rohdaten gesprochen wird */
                           
                          Clary clary = new Clary();
                          clary.rohdaten(puffer_string , this.proto_gruppe );
                     }
                     else {}
                     
                     
                   }
                   catch (System.IO.IOException)
                   {  protokol.erstellen( this.proto_woher , this.proto_gruppe , "Client hat Verbindung beendet.", this.proto_datei ,"Client_Instanz","rennen_client()" , true );  }
                   catch (SocketException e)
                   {
                       string fehlermeldung = String.Format("SocketException: {0}", e.Message);
                       protokol.erstellen( this.proto_woher , this.proto_gruppe , "SocketException wurde gewurfen Verbindung wurde getrennt. Fehler: " + fehlermeldung , this.proto_datei ,"Client_Instanz","rennen_client()" , true );
                   }
                   
                   portzahler.kontrolle( max_verbindung , port , false ); /* Verbindung wieder Frei geben */ 
              }
              else
              { /* Maximale Verbindung für Port wurden festgestellt oder CFY DatenImport wird noch bearbeitet im System */
              	  
                	   NetworkStream stream = this.clientInstanz.GetStream(); /* Datenstrom vom Clienten holen und im Stream legen */ 
                	  
                	   /* Speziele regelung für CFY Rohdaten empfangen */
                     if(this.port_bezeichnung == "cfy_rohdaten" &&  Clary.cfy_port_status != "leer" ) 
          	         {     protokol.erstellen( this.proto_woher , this.proto_gruppe , "CFY Rohdaten werden gerade im System Verarbeitet. Neue Verbindung wurde abgelehnt." , this.proto_datei ,"Client_Instanz","rennen_client()" , false );
                           client_antwort = text.byte_stream(" Achutung CFY Rohdaten Import wird gerade gerade im System verarbeitet. \n Neue Verbindung wurde abgebrochen. \n Bitte versuchen Sie es später noch einmal. \n");   }
                     else
                     {     protokol.erstellen( this.proto_woher , this.proto_gruppe , "Verbindung wurde abgebrochen Maximale Verbindungen ist erreicht." , this.proto_datei ,"Client_Instanz","rennen_client()" , false );
                           client_antwort = text.byte_stream(" Achtung es sind keine Verbindungen mehr Frei! Es wurde abgebrochen! \n Bitte versuchen Sie es später noch einmal. \n");   }
                      	   
                     /* Info an Client senden */ 
                     stream.Write(client_antwort, 0, client_antwort.Length);
                     
                     this.clientInstanz.Close(); /* Verbindung wurde getrennt */
              }
          }
	  	         
	  	         
	  	    public class PortPuffer
	  	    {   /* Klasse Puffert die empfangen Daten vom Clienten für weitere bearbeitung  ( List vorlage ) */
	  	    	
	  	    	   public volatile byte[] bitpuffer;
	  	    	   public volatile int bytelange;
	  	    	   
	  	    	   public PortPuffer() {}
	  	    	   
	  	    	   public  PortPuffer( byte[] bitpuffer , int bytelange )
	  	    	   {
	  	    	   	   this.bitpuffer = bitpuffer;
	  	    	   	   this.bytelange = bytelange;
	  	    	   }
	  	    	
	  	    }
	  	         
	   }

}