using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace FastKing
{
    public class UDPManager
    {
        public Action<string> onDebug;
        public Action<string> onReceiveMsg;

        private UdpClient udpClient = null;
        private string ip, remoteIP;
        private int port, remotePort;
        private IPEndPoint remotePoint;

        public UDPManager(string ip, int port, string remoteIP, int remotePort)
        {
            this.ip = ip;
            this.port = port;
            this.remoteIP = remoteIP;
            this.remotePort = remotePort;
        }

        public void StartServer()
        {
            try
            {
                //本机
                IPAddress locateIpAddr = IPAddress.Parse(ip);
                IPEndPoint locatePoint = new IPEndPoint(locateIpAddr, port);
                udpClient = new UdpClient(locatePoint);
                onDebug?.Invoke("UDP Start");

                //远程。提示：同一计算机连接时，可提供相同的ip和不同的端口
                IPAddress remoteIp = IPAddress.Parse(remoteIP);
                remotePoint = new IPEndPoint(remoteIp, remotePort);

                //Receive();
                ReceiveAdvance();
            }
            catch (Exception e)
            {
                onDebug?.Invoke("Error:" + e.Message);
            }
        }

        public void Close()
        {
            if (udpClient != null)
            {
                udpClient.Close();
            }
        }

        public void Send(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);

            if (udpClient != null)
            {
                udpClient.Send(buffer, buffer.Length, remotePoint);
                //onDebug?.Invoke("Send OK");
            }
        }


        public void Receive()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (udpClient != null)
                    {
                        var received = udpClient.Receive(ref remotePoint);
                        string info = Encoding.UTF8.GetString(received);

                        if (!string.IsNullOrEmpty(info))
                        {
                            onReceiveMsg?.Invoke(info);
                            //onDebug?.Invoke("Receive:" + info);
                        }
                    }
                }
            });
        }


        
        public event UDPReceivedEventHandler UDPMessageReceived;
        public void ReceiveAdvance()
        {
            UDPMessageReceived += (args) => {
                var remotePoint = args.remoteEndPoint;
                string info = Encoding.UTF8.GetString(args.buffer);
                if (!string.IsNullOrEmpty(info))
                {
                    onReceiveMsg?.Invoke(info);
                    //onDebug?.Invoke("Receive:" + info);
                }
            };

            Task.Run(() =>
            {
                while (true)
                {
                    UdpStateEventArgs udpReceiveState = new UdpStateEventArgs();

                    if (udpClient != null)
                    {
                        var received = udpClient.Receive(ref remotePoint);
                        udpReceiveState.remoteEndPoint = remotePoint;
                        udpReceiveState.buffer = received;
                        UDPMessageReceived?.Invoke(udpReceiveState);
                    }
                    else
                    {
                        break;
                    }
                }
            });
        }
    }


    public class UdpStateEventArgs : EventArgs
    {
        public IPEndPoint remoteEndPoint;
        public byte[] buffer = null;
    }
    public delegate void UDPReceivedEventHandler(UdpStateEventArgs args);
}
