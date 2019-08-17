namespace ChatConsole
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;

    public class StartUpClient
    {
        public static void Main(String[] args)
        {
            IPAddress ip_address = IPAddress.Parse("127.0.0.1");
            int port = 8080;
            try
            {
                if (args.Length >= 1)
                {
                    ip_address = IPAddress.Parse(args[0]);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid IP address entered. Using default IP of: "
                                                    + ip_address.ToString());
            }
            try
            {
                Console.WriteLine("Attempting to connect to server at IP address: {0} port: {1}",
                                                    ip_address.ToString(), port);
                TcpClient client = new TcpClient(ip_address.ToString(), port);

                if (client.Connected)
                {
                    Console.WriteLine("Connection successful!");
                }

                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());

                Console.Write("Enter a name: ");
                string nameEnter = Console.ReadLine();
                Console.WriteLine();
                string name = nameEnter + "-> ";

                string s = String.Empty;
                while (!s.Equals("Exit"))
                {
                    Console.Write("Enter a message: ");
                    s = Console.ReadLine();
                    Console.WriteLine();
                    writer.WriteLine(name + s);
                    writer.Flush();
                    if (!s.Equals("Exit"))
                    {
                        String server_string = reader.ReadLine();
                        Console.WriteLine(server_string);
                    }
                    else
                    {
                        writer.WriteLine(nameEnter + " exit the chat.");
                        writer.Flush();
                        Console.WriteLine("You exit the chat.");
                    }
                }
                reader.Close();
                writer.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
