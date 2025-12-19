using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System;
using System.Collections.Generic;

/// <summary>
/// 网络模块
///     若服务器和客户端在一台电脑，客户端绑定的端口不能与服务器相同，否则端口被占用报错
/// </summary>
public class NetModuleUDP
{
    private IPEndPoint clientPoint;
    private IPEndPoint serverPoint;
    private UdpClient client;

    private Thread thread;

    private Queue<string> messageQueue;

    public void Setup()
    {
        // UDP 初始化
        messageQueue = new Queue<string>();

        clientPoint = new IPEndPoint(IPAddress.Any, Const.udp_clientPort);
        serverPoint = new IPEndPoint(IPAddress.Parse(Const.serverIp), Const.udp_serverPort);
        client = new UdpClient(clientPoint);

        // 接收消息，方式一，异步调用 + 尾递归
        client.BeginReceive(ReceiveCallback, null);

        // 接收消息，方式二，线程阻塞
        // 开启一个线程接收服务器消息，否则主线程卡死
        thread = new Thread(new ThreadStart(Receive));
        thread.Start();
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        if (ar.IsCompleted)
        {
            byte[] bytes = client.EndReceive(ar, ref receivePoint);
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Console.WriteLine("UPD 接收到消息：" + message);
            messageQueue.Enqueue(message);

            client.BeginReceive(ReceiveCallback, null);
        }
    }

    //接收
    void Receive()
    {
        while (true)
        {
            byte[] bytes = client.Receive(ref receivePoint);

            // 将接收到的消息解析为字符串，为坐标数据，例如 10,15;20;30
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Console.WriteLine("从服务器接收到消息：" + message);
            messageQueue.Enqueue(message);
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    public void Send(string message)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(message);
        client.Send(bytes, bytes.Length, serverPoint);
    }

    public void Close()
    {
        Console.WriteLine("关闭 UDP");

        if (client != null)
            client.Close();

        if (thread != null)
            thread.Abort();
    }
}
