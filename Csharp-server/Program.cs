using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_server;

class Program
{
    static readonly char[] HEX_DIGITS = "0123456789ABCDEF".ToCharArray();
    
    static async Task Main()
    {
        string ip = "192.168.1.77";
        int port = 8686;

        byte[] readBuffer, sendBuffer;
        int bytesRead;
        
        // Keys
        string serverKey, clientKey;
        
        TcpListener server = new TcpListener(IPAddress.Parse(ip), port);
        
        server.Start();

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

        serverKey = GenerateRandomKey();
        
        NetworkStream stream = client.GetStream();
        
        readBuffer = new byte[1024];

        sendBuffer = Encoding.UTF8.GetBytes(serverKey);
        
        await stream.WriteAsync(sendBuffer, 0, sendBuffer.Length);

        Console.Clear();
        Console.WriteLine($"Key sent to client: {serverKey}");
        
        while (!stream.DataAvailable)
        {
            Console.Clear();
            
            Console.WriteLine("Waiting for key acceptation from client");
            await Task.Delay(500);
            Console.Write(".");
            await Task.Delay(500);
            Console.Write(".");
            await Task.Delay(500);
            Console.Write(".");
            await Task.Delay(500);
        }

        bytesRead = await stream.ReadAsync(readBuffer, 0, readBuffer.Length);
        
        clientKey = Encoding.UTF8.GetString(readBuffer, 0, bytesRead).Trim();
        
        Console.WriteLine($"Key from client: {clientKey}");

        byte response = (byte)(serverKey == clientKey ? 1 : 0);
        stream.WriteByte(response);
        
        stream.Close();
        
        client.Close();
        
        server.Stop();

        Console.Write("\nServer stop");
        
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