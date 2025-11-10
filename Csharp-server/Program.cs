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
            await SendKey(stream, serverKey);

            string clientKey = await ReceiveClientKey(stream);

            Console.WriteLine($"Key from client: {clientKey}");

            SendResult(stream, serverKey, clientKey);

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
        
        static async Task SendKey(NetworkStream stream, string key)
        {
            byte[] sendBuffer = Encoding.UTF8.GetBytes(key);
            await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length);
            await stream.FlushAsync();

            Console.Clear();
            Console.WriteLine($"Key sent to client: {key}");
        }
        
        static async Task<string> ReceiveClientKey(NetworkStream stream)
        {
            byte[] readBuffer = new byte[1024];

            while (!stream.DataAvailable)
            {
                Console.Clear();
                Console.WriteLine("Waiting for key sending from client");
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
        
        static void SendResult(NetworkStream stream, string serverKey, string clientKey)
        {
            byte response = (byte)(serverKey == clientKey ? 1 : 0);
            stream.WriteByte(response);

            Console.WriteLine(serverKey == clientKey
                ? "Keys match — client verified!"
                : "Keys mismatch!");
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
