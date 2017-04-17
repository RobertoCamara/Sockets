using System;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using Util.ExtensionMethods;
using Util.Logger;

namespace ListenTCP
{
    partial class ListenTCPService : ServiceBase
    {

        private static TcpListener _tcpListener;

        static ListenTCPService()
        {
            _tcpListener = new TcpListener(IPAddress.Parse("192.168.1.44"), 11000);
        }

        public ListenTCPService()
        {
            InitializeComponent();
#if (DEBUG)
            OnStart(null);
#endif
        }

        protected override void OnStart(string[] args)
        {
            ListenTCP();
        }

        private static void ListenTCP()
        {
            try
            {
                _tcpListener.Start();

                LogController.Instance.GravarLogInformacao("Servidor FTP rodando na porta 11000...");
                LogController.Instance.GravarLogInformacao("Endpoint local é :" + _tcpListener.LocalEndpoint);
                LogController.Instance.GravarLogInformacao("Aguardando uma conexão.....");

                Socket socket = _tcpListener.AcceptSocket();

                while (socket.Connected)
                {
                    LogController.Instance.GravarLogInformacao("Conexão aceita de: " + socket.RemoteEndPoint);
                    ReceberMensagem(socket);
                    NotificarRecebimento(socket);
                }

                socket.Close();
                _tcpListener.Stop();
            }
            catch (Exception e)
            {
                LogController.Instance.GravarLogErro(e);
            }
        }

        protected override void OnStop()
        {
            _tcpListener.Stop();
            LogController.Instance.GravarLogInformacao("Serviço parado.");
        }

        private static void NotificarRecebimento(Socket socket)
        {
            if (!socket.Connected)
            {
                LogController.Instance.GravarLogInformacao("Nenhuma conexão ativa.");
                return;
            }

            string messageSend = $"{DateTime.Now.ToString("HH:mm:ss.ffff")} - Confirmação de recebimento endpoint {socket.RemoteEndPoint}";
            byte[] bytesSend = messageSend.StringToBytes();
            socket.Send(bytesSend, bytesSend.Length, SocketFlags.None);
            LogController.Instance.GravarLogInformacao("Cliente notificado sobre recebimento.");
        }

        private static void ReceberMensagem(Socket socket)
        {
            if (!socket.Connected)
            {
                LogController.Instance.GravarLogInformacao("Nenhuma conexão ativa.");
                return;
            }

            byte[] bytesReceived = new byte[socket.ReceiveBufferSize];
            int totalBytesReceived = socket.Receive(bytesReceived);
            LogController.Instance.GravarLogInformacao("Recebendo...");

            string messageReceived = "";
            for (int i = 0; i < totalBytesReceived; i++)
                messageReceived += Convert.ToChar(bytesReceived[i]);

            LogController.Instance.GravarLogInformacao("Mensagem recebida: " + messageReceived);
        }
    }
}
