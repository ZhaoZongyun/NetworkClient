using HT.Framework;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Threading;

/// <summary>
/// 网络模块
/// </summary>
[CustomModule("NetModuleUDP", true)]
public partial class NetModuleUDP : CustomModuleBase
{
    private static NetModuleUDP _instance;
    public static NetModuleUDP Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Main.m_CustomModule["NetModuleUDP"].Cast<NetModuleUDP>();
            }

            return _instance;
        }
    }

    //服务器端口
    private const int port = 11000; //服务器和客户端在一台电脑，客户端绑定的端口不能与服务器相同，否则端口被占用报错
    private UdpClient udpClient;
    private IPEndPoint endPoint;
    private Thread thread;

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnReady()
    {
        base.OnReady();

        udpClient = new UdpClient(22000);   //绑定一个端口
        endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);  //接收远程ip和端口的数据

        //开启一个线程接收服务器消息，否则主线程卡死
        thread = new Thread(new ThreadStart(Receive));
        thread.Start();
    }

    //接收
    void Receive()
    {
        while (true)
        {
            byte[] bytes = udpClient.Receive(ref endPoint);

            // 将接收到的消息解析为字符串，为坐标数据，例如 10,15;20;30
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Log.Info("从服务器接收到消息：" + message);
        }
    }

    /// <summary>
    /// 发送网络消息
    /// </summary>
    public void Send(string message)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(message);
        udpClient.Send(bytes, bytes.Length, endPoint);
    }

    public override void OnTerminate()
    {
        base.OnTerminate();

        Log.Info("退出 NetModuleUDP");
        if (thread != null)
            thread.Abort();
        if (udpClient != null)
            udpClient.Close();
    }
}
