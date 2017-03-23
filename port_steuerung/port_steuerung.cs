/*
   *************************************************************************************************************
   /      Port Steuerung  - Modul                                                                              /
   /                                                                                                           /
   /                                                                                                           /
   /      Cod by Meiko Eichler                                                                                 /
   /      Copyright by Meiko Eichler                                                                           /
   /      Handy: 0163 7378481                                                                                  /
   /      Email: Meiko@Somba.de                                                                                /
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
	   	    
          private Protokoll  protokoll = new Protokoll();
          private Debuger   debuger = new Debuger();
	   	    private string    proto_woher  = "Server Portsteuerung";
	  	    private string    proto_klasse = "PortZahler";
	  	    
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
              	   	   	     protokoll.erstellen( proto_woher , "Port_" + port  , "Prüfen ob Port: " + port +" noch freie Plätze hat.",  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
              	   	   	     if(zahl.laufend == max_verbindung )
              	   	         {  /* Es ist die Maximale Verbindung auf diesem Port Vorhanden false zurückgeben */
              	   	            protokoll.erstellen( proto_woher , "Port_" + port  , "Keine Verbindung auf Port "+ port +" mehr Verfügbar Max: " + max_verbindung +" erreicht.",  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
              	   	         }  
              	   	         else
              	   	         { status_frei = true;  zahl.laufend++;   }  /*  Es sind noch Verbindung übrig zähler hochsetzten und true zurückgeben  */ 
              	   	   }
              	   	   else 
              	   	  	 if(zahl.laufend > 0)zahl.laufend--; else {}  /* Die Verbindung auf dem Port wurde geändert Zähler reduzoeren und Verbindungen wieder Frei geben */
              	   	   
              	   	   if(status_frei == true)
              	   	     protokoll.erstellen( proto_woher , "Port_" + port  , "Port Status. Max: " + max_verbindung +" Vergeben Pos.: " + zahl.laufend ,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
              	   	   else { }
              	   	  
              	   	   status_vorhanden = true; /* Status auf True setzen da Port schon in liste ist */ 
              	   	   break;
              	   }
                   else {}
              }
              
              /* Es wurde noch kein eintrag für Port gemacht neuen hinzufügen mit Zählwerk */
	   	    	  if(status_vorhanden == false)
	   	    	  {
	   	    	  	  zahlwerk.Add( new Zahlwerk_List( port , 1 ) ); /* Zählwerk Inizialiesiern mit 1 ( 1 verbindung ) */
	   	    	  	  protokoll.erstellen( proto_woher , "Port_" + port  , "Port Status. Max: " + max_verbindung +" Vergeben Pos.: 1" ,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
	   	    	  	  status_frei = true;
	   	    	  }else{}
	   	    	   
	   	    	  return status_frei;
	   	    }
	   	
	   }
	   
	   
	   public class TCP_Verwaltung
	   {     
	   	     public static List<TCP_List> liste = new List<TCP_List>();
	   	     
	   	     private Protokoll protokoll = new Protokoll();
	   	     private Debuger   debuger = new Debuger();
	   	     private string  proto_woher  = "Server Portsteuerung";
	  	     private string  proto_klasse = "TCP_Verwaltung";
	  	     
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
	   	     
	   	     public void tcp_eintrag(string ip_adresse,int port,string thread_name , string proto_gruppe)
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
              	 try
              	 {
              	    IPAddress ipadress = IPAddress.Parse(ip_adresse);
                    TcpListener listener = new TcpListener ( ipadress , port );
                    liste.Add( new TCP_List( listener , thread_name  ) );
                    protokoll.erstellen( proto_woher , proto_gruppe  , "TCP Verbindung vorbereitet auf: " + ipadress +":"+ port ,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
                 }
                 catch(ThreadAbortException) /* Thread wird von der Main aus sofort abgebrochen - Programm wurde beendet */
	  	  	       {   protokoll.erstellen( proto_woher , proto_gruppe , "Thread wurde von der Main sofort Beendet. ( .Abort() )"  ,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */  }
                 catch(System.FormatException e)
                 {
                     protokoll.erstellen( proto_woher , proto_gruppe  , "IP Adresse ist Fehlerhaft. FormatException wurde gewurfen mit Fehler: " + e.Message ,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true );  /* Protokoll erstellen */   
                 }
                 catch(NullReferenceException e)
                 {
                 	  protokoll.erstellen( proto_woher , proto_gruppe  , "NullReferenceException wurde gewurfen. Fehler: " + e.Message ,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
                 }
                 catch(SocketException e)
                 {
                    protokoll.erstellen( proto_woher , proto_gruppe  , "SocketException wurde gewurfen. Fehler: " + e.Message ,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
                 } 
                 catch(ArgumentOutOfRangeException e)
                 {
                   	protokoll.erstellen( proto_woher , proto_gruppe  , "Port ist Fehlerhaft. ArgumentOutOfRangeException wurde gewurfen mit Fehler: " + e.Message + "<br /> Tip! MaxPort ist: " + IPEndPoint.MaxPort ,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
                 }
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
	  	    
	  	    private Protokoll  protokoll = new Protokoll();
	  	    private volatile Debuger debuger = new Debuger();
	  	    public  string  proto_woher = "Server Portsteuerung";
	  	    public  string  proto_klasse = "PortListener";
	  	    public volatile string proto_gruppe;
	  	    
	  	    
	  	    
	  	    private static ArrayList clientThread = new ArrayList();  /* Die Liste der laufenden Server-Threads */
    
         
         
    
          
	  	    public void rennen()
	  	    {    
	  	    	 	    	 
	  	    	 proto_gruppe = ip_adresse + ":" + port + "|" + port_bezeichnung;  /* Gruppe für durchlauf defienieren für Protokoll */
	  	    	 
	  	    	 try
	  	       { 
	  	       	 /* verbindung zur Netzwerk Schnittstelle herstellen ( Listener Vorbereiten und in List legen ) */
	  	       	 TCP_Verwaltung tcp_verwaltung = new TCP_Verwaltung();
	  	       	 tcp_verwaltung.tcp_eintrag( ip_adresse , port , thread_name , proto_gruppe );
	  	       	 
	  	       	
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
	  	    	   
	  	    	   protokoll.erstellen( proto_woher , proto_gruppe , "TCP Verbindung gestartet." , proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
	  	  	     while(status)
	  	  	     {
	  	  	     	   try
	  	  	     	   {
	  	  	     	       protokoll.erstellen( proto_woher , proto_gruppe , "Warte auf Eingehende Verbindung." ,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
	  	  	     	  
	  	  	     	       /* Auf Verbindung Lauschen ob Jemand was möchte - System wartet hier bis sich jemand meldet */   
	  	  	  	         TcpClient client = listener.AcceptTcpClient();    
                      
                       protokoll.erstellen( proto_woher , proto_gruppe , "Antwort vom Client erhalten Übergebe Verbindung an Thread und erstelle neue Verbindung. ( Client_Instanz() )" ,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
                       clientThread.Add( new Client_Instanz( client , proto_woher , proto_gruppe , port_bezeichnung , max_verbindung , port ) ); /* übergebe Verbindung um eigenen Thread zu erstellen für Verbindung */
	  	  	  	     }
	  	  	  	     catch(ThreadAbortException) /* Thread wird von der Main aus sofort abgebrochen - Programm wurde beendet */
	  	  	         {
	  	  	   	          protokoll.erstellen( proto_woher , proto_gruppe , "Thread wurde von der Main sofort Beendet. ( .Abort() )",  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
	  	  	   	          break;
	  	  	         }
	  	  	  	     catch (System.IO.IOException)
                   {
                        protokoll.erstellen( proto_woher , proto_gruppe , "Client hat Verbindung beendet.",   proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
                   }
                   catch (SocketException e)
                   {
                        protokoll.erstellen( proto_woher , proto_gruppe , "SocketException wurde gewurfen Verbindung wurde getrennt. Fehler: " + e.Message,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
                   }
	  	  	  	    
	  	  	  	     Thread.Sleep(2000); /* 2 Secunden Warden bis Überwachung erneut gestartet wird */
	  	  	     }
	  	  	     
	  	  	     protokoll.erstellen( proto_woher , proto_gruppe , "TCP Verbindung wurde beendet.",  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
	  	  	     listener.Stop ();   
	  	  	   }
	  	  	   catch(ThreadAbortException ) /* Thread wird von der Main aus sofort abgebrochen - Programm wurde beendet */
	  	  	   {
	  	  	   	  protokoll.erstellen( proto_woher , proto_gruppe , "Thread wurde von der Main sofort Beendet. ( .Abort() )" ,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
	  	  	   	  listener.Stop ();  	       
	  	  	   }
	  	  	   catch(NullReferenceException e)
	  	  	   {
	  	  	   	  protokoll.erstellen( proto_woher , proto_gruppe , "NullReferenceException wurde gewurfen TCP Verbindung wurde beendet. Fehler: " + e.Message , proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
	  	  	   }
	  	  	   catch (SocketException e)
             {
                protokoll.erstellen( proto_woher , proto_gruppe , "SocketException wurde gewurfen TCP Verbindung wurde beendet. Fehler: " + e.Message ,   proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
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
	  	    
	  	    private  Protokoll  protokoll = new Protokoll();
	  	    private  Debuger debuger = new Debuger();
	  	    private  string  proto_woher;
	  	    private  string  proto_gruppe;
	  	    private  string  proto_klasse = "Client_Instanz";

	  	    private  int byte_lange;            /* wert wie viel in Byte gescrieben wurde Antwort erhäkt man aus dem stream */
	  	    private  string  port_bezeichnung;
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
	  	    
	  	    
	  	    public Client_Instanz( TcpClient client  ,string proto_woher , string proto_gruppe ,string  port_bezeichnung  , int max_verbindung , int port )
	  	    {    
	  	    	      this.clientInstanz = client;
	  	    	      this.proto_woher   = proto_woher;
	  	    	      this.proto_gruppe  = proto_gruppe;
	  	    	      this.port_bezeichnung = port_bezeichnung;
	  	    	      this.max_verbindung   = max_verbindung;
	  	    	      this.port             = port;
	  	    	      
	  	    	      protokoll.erstellen( proto_woher , proto_gruppe , "Übernehme TCP Verbindung und starte Thread. Arbeite Client Anfragen ab." ,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
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
          	          protokoll.erstellen( this.proto_woher , this.proto_gruppe , "TCP Kommunikation wird aufgebaut um Daten vom Client zu empfangen." ,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
          	          
          	          Benutzer benutzer = new Benutzer();
	  	    	          port_stream = new List<PortPuffer>();
	  	    	   
	  	    	          client_eingang = new Byte[256]; /* Länge vom Stream defienieren ( 256 byte lang )  */
                
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
                           	 protokoll.erstellen( this.proto_woher , this.proto_gruppe , "Verbindung wurde beendet. ( exit wurde empfangen )" ,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
                             client_antwort = text.byte_stream("Die Verbindung wurde beendet. Have a Nice Day :-) \n"); 
                      	     /* Info an Client senden */ 
                             stream.Write(client_antwort, 0, client_antwort.Length);
                       	     break;
                         }
                         else if( text.klein(puffer_string) == "hallo" && anmeldung != "ok")  /* Verbindung wird mit "hallo" inisialiesiert  Vorher reagiert Port auf Datenstrom nicht */
                         {
                      	     protokoll.erstellen( this.proto_woher , this.proto_gruppe , "Benutzer wird aufgefordert Name einzugeben." ,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
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
                      	 	        protokoll.erstellen( this.proto_woher , this.proto_gruppe , "Benutzer wird aufgefordert Name einzugeben." ,  this.proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
                      	 	        client_antwort = text.byte_stream("Bitte gebe dein Password ein.\n");
                      	 	        anmeldung    =  "password";
                      	     } 
                      	     else
                      	     {    protokoll.erstellen( this.proto_woher , this.proto_gruppe , "Benutzer "+ benutzername +" wurde nicht gefunden." ,  this.proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
                      	 	        client_antwort = text.byte_stream("Benutzername wurde nicht gefunden Bitte Neu eingeben!\n");  }
                      	 
                             /* Info an Client senden */ 
                             stream.Write(client_antwort, 0, client_antwort.Length);
                         }
                         else if(anmeldung ==  "password") /* Wenn Benutzername OK ist Wird nach Password geprüft */
                         {
                      	     if( benutzer.liste( text.klein(benutzername) , text.klein(puffer_string) )  )
                      	     {
                      	 	        protokoll.erstellen( this.proto_woher , this.proto_gruppe , "Benutzer "+ benutzername +" wurde erfolgreich angemeldet. Warte auf Datenempfang." ,  this.proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
                      	 	        client_antwort = text.byte_stream("Willkommen im NOC Portal Backend. Ich warte jetzt auf Daten.\n");
                      	 	        anmeldung =  "ok";
                      	     } 
                      	     else
                      	     {    protokoll.erstellen( this.proto_woher , this.proto_gruppe , "Benutzer "+ benutzername +" hat Fehlerhaftes Password eingetragen." ,  this.proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
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
	  	  	  	       protokoll.erstellen( this.proto_woher , this.proto_gruppe , "TCP Verbindung wurde beendet." ,  this.proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
	  	  	  	    
	  	  	  	    
	  	  	          /*  Datenstrom Bauen und in einem String legen -- Start -- */
	  	  	  	       protokoll.erstellen( this.proto_woher , this.proto_gruppe , "Baue Datenstrom zum weiter Verarbeiten." ,  this.proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
	  	  	  	       komplett_byte = 0;
	  	  	  	       foreach (PortPuffer byte_daten in port_stream)  /* Byte stream Länge ermitteln */
                     {    komplett_byte += byte_daten.bitpuffer.Length;   } 
                  
	  	  	  	       memstream = new MemoryStream(new byte[komplett_byte], 0, komplett_byte , true, true);
	  	  	  	       foreach (PortPuffer byte_daten in port_stream)   /* Byte Pakete aus List holen und im MemoryStream zusammenfügen */ 
                     {   memstream.Write(byte_daten.bitpuffer, 0, byte_daten.bytelange);  }  
                  
                     komplett_byte_stream = memstream.GetBuffer(); /* Byte vom stream zusammenfügen und  in ein Gesamtes Array Byte legen  */
                  
                     
                     protokoll.erstellen( this.proto_woher , this.proto_gruppe , "Erstelle Daten String aus Datenstrom." ,  this.proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
                     
                     puffer_string = string.Empty;
                     puffer_string = text.text_stream( komplett_byte_stream , komplett_byte_stream.Length );  /* client_eingang Byte Array  mit angabe der länge was befühlt ist in  UTF8 string Übersetzten */
                     puffer_string = text.steuerzeichen( puffer_string , "alle" ); 
                    /*  Datenstrom Bauen und in einem String legen -- Ende -- */  
                    
                    /* Protokoll vom Client aus dem Text Strem holen und in eigenes Protokoll Schreiben */
                     string[] proto_client = text.split( "|Protokoll|" , puffer_string );
                     if(proto_client.Length > 0) /* Schutz das auch Protokolle da sind */
                     {
                     	  puffer_string = proto_client[0]; /* Eigentliche Daten zum Weiterverarbeiten in Variable wieder legen ( Liegen immer vor Protokoll )  */
                     	  
                     	  string[] proto_client_zeilen = text.split( "|tr|" , proto_client[1] ); /* Protokolle Zeilen ermitteln */
                     	  //bool fehler_client;
                     	  foreach(string  proto_daten in proto_client_zeilen)
                     	  { /* in dieser schleife werden die Protokolle übernohmen auf dem Server */
                     	     try
                     	     { string[] proto_inhalt = text.split( "|td|" , proto_daten );
                     	  	   //if(proto_inhalt[6] == "ja")fehler_client = true; else fehler_client = false;
                     	  	   //protokoll.erstellen( proto_inhalt[0] , proto_inhalt[1] , proto_inhalt[2] , proto_inhalt[3] ,proto_inhalt[4],proto_inhalt[5] , fehler_client , 0 );
                     	  	 }
                     	  	 catch(IndexOutOfRangeException e)
                     	  	 {
                     	  	 	  protokoll.erstellen( this.proto_woher , this.proto_gruppe , "Client Protokoll Schreiben Fehler: " + e.Message ,  proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
                     	  	 }
                     	  }
                     	  
                     	  
                     }
                     else { }
                    
                    
	  	  	  	       if(this.port_bezeichnung == "cfy_rohdaten")
                     {   /* CFY status nur ändern wenn Mit Port für CFY_Rohdaten gesprochen wird */
                           
                          protokoll.erstellen( this.proto_woher , this.proto_gruppe , "Daten werden an CFY Datenimport Übergeben." ,  this.proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
                          Clary clary = new Clary();
                          Clary.cfy_port_gruppe = this.proto_gruppe;
                          clary.rohdaten(puffer_string );
                     }
                     else {}
                     
                     
                   }
                   catch(ThreadAbortException) /* Thread wird von der Main aus sofort abgebrochen - Programm wurde beendet */
	  	  	         {  protokoll.erstellen( this.proto_woher , this.proto_gruppe , "Thread wurde von der Main sofort Beendet. ( .Abort() )"  ,  this.proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */  }
                   catch (System.IO.IOException e)
                   {  protokoll.erstellen( this.proto_woher , this.proto_gruppe , "Client hat Verbindung beendet. Info: " + e.Message ,  this.proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */  }
                   catch (SocketException e)
                   {  protokoll.erstellen( this.proto_woher , this.proto_gruppe , "SocketException wurde gewurfen Verbindung wurde getrennt. Fehler: " + e.Message ,  this.proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */ } 
                   
                   protokoll.erstellen( this.proto_woher , this.proto_gruppe , "Gebe TCP Verbindung wieder frei." ,  this.proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , false  ); /* Protokoll erstellen */
                   portzahler.kontrolle( max_verbindung , port , false ); /* Verbindung wieder Frei geben */ 
              }
              else
              { /* Maximale Verbindung für Port wurden festgestellt oder CFY DatenImport wird noch bearbeitet im System */
              	  
                	   NetworkStream stream = this.clientInstanz.GetStream(); /* Datenstrom vom Clienten holen und im Stream legen */ 
                	  
                	   /* Speziele regelung für CFY Rohdaten empfangen */
                     if(this.port_bezeichnung == "cfy_rohdaten" &&  Clary.cfy_port_status != "leer" ) 
          	         {     protokoll.erstellen( this.proto_woher , this.proto_gruppe , "CFY Rohdaten werden gerade im System Verarbeitet. Neue Verbindung wurde abgelehnt." ,  this.proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
                           client_antwort = text.byte_stream(" Achutung CFY Rohdaten Import wird gerade gerade im System verarbeitet. \n Neue Verbindung wurde abgebrochen. \n Bitte versuchen Sie es später noch einmal. \n");   }
                     else
                     {     protokoll.erstellen( this.proto_woher , this.proto_gruppe , "Verbindung wurde abgebrochen Maximale Verbindungen ist erreicht." ,  this.proto_klasse , debuger.path() , debuger.dateiName() , debuger.funktion() , debuger.zeile() , true  ); /* Protokoll erstellen */
                           client_antwort = text.byte_stream(" Achtung es sind keine Verbindungen mehr Frei! Es wurde abgebrochen! \n Bitte versuchen Sie es später noch einmal. \n");   }
                      	   
                     /* Info an Client senden */ 
                     stream.Write(client_antwort, 0, client_antwort.Length);

                     Thread.Sleep(2000); /* zwei Secunden warten und Verbindung dann  beenden */
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