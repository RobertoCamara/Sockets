using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
           ServerTcp();
        }

        private static void ServerTcp()
        {
            try
            {
                IPAddress ipAd = IPAddress.Parse("192.168.1.44");
                TcpListener myList = new TcpListener(ipAd, 8001);

                myList.Start();

                Console.WriteLine("Servidor FTP rodando na porta 8001...");
                Console.WriteLine("Endpoint local é :" + myList.LocalEndpoint);
                Console.WriteLine("Aguardando uma conexão.....");

                Socket socket = myList.AcceptSocket();

                while (socket.Connected)
                {
                    Console.WriteLine("\r\nConexão aceita de: " + socket.RemoteEndPoint);
                    ReceberMensagem(socket);
                    NotificarRecebimento(socket);
                }

                socket.Close();
                myList.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }

        private static void NotificarRecebimento(Socket socket)
        {
            if (!socket.Connected)
            {
                Console.WriteLine("Nenhuma conexão ativa.");
                return;
            }

            ASCIIEncoding asen = new ASCIIEncoding();
            socket.Send(asen.GetBytes($"Mensagem recebida pelo servidor em: {DateTime.Now.ToString("HH:mm:ss.ffff")}\r\n"));
            Console.WriteLine("\r\nCliente notificado sobre recebimento.");
        }

        private static void ReceberMensagem(Socket socket)
        {
            if (!socket.Connected)
            {
                Console.WriteLine("Nenhuma conexão ativa.");
                return;
            }

            byte[] b = new byte[100];
            int k = socket.Receive(b);
            Console.WriteLine("Recebendo...");
            for (int i = 0; i < k; i++)
                Console.Write(Convert.ToChar(b[i]));
        }
    
    }
}
