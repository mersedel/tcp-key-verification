using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_server
{
    class Program
    {
        static readonly char[] HEX_DIGITS = "0123456789ABCDEF".ToCharArray();
        
        const string IP = "192.168.1.77";
        const int PORT = 8686;

        static async Task Main()
        {
            await StartServer(IP, PORT);
        }
        
        static async Task StartServer(string ip, int port)
        {
            TcpListener server = new TcpListener(IPAddress.Parse(ip), port);
            server.Start();

            Console.WriteLine("Server started.");

            TcpClient client = await WaitForClient(server);

            NetworkStream stream = client.GetStream();

            string serverKey = GenerateRandomKey();
            await SendKeyAsync(stream, serverKey);

            string clientKey = await ReceiveClientKeyAsync(stream);

            Console.WriteLine($"\n\nKey from client: {clientKey}");

            await SendResultAsync(stream, serverKey, clientKey);

            CloseConnections(stream, client, server);
        }
        
        static async Task<TcpClient> WaitForClient(TcpListener server)
        {
            Task<TcpClient> clientTask = server.AcceptTcpClientAsync();

            while (!clientTask.IsCompleted)
            {
                Console.Clear();
                Console.WriteLine("Server started\n");
                Console.Write("Waiting for client");
                await Task.Delay(500);
                Console.Write(".");
                await Task.Delay(500);
                Console.Write(".");
                await Task.Delay(500);
                Console.Write(".");
                await Task.Delay(500);
            }

            TcpClient client = await clientTask;
            Console.Clear();
            Console.WriteLine("Client connected!");
            return client;
        }
        
        static async Task SendKeyAsync(NetworkStream stream, string key)
        {
            byte[] sendBuffer = Encoding.UTF8.GetBytes(key);
            await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length);
            await stream.FlushAsync();

            Console.Clear();
            Console.WriteLine($"Key sent to client: {key}");
        }
        
        static async Task<string> ReceiveClientKeyAsync(NetworkStream stream)
        {
            byte[] readBuffer = new byte[1024];

            while (!stream.DataAvailable)
            {
                Console.Clear();
                Console.Write("Waiting for key sending from client");
                await Task.Delay(500);
                Console.Write(".");
                await Task.Delay(500);
                Console.Write(".");
                await Task.Delay(500);
                Console.Write(".");
                await Task.Delay(500);
            }

            int bytesRead = await stream.ReadAsync(readBuffer, 0, readBuffer.Length);
            string clientKey = Encoding.UTF8.GetString(readBuffer, 0, bytesRead).Trim();

            return clientKey;
        }
        
        static async Task SendResultAsync(NetworkStream stream, string serverKey, string clientKey)
        {
            string result = (serverKey == clientKey) ? "1" : "0";
            byte[] sendBuffer = Encoding.UTF8.GetBytes(result);

            await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length);
            await stream.FlushAsync();

            Console.WriteLine(serverKey == clientKey
                ? "\nKeys match — client verified!"
                : "\nKeys mismatch!");
        }
        
        static void CloseConnections(NetworkStream stream, TcpClient client, TcpListener server)
        {
            stream.Close();
            client.Close();
            server.Stop();

            Console.WriteLine("\nServer stopped.");
            Console.ReadLine();
        }
        
        static string GenerateRandomKey(int length = 16)
        {
            Random rnd = new Random();
            StringBuilder keyBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int index = rnd.Next(HEX_DIGITS.Length);
                keyBuilder.Append(HEX_DIGITS[index]);
            }

            return keyBuilder.ToString();
        }
    }
}
