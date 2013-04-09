using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Linq;

// State object for reading client data asynchronously

public class SynchronousSocketListener
{
    // Incoming data from the client.
    public static string data = null;

    public static void StartListening(byte[] f)
    {
       /* const string policy = "<?xml version=\"1.0\"?>" +
"<!DOCTYPE cross-domain-policy SYSTEM \"http://www.adobe.com/xml/dtds/cross-domain-policy.dtd\"> " +
"<!-- Policy file for xmlsocket://socks.mysite.com --> " +
"<cross-domain-policy>  " +
    "<allow-access-from domain=\"*\" to-ports=\"22000\" />  " +
"</cross-domain-policy> ";*/

        byte[] bytes = new Byte[1024];

        IPAddress ipAddress = new IPAddress(new byte[] { 127, 0, 0, 1 });
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        Socket listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);

        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                Socket handler = listener.Accept();
                data = null;
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                Console.WriteLine("Text received : {0}", data);

                
              //  byte[] msg = Encoding.ASCII.GetBytes(policy);

                handler.Send(f);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();

    }
    public static int Main(String[] args)
    {
        Stream s = new FileStream("crossdomain.xml", FileMode.Open);
        BinaryReader reader = new BinaryReader(s);
        byte[] fi = reader.ReadBytes(Convert.ToInt32(s.Length));
        fi.Concat(new byte[0]);
        StartListening(fi);
        return 0;
    }
}



