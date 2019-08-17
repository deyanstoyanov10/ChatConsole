namespace ChatConsole
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class StartUpServer
    {
        public static void Main()
        {
            TcpListener listener = null;
            try
            {
                listener = new TcpListener(IPAddress.Any, 8080);
                listener.Start();
                Console.WriteLine("Server started...");

                while (true)
                {
                    Console.WriteLine("Waiting for incoming client connections...");
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Accepted new client connection...");
                    Thread t = new Thread(ProcessClientRequests);
                    t.Start(client);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (listener != null)
                {
                    listener.Stop();
                }
            }
        }

        private static void ProcessClientRequests(object argument)
        {
            TcpClient client = (TcpClient)argument;

            try
            {
                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());
                string s = String.Empty;

                while (!(s = reader.ReadLine()).Equals("Exit") || (s == null))
                {
                    Console.WriteLine(s);
                    writer.WriteLine(s);
                    writer.Flush();
                }
                reader.Close();
                writer.Close();
                client.Close();
                Console.WriteLine("Client connection closed!");
            }
            catch (IOException)
            {
                Console.WriteLine("Problem with client communication. Exiting thread.");
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                }
            }
        }
    }
}
