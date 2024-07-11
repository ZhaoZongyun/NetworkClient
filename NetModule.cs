using Google.Protobuf;
using HT.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using Debug = UnityEngine.Debug;
using System.Threading;

/// <summary>
/// 网络模块
/// </summary>
[CustomModule("NetModule", true)]
public partial class NetModule : CustomModuleBase
{
    private static NetModule _instance;
    public static NetModule Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Main.m_CustomModule["NetModule"].Cast<NetModule>();
            }

            return _instance;
        }
    }

    //服务器端口
    private const int port = 11000; //服务器和客户端在一台电脑，端口不能与服务器程序相同，否则端口被占用
    private UdpClient udpClient;
    private IPEndPoint endPoint;
    private Thread thread;

    private Dictionary<NetMessageId, List<Action<IMessage>>> callbackDict = new Dictionary<NetMessageId, List<Action<IMessage>>>();

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnReady()
    {
        base.OnReady();

        udpClient = new UdpClient(11001);
        endPoint = new IPEndPoint(IPAddress.Parse("192.168.0.33"), port);

        //开启一个线程接收服务器消息，否则主线程卡死
        thread = new Thread(new ThreadStart(SocketReceive));
        thread.Start();
    }

    //接收
    void SocketReceive()
    {
        while (true)
        {
            byte[] bytes = udpClient.Receive(ref endPoint);
            short id = GetInt16(bytes, 0);
            NetMessageId messageId = (NetMessageId)id;
            byte[] contentBytes = bytes.Skip(2).Take(bytes.Length - 2).ToArray();
            Debug.Log($"从服务器({endPoint})接收到消息 " + messageId.ToString());
            OnReceive(messageId, contentBytes);
        }
    }

    /// <summary>
    /// 发送网络消息
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="totalBytes"></param>
    public void Send(NetMessageId messageId, byte[] totalBytes)
    {
        udpClient.Send(totalBytes, totalBytes.Length, endPoint);
    }

    public override void OnTerminate()
    {
        base.OnTerminate();

        Log.Info("退出 NetModule");
        if (udpClient != null)
            udpClient.Close();
        if (thread != null)
            thread.Abort();
    }

    public void AddNetMessageEvent(NetMessageId id, Action<IMessage> action)
    {
        if (!callbackDict.ContainsKey(id))
        {
            callbackDict.Add(id, new List<Action<IMessage>>());
            callbackDict[id].Add(action);
        }
        else
        {
            if (!callbackDict[id].Contains(action))
                callbackDict[id].Add(action);
        }
    }

    private void CallNetMessageEvent(NetMessageId id, IMessage data)
    {
        List<Action<IMessage>> list;
        callbackDict.TryGetValue(id, out list);
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i](data);
            }
        }
    }

    /// <summary>
    /// 根据消息id和proto实例创建一条消息的完整字节数组
    /// </summary>
    private byte[] CreateTotalBytes(NetMessageId messageId, IMessage data)
    {
        byte[] contentBytes = data.ToByteArray();
        int totalLength = contentBytes.Length + 2;
        byte[] allBytes = new byte[totalLength];

        ushort id = (ushort)messageId;
        allBytes[0] = (byte)(id >> 8);
        allBytes[1] = (byte)id;

        Array.Copy(contentBytes, 0, allBytes, 2, contentBytes.Length);

        return allBytes;
    }

    /// <summary>
    /// 从字节数组取一个short
    /// </summary>
    private static short GetInt16(byte[] bytes, int offset)
    {
        return (short)(bytes[offset] << 8 | bytes[offset + 1]);
    }

    /// <summary>
    /// 从字节数组取一个int
    /// </summary>
    private static int GetInt32(byte[] bytes, int offset)
    {
        return bytes[offset] << 24 | bytes[offset + 1] << 16 | bytes[offset + 2] << 8 | bytes[offset + 3];
    }
}
