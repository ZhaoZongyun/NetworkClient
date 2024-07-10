using System.Collections.Generic;
using System;
using Google.Protobuf;

/// <summary>
/// 网络消息回调封装类
///     用于 NetMsgEvent<T> 泛型类继承，继承后可被类型转换
/// </summary>
public class NetEvent
{

}

public class NetEvent<T> : NetEvent where T : class, IMessage
{
    private List<Action<T>> _list = new List<Action<T>>();

    public void Add(Action<T> action)
    {
        if (!_list.Contains(action))
        {
            _list.Add(action);
        }
    }

    public void Remove(Action<T> action)
    {
        _list.Remove(action);
    }

    public void Invoke(IMessage data)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            _list[i](data as T);
        }
    }
}