using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Util.ExtensionMethods;
using Util.Logger;

namespace SendTCP
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                using (TcpClient clientTcp = new TcpClient("192.168.1.44", 11000))
                {
                    Console.WriteLine($"{DateTime.Now.ToString("dd-mm-yyyy HH:mm:ss ffff")} - Connecting.....");

                    Console.WriteLine($"{DateTime.Now.ToString("dd-mm-yyyy HH:mm:ss ffff")} - Connected!!!\r\n");

                    while (clientTcp.Connected)
                    {
                        Stream streamNetwork = SendMessageTCP(clientTcp);
                        string messageReceived = ReceivedMessageConfirm(clientTcp, streamNetwork);

                        Console.WriteLine(messageReceived + "\r\n");
                        Thread.Sleep(2000);
                    }
                }
            }

            catch (Exception e)
            {
                LogController.Instance.GravarLogErro(e);
            }
        }

        private static string ReceivedMessageConfirm(TcpClient clientTcp, Stream streamNetwork)
        {
            byte[] bytesConfirmReceived = new byte[clientTcp.ReceiveBufferSize];
            int totalBytesReceived = streamNetwork.Read(bytesConfirmReceived, 0, clientTcp.ReceiveBufferSize);

            string messageReceived = "";
            for (int i = 0; i < totalBytesReceived; i++)
                messageReceived += Convert.ToChar(bytesConfirmReceived[i]);
            return messageReceived;
        }

        private static Stream SendMessageTCP(TcpClient clientTcp)
        {
            string messageSend = $"{DateTime.Now.ToString("dd-mm-yyyy HH:mm:ss ffff")} - {Guid.NewGuid().ToString()}";
            Stream streamNetwork = clientTcp.GetStream();

            byte[] bytesSend = messageSend.StringToBytes();
            Console.WriteLine($"{DateTime.Now.ToString("dd-mm-yyyy HH:mm:ss ffff")} - Transmitting.....");

            streamNetwork.Write(bytesSend, 0, bytesSend.Length);
            return streamNetwork;
        }
    }
   
}
