using HT.Framework;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf;
using ProtobufGen;

/// <summary>
/// 网络消息收发，示例
/// </summary>
public class TesNet : HTBehaviour
{
    [SerializeField] private Button btnReqister;
    [SerializeField] private Button btnLogin;

    void Start()
    {
        btnReqister.onClick.AddListener(OnReqister);
        btnLogin.onClick.AddListener(OnLoginClick);

        //监听服务器消息
        NetModule.Instance.AddNetMessageEvent(NetMessageId.ResRegister, OnResRegister);
        NetModule.Instance.AddNetMessageEvent(NetMessageId.ResLogin, OnResLogin);
    }

    /// <summary>
    /// 接收到服务器下发注册信息
    /// </summary>
    /// <param name="message"></param>
    private void OnResRegister(IMessage message)
    {
        ResRegister res = message as ResRegister;

        if (res.ResCode == 0)
            Debug.Log("注册成功");
        else if (res.ResCode == 1)
            Debug.Log("账号不合法");
        else if (res.ResCode == 2)
            Debug.Log("密码不合法");
        else if (res.ResCode == 3)
            Debug.Log("账号已存在");

        Debug.Log($"用户id: {res.UserId}");
        Debug.Log($"用户名: {res.Username}");
    }

    /// <summary>
    /// 接收到服务器下发登录信息
    /// </summary>
    /// <param name="message"></param>
    private void OnResLogin(IMessage message)
    {
        ResLogin res = message as ResLogin;

        if (res.ResCode == 0)
            Debug.Log("登录成功");
        else if (res.ResCode == 1)
            Debug.Log("账号不合法");
        else if (res.ResCode == 2)
            Debug.Log("密码不合法");
        else if (res.ResCode == 3)
            Debug.Log("账号已存在");

        Debug.Log($"用户id: {res.UserId}");
        Debug.Log($"用户名: {res.Username}");
    }

    /// <summary>
    /// 请求注册
    /// </summary>
    private void OnReqister()
    {
        NetModule.Instance.ReqRegister("张三丰", "1234");
    }

    /// <summary>
    /// 请求登录
    /// </summary>
    private void OnLoginClick()
    {
        NetModule.Instance.ReqLogin("张三丰", "1234");
    }
}
