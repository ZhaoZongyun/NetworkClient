using HT.Framework;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using Debug = UnityEngine.Debug;
using System;
using System.Threading;

/// <summary>
/// 网络模块
/// </summary>
[CustomModule("NetModuleTCP", true)]
public partial class NetModuleTCP : CustomModuleBase
{
    private static NetModuleTCP _instance;
    public static NetModuleTCP Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Main.m_CustomModule["NetModuleTCP"].Cast<NetModuleTCP>();
            }

            return _instance;
        }
    }

    private TcpClient tcpClient;
    private IPEndPoint endPoint;
    private NetworkStream stream;
    private byte[] receiveBuffer;
    private Thread thread;

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnReady()
    {
        base.OnReady();

        tcpClient = new TcpClient();
        receiveBuffer = new byte[1024];
        Debug.Log("发起连接");

        tcpClient.BeginConnect(IPAddress.Parse("192.168.0.33"), 11000, ConnectCallback, tcpClient);
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            tcpClient.EndConnect(ar);
            Debug.Log("连接服务器成功");
            stream = tcpClient.GetStream();


            // 接收消息，方式一，异步调用 + 尾递归
            Receive();

            // 接收消息，方式二，线程阻塞
            //thread = new Thread(new ThreadStart(ReceiveThread));
            //thread.Start();
        }
        catch (Exception ex)
        {
            Debug.Log("连接服务器异常：" + ex.Message);
        }
    }

    void Receive()
    {
        stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReadCallback, stream);
    }

    private void ReadCallback(IAsyncResult ar)
    {
        int length = stream.EndRead(ar);
        string message = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, length);
        Debug.Log($"从服务器接收到消息：" + message);

        // 尾递归
        Receive();
    }

    //接收线程
    void ReceiveThread()
    {
        while (true)
        {
            int length = tcpClient.Available;
            if (tcpClient.Connected && length > 0)
            {
                stream.Read(receiveBuffer, 0, length);
                string message = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, length);
                Debug.Log($"从服务器接收到消息：" + message);
            }
        }
    }

    // 发送
    public void Send(string message)
    {
        byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(message);

        // 发送方式一，用 NetworkStream
        stream.Write(sendBytes, 0, sendBytes.Length);

        // 发送方式二，用 TcpClient 的 Client（Socket）
        //int length = tcpClient.Client.Send(sendBytes);
        //Console.WriteLine($"发送字节数：{length}");
    }

    public void Disconnect()
    {
        if (tcpClient != null)
            tcpClient.Close();
    }

    public override void OnTerminate()
    {
        base.OnTerminate();
        Log.Info("退出 NetModuleTCP");

        if (thread != null)
        {
            thread.Abort();
            thread = null;
        }
        if (stream != null)
        {
            stream.Close();
            stream = null;
        }
        if (tcpClient != null)
        {
            tcpClient.Close();
            tcpClient = null;
        }
    }
}
