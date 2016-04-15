using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grit.Net.Common.Network
{
    public class HttpServer
    {
        private const int lenSyncWork = 24;//运行同时运行处理Socket连接最大数
        private Socket server_Socket;//服务端Socket对象
        private bool isAlive = false;//是否可用
        public void StartListen(int port)
        {
            server_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipend = new IPEndPoint(IPAddress.Any, port);
            try
            {
                server_Socket.Bind(ipend);
                server_Socket.Listen(lenSyncWork);
            }
            catch (SocketException) { }
            catch (ObjectDisposedException) { }
            catch (Exception) { }
            isAlive = true;
            Thread thread = new Thread(Accept);
            thread.Start();
        }

        private void Accept()
        {
            while (isAlive)
            {
                Socket client_Socket = null;
                try
                {
                    client_Socket = server_Socket.Accept();
                }
                catch (SocketException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }

                if (client_Socket != null)
                {
                    Thread work = new Thread(new ThreadStart(delegate { Receiver(client_Socket); }));
                    work.Start();
                }
            }
        }
        private void Receiver(Socket socket)
        {
          
        }
    }
}
