using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using lab3;
using System.Net.Http;
namespace lab3
{
    internal class Client
    {
        private const int DefaultPort = 5678;
        private TcpClient tcpClient;
        private NetworkStream stream;
        private byte[] buffer = new byte[1024];
        private bool isConnected = false;

        public async Task ConnectAsync(string host = "127.0.0.1", int port = 5678)
        {
            try
            {
                tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(host, port);
                stream = tcpClient.GetStream();
                isConnected = true;

                Console.WriteLine($"Подключение к {host}:{port} установлено");

                // Запускаем асинхронное чтение данных
                await ReadDataAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения: {ex.Message}");
            }
        }

        private async Task ReadDataAsync()
        {
            while (isConnected)
            {
                try
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Получено: {message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка чтения данных: {ex.Message}");
                    Disconnect();
                    break;
                }
            }
        }

        public async Task SendMessageAsync(string message)
        {
            if (!isConnected) return;

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки данных: {ex.Message}");
                Disconnect();
            }
        }

        public void Disconnect()
        {
            isConnected = false;
            stream?.Close();
            tcpClient?.Close();
            Console.WriteLine("Подключение закрыто");
        }
    }

}
