using System.Net.Sockets;
using System.Net;
using System;
using System.Threading;

/// <summary>
/// 网络模块
/// </summary>
public class NetModuleTCP
{
    private TcpClient cient;
    private NetworkStream stream;
    private byte[] receiveBuffer;
    private Thread thread;

    public void Setup()
    {
        Console.WriteLine("发起连接");
        cient = new TcpClient();
        receiveBuffer = new byte[1024];

        // 发起连接，方式一，APM
        cient.BeginConnect(IPAddress.Parse(Const.serverIp), Const.tcp_serverPort, ConnectCallback, cient);

        // 发起连接，方式二，TAP
        // await Connect();
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            cient.EndConnect(ar);
            Console.WriteLine("连接服务器成功");
            stream = cient.GetStream();

            // 接收消息，方式一，异步调用 + 尾递归
            Receive();

            // 接收消息，方式二，线程阻塞
            //thread = new Thread(new ThreadStart(ReceiveThread));
            //thread.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine("连接服务器异常：" + ex.Message);
        }
    }

    async Task Connect()
    {
        await tcpClient.ConnectAsync(IPAddress.Parse("192.168.0.11"), 11000);
        Console.WriteLine("连接服务器成功");

        // 接收消息，方式三，TAP
        stream = tcpClient.GetStream();
        // 启动接收数据的任务
        var receiveTask = ReceiveDataAsync(stream);
    }

    void Receive()
    {
        stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReadCallback, stream);
    }

    private void ReadCallback(IAsyncResult ar)
    {
        int length = stream.EndRead(ar);
        string message = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, length);
        Console.WriteLine($"从服务器接收到消息：" + message);

        // 尾递归
        Receive();
    }

    //接收线程
    void ReceiveThread()
    {
        while (true)
        {
            int length = cient.Available;
            if (cient.Connected && length > 0)
            {
                stream.Read(receiveBuffer, 0, length);
                string message = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, length);
                Console.WriteLine($"从服务器接收到消息：" + message);
            }
        }
    }

    async Task ReceiveDataAsync(NetworkStream stream)
    {
        try
        {
            while (true)
            {
                int bytesRead = await stream.ReadAsync(receiveBuffer, 0, receiveBuffer.Length);
                if (bytesRead == 0)
                {
                    Console.WriteLine("服务器关闭连接");
                    break;
                }

                string received = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);
                Console.WriteLine("收到：" + received);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("接收异常：" + ex.Message);
        }
    }
    
    // 发送
    public void Send(string message)
    {
        if (stream == null)
            return;
        
        byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(message);

        // 发送方式一，用 NetworkStream
        stream.Write(sendBytes, 0, sendBytes.Length);

        // 发送方式二，用 TcpClient 的 Client（Socket）
        //int length = tcpClient.Client.Send(sendBytes);
        //Console.WriteLine($"发送字节数：{length}");
    }

    public void Disconnect()
    {
        if (cient != null)
            cient.Close();
    }

    public void Close()
    {
        Console.WriteLine("关闭 TCP");

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
        if (cient != null)
        {
            cient.Close();
            cient = null;
        }
    }
}
