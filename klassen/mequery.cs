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
   /                    - "PortZuweisung" Klasse  sucht alle Netzwerkkomponenetn zb IP Adresse, Status,        /
   /                       Lebenszeit, Welche art usw und Speichert diese in ein List                          /
   /                    - "AsciiPic" Klasse hier sind Bilder in Ascii Format hinterlegt                        /
   /                                                                                                           /
   *************************************************************************************************************  
*/


using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text;
using System.Collections.Generic;
using System.Collections;



namespace MEQery
{   
	
	 public class Benutzer
	 {
	 	     
	 	     
	 	     private bool prufung(string p_name ,string p_passw = null ,string was = null )
	 	     {
	 	     	   bool aussage = false;
	 	     	        if(p_name == "meiko"   && p_passw == "dell" || 
	 	     	           p_name == "meiko"   && was     == "name" ) aussage = true;
	 	     	   else if(p_name == "martin"  && p_passw == "dell" || 
	 	     	           p_name == "martin"  && was     == "name" ) aussage = true;
	 	     	   else { }
	 	     	   
	 	     	   return aussage;
	 	     }
	 	     
	 	     public bool liste(string p_name ,string p_passw = null ,string was = null )
	 	     {
	 	     	  return prufung(p_name ,p_passw, was);
	 	     }
	 	     
	 	
	 }
	 
	 public class MYSQL
	 {
	 	     private  string benutzer_in()
	 	     {   return "root";   }
	 	     
	 	     private  string password_in()
	 	     {   return "pass";   }
	 	     
	 	     private  string ipadresse_in()
	 	     {   return "127.0.0.1";   }
	 	     
	 	     
	 	     public  string ben()
	 	     {   return benutzer_in();   }
	 	     
	 	     public  string pass()
	 	     {   return password_in();   }
	 	     
	 	     public  string ip()
	 	     {   return ipadresse_in();   }
	 }
	 
   public class EventObjekt
   {
   	    
         public bool tastatur(ConsoleKeyInfo taste,string was )
	  	   {  /*  Eventhändler */
	  	  	
	  	  	 bool ruckgabe = false;
	  	  	 if(was == "altgr+s" && ( taste.Modifiers & ConsoleModifiers.Control) != 0  && taste.Key == ConsoleKey.S) ruckgabe = true; else {}
	  	  	 if(was == "altgr+1" && ( taste.Modifiers & ConsoleModifiers.Control) != 0  && taste.Key == ConsoleKey.D1) ruckgabe = true; else {}
	  	  	 if(was == "altgr+2" && ( taste.Modifiers & ConsoleModifiers.Control) != 0  && taste.Key == ConsoleKey.D2) ruckgabe = true; else {}
	  	  	 if(was == "altgr+3" && ( taste.Modifiers & ConsoleModifiers.Control) != 0  && taste.Key == ConsoleKey.D3) ruckgabe = true; else {}
	  	  	 if(was == "altgr+4" && ( taste.Modifiers & ConsoleModifiers.Control) != 0  && taste.Key == ConsoleKey.D4) ruckgabe = true; else {}
	  	  	 if(was == "altgr+5" && ( taste.Modifiers & ConsoleModifiers.Control) != 0  && taste.Key == ConsoleKey.D5) ruckgabe = true; else {}
	  	  	 if(was == "altgr+c" && ( taste.Modifiers & ConsoleModifiers.Control) != 0  && taste.Key == ConsoleKey.C) ruckgabe = true; else {}
	  	  	
	  	  	 return ruckgabe;
	  	   }
   }
   
   
   public class Protokol
   {
   	
   	    public class Protokol_List
   	    {
   	    	   public string  woher;
   	    	   public string  inhalt;
   	    	   public string  datei;
   	    	   public string  klasse;
   	    	   public string  funktion;
   	    	   public bool    fehler;
   	    	   public int     unixzeit;
   	    	   public string  gruppe;
   	    	   
   	    	   public Protokol_List( ) { }   
   	    	   
   	    	   public Protokol_List(string woher,string gruppe ,string inhalt,string datei,string klasse,string  funktion,bool fehler,int unixzeit ) 
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
   	    
   	    /* Protokol Liste erstellen */
   	    public static List<Protokol_List> liste = new List<Protokol_List>();
   	     
   	    public void erstellen(string woher,string gruppe,string inhalt,string datei,string klasse ,string  funktion ,bool fehler )
   	    {    
   	    	   Datum datum = new Datum();
   	     	   liste.Add(new Protokol_List( woher , gruppe , inhalt ,datei ,klasse ,funktion ,fehler , datum.unix() ) );   	     	 
   	    }
   	    
   }
   
   
   /* String KLasse um Inhalt zu bearbeiten */
   public class Text 
   {
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
	      
   }

   
   public class PortZuweisung
   {
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
   	     	  erstellen();
   	     }
   	    
   }
   
   
   public class Host
   {   
   	   public string name;
   	   
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
                         
                        /* Maximale Lebensdauer ermitteln was vom DNS zugewiesen wurde in Secunden */
                        max_lebenszeit = UnicastIPAddressInformation.DhcpLeaseLifetime.ToString();

                        /* Verbleibende lebenszeit der IP Adresse in Secunden */
                        offen_lebenszeit = UnicastIPAddressInformation.AddressValidLifetime.ToString();


                }               
             
                netzw_daten.Add(new Neztwerk_List( name , type , status , ip4_or_ip6 , physical_adresse , max_lebenszeit , offen_lebenszeit , ip_adresse , aktiv_ip  ) ); 
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
}